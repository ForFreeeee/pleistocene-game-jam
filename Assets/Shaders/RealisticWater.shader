// =============================================================================
// RealisticWater.shader — Unity 6+ (URP 6000.x) — REWRITE v2
//
// Fonctionnalités :
//   - Vagues Gerstner fluides (déplacement vertex)
//   - Double normal map animée
//   - Couleur shallow / deep basée sur la profondeur
//   - Foam à l'intersection avec d'autres meshes (depth-based)
//   - Réfraction via Opaque Texture
//   - Fresnel + spéculaire
//   - Clip circulaire (basé sur les UV du mesh)
//
// PRÉREQUIS :
//   1. URP Asset : Depth Texture = ON, Opaque Texture = ON
//   2. Plane subdivisé (64×64 min) avec UVs 0-1
//   3. Assigner 2 Normal Maps + 1 texture foam (bruit)
// =============================================================================

Shader "Custom/RealisticWater"
{
    Properties
    {
        [Header(__________ COULEURS __________)]
        [HDR] _ShallowColor ("Couleur peu profonde", Color) = (0.3, 0.8, 0.85, 0.4)
        [HDR] _DeepColor ("Couleur profonde", Color) = (0.02, 0.1, 0.25, 0.9)
        _DepthMaxDistance ("Profondeur max", Range(0.1, 50)) = 5.0

        [Header(__________ NORMAL MAPS __________)]
        _NormalMapA ("Normal Map A", 2D) = "bump" {}
        _NormalMapB ("Normal Map B", 2D) = "bump" {}
        _NormalStrength ("Intensite normales", Range(0, 2)) = 0.3
        _NormalTilingA ("Tiling A", Float) = 2.0
        _NormalTilingB ("Tiling B", Float) = 3.0
        _NormalSpeedA ("Vitesse A (XY)", Vector) = (0.02, 0.03, 0, 0)
        _NormalSpeedB ("Vitesse B (XY)", Vector) = (-0.01, 0.02, 0, 0)

        [Header(__________ VAGUES __________)]
        _WaveA ("Vague A (dirX, dirY, amplitude, longueur)", Vector) = (1, 0, 0.08, 10)
        _WaveB ("Vague B (dirX, dirY, amplitude, longueur)", Vector) = (0, 1, 0.05, 7)
        _WaveC ("Vague C (dirX, dirY, amplitude, longueur)", Vector) = (0.7, 0.7, 0.03, 5)
        _WaveSpeed ("Vitesse vagues", Range(0.1, 5)) = 1.0

        [Header(__________ FOAM __________)]
        _FoamTex ("Texture Foam (bruit)", 2D) = "white" {}
        [HDR] _FoamColor ("Couleur Foam", Color) = (1, 1, 1, 1)
        _FoamWidth ("Largeur Foam", Range(0.01, 5)) = 1.0
        _FoamTiling ("Tiling Foam", Float) = 8.0
        _FoamIntensity ("Intensite Foam", Range(0, 3)) = 1.0

        [Header(__________ REFRACTION __________)]
        _RefractionStrength ("Force refraction", Range(0, 0.1)) = 0.02

        [Header(__________ SPECULAR ET FRESNEL __________)]
        [HDR] _SpecColor2 ("Couleur speculaire", Color) = (1, 1, 1, 1)
        _Smoothness ("Smoothness", Range(0, 1)) = 0.85
        _FresnelPower ("Puissance Fresnel", Range(0.5, 10)) = 4.0

        [Header(__________ FORME CIRCULAIRE __________)]
        [Toggle(_CIRCULAR_CLIP)] _EnableCircularClip ("Activer clip circulaire", Float) = 0
        _CircleRadius ("Rayon (0 a 0.5 en UV)", Range(0.01, 0.5)) = 0.5
        _CircleSoftness ("Douceur bord", Range(0.001, 0.1)) = 0.02
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        // =====================================================================
        // PASS PRINCIPAL — Forward
        // =====================================================================
        Pass
        {
            Name "WaterForward"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex WaterVert
            #pragma fragment WaterFrag
            #pragma shader_feature_local _CIRCULAR_CLIP
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            // -----------------------------------------------------------------
            // Properties
            // -----------------------------------------------------------------
            CBUFFER_START(UnityPerMaterial)
                half4 _ShallowColor;
                half4 _DeepColor;
                float _DepthMaxDistance;

                float _NormalStrength;
                float _NormalTilingA;
                float _NormalTilingB;
                float4 _NormalSpeedA;
                float4 _NormalSpeedB;

                float4 _WaveA;
                float4 _WaveB;
                float4 _WaveC;
                float _WaveSpeed;

                half4 _FoamColor;
                float _FoamWidth;
                float _FoamTiling;
                float _FoamIntensity;

                float _RefractionStrength;

                half4 _SpecColor2;
                float _Smoothness;
                float _FresnelPower;

                float _EnableCircularClip;
                float _CircleRadius;
                float _CircleSoftness;
            CBUFFER_END

            TEXTURE2D(_NormalMapA);  SAMPLER(sampler_NormalMapA);
            TEXTURE2D(_NormalMapB);  SAMPLER(sampler_NormalMapB);
            TEXTURE2D(_FoamTex);    SAMPLER(sampler_FoamTex);

            // -----------------------------------------------------------------
            // Structures
            // -----------------------------------------------------------------
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 positionWS   : TEXCOORD1;
                float3 normalWS     : TEXCOORD2;
                float3 tangentWS    : TEXCOORD3;
                float3 bitangentWS  : TEXCOORD4;
                float4 screenPos    : TEXCOORD5;
                float  fogFactor    : TEXCOORD6;
            };

            // -----------------------------------------------------------------
            // Gerstner Wave
            // -----------------------------------------------------------------
            float3 GerstnerWave(float4 wave, float3 p, float time,
                                inout float3 tangent, inout float3 binormal)
            {
                float2 d = normalize(wave.xy);
                float amp = wave.z;
                float wl = max(wave.w, 0.001);

                float k = TWO_PI / wl;
                float c = sqrt(9.81 / k);
                float f = k * (dot(d, p.xz) - c * time * _WaveSpeed);
                float Q = 0.35;

                float cosF = cos(f);
                float sinF = sin(f);

                // Deplacement
                float3 disp;
                disp.x = Q * amp * d.x * cosF;
                disp.y = amp * sinF;
                disp.z = Q * amp * d.y * cosF;

                // Derivees pour les normales
                tangent += float3(
                    -d.x * d.x * Q * amp * k * sinF,
                     d.x * amp * k * cosF,
                    -d.x * d.y * Q * amp * k * sinF
                );
                binormal += float3(
                    -d.x * d.y * Q * amp * k * sinF,
                     d.y * amp * k * cosF,
                    -d.y * d.y * Q * amp * k * sinF
                );

                return disp;
            }

            // -----------------------------------------------------------------
            // Vertex Shader
            // -----------------------------------------------------------------
            Varyings WaterVert(Attributes input)
            {
                Varyings o = (Varyings)0;

                float3 posWS = TransformObjectToWorld(input.positionOS.xyz);
                float time = _Time.y;

                float3 T = float3(1, 0, 0);
                float3 B = float3(0, 0, 1);

                float3 disp = float3(0, 0, 0);
                disp += GerstnerWave(_WaveA, posWS, time, T, B);
                disp += GerstnerWave(_WaveB, posWS, time, T, B);
                disp += GerstnerWave(_WaveC, posWS, time, T, B);

                posWS += disp;

                float3 N = normalize(cross(B, T));

                o.positionWS   = posWS;
                o.positionCS   = TransformWorldToHClip(posWS);
                o.normalWS     = N;
                o.tangentWS    = normalize(T);
                o.bitangentWS  = normalize(B);
                o.uv           = input.uv;
                o.screenPos    = ComputeScreenPos(o.positionCS);
                o.fogFactor    = ComputeFogFactor(o.positionCS.z);

                return o;
            }

            // -----------------------------------------------------------------
            // Fragment Shader
            // -----------------------------------------------------------------
            half4 WaterFrag(Varyings i) : SV_Target
            {
                // =============================================
                // 1. CLIP CIRCULAIRE (base sur les UVs)
                // =============================================
                #ifdef _CIRCULAR_CLIP
                {
                    float2 centeredUV = i.uv - 0.5;
                    float dist = length(centeredUV);
                    clip(_CircleRadius - dist);
                }
                #endif

                float time = _Time.y;

                // =============================================
                // 2. SCREEN UV + PROFONDEUR
                // =============================================
                float2 screenUV = i.screenPos.xy / i.screenPos.w;

                float rawDepth = SampleSceneDepth(screenUV);
                float sceneEyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);
                float surfaceEyeDepth = i.screenPos.w;
                float depthDiff = max(0.0, sceneEyeDepth - surfaceEyeDepth);

                // =============================================
                // 3. COULEUR EAU (shallow vers deep)
                // =============================================
                float depthFactor = saturate(depthDiff / _DepthMaxDistance);
                half4 waterColor = lerp(_ShallowColor, _DeepColor, depthFactor);

                // =============================================
                // 4. NORMAL MAPS ANIMEES
                // =============================================
                float2 uvA = i.positionWS.xz * _NormalTilingA + _NormalSpeedA.xy * time;
                float2 uvB = i.positionWS.xz * _NormalTilingB + _NormalSpeedB.xy * time;

                float3 nA = UnpackNormalScale(
                    SAMPLE_TEXTURE2D(_NormalMapA, sampler_NormalMapA, uvA),
                    _NormalStrength);
                float3 nB = UnpackNormalScale(
                    SAMPLE_TEXTURE2D(_NormalMapB, sampler_NormalMapB, uvB),
                    _NormalStrength);

                // Blend RNM (Reoriented Normal Mapping)
                float3 normalTS = normalize(float3(nA.xy + nB.xy, nA.z * nB.z));

                // Tangent vers World
                float3x3 TBN = float3x3(
                    normalize(i.tangentWS),
                    normalize(i.bitangentWS),
                    normalize(i.normalWS)
                );
                float3 normalWS = normalize(mul(normalTS, TBN));

                // =============================================
                // 5. REFRACTION
                // =============================================
                float2 refrUV = screenUV + normalTS.xy * _RefractionStrength;
                // Securite : ne pas sampler au-dessus de la surface
                float refrDepthRaw = SampleSceneDepth(refrUV);
                float refrEyeDepth = LinearEyeDepth(refrDepthRaw, _ZBufferParams);
                refrUV = (refrEyeDepth < surfaceEyeDepth) ? screenUV : refrUV;
                half3 sceneColor = SampleSceneColor(refrUV);

                // =============================================
                // 6. FRESNEL
                // =============================================
                float3 viewDir = normalize(GetWorldSpaceViewDir(i.positionWS));
                float fresnel = pow(1.0 - saturate(dot(normalWS, viewDir)), _FresnelPower);

                // Melange scene refractee + couleur eau
                half3 color = lerp(sceneColor * waterColor.rgb, waterColor.rgb, fresnel);

                // =============================================
                // 7. SPECULAIRE (Blinn-Phong)
                // =============================================
                Light mainLight = GetMainLight();
                float3 H = normalize(mainLight.direction + viewDir);
                float NdotH = saturate(dot(normalWS, H));
                float specPower = exp2(10.0 * _Smoothness + 1.0);
                half3 spec = _SpecColor2.rgb * pow(NdotH, specPower) * mainLight.color;
                color += spec;

                // =============================================
                // 8. FOAM (intersection depth-based)
                // =============================================
                // Masque : plus depthDiff est petit, plus on est proche
                float foamMask = 1.0 - saturate(depthDiff / _FoamWidth);

                // Deux couches de bruit animees differemment
                float2 fUV1 = i.positionWS.xz * _FoamTiling
                            + float2(time * 0.03, time * 0.02);
                float2 fUV2 = i.positionWS.xz * _FoamTiling * 0.8
                            + float2(-time * 0.02, time * 0.04);
                half noise1 = SAMPLE_TEXTURE2D(_FoamTex, sampler_FoamTex, fUV1).r;
                half noise2 = SAMPLE_TEXTURE2D(_FoamTex, sampler_FoamTex, fUV2).r;
                half foamNoise = min(noise1, noise2);

                // Le foam n apparait QUE la ou foamMask > 0
                float foam = foamMask - (1.0 - foamNoise) * 0.5;
                foam = saturate(foam * _FoamIntensity);

                // Appliquer le foam par-dessus la couleur
                color = lerp(color, _FoamColor.rgb, foam);

                // =============================================
                // 9. ALPHA + BORD DOUX CIRCULAIRE
                // =============================================
                float alpha = lerp(_ShallowColor.a, _DeepColor.a, depthFactor);
                alpha = saturate(alpha + foam * 0.3);

                #ifdef _CIRCULAR_CLIP
                {
                    float2 centeredUV = i.uv - 0.5;
                    float dist = length(centeredUV);
                    float edgeFade = 1.0 - smoothstep(
                        _CircleRadius - _CircleSoftness,
                        _CircleRadius,
                        dist);
                    alpha *= edgeFade;
                }
                #endif

                // Fog
                color = MixFog(color, i.fogFactor);

                return half4(color, alpha);
            }
            ENDHLSL
        }

        // =====================================================================
        // PASS DEPTH
        // =====================================================================
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }

            ZWrite On
            ColorMask R
            Cull Back

            HLSLPROGRAM
            #pragma vertex DepthVert
            #pragma fragment DepthFrag
            #pragma shader_feature_local _CIRCULAR_CLIP

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _ShallowColor; half4 _DeepColor; float _DepthMaxDistance;
                float _NormalStrength; float _NormalTilingA; float _NormalTilingB;
                float4 _NormalSpeedA; float4 _NormalSpeedB;
                float4 _WaveA; float4 _WaveB; float4 _WaveC; float _WaveSpeed;
                half4 _FoamColor; float _FoamWidth; float _FoamTiling; float _FoamIntensity;
                float _RefractionStrength;
                half4 _SpecColor2; float _Smoothness; float _FresnelPower;
                float _EnableCircularClip; float _CircleRadius; float _CircleSoftness;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
            };

            float3 GerstnerSimple(float4 w, float3 p, float t)
            {
                float2 d = normalize(w.xy);
                float wl = max(w.w, 0.001);
                float k = TWO_PI / wl;
                float c = sqrt(9.81 / k);
                float f = k * (dot(d, p.xz) - c * t * _WaveSpeed);
                float Q = 0.35;
                return float3(
                    Q * w.z * d.x * cos(f),
                    w.z * sin(f),
                    Q * w.z * d.y * cos(f)
                );
            }

            Varyings DepthVert(Attributes input)
            {
                Varyings o;
                float3 posWS = TransformObjectToWorld(input.positionOS.xyz);
                float t = _Time.y;
                posWS += GerstnerSimple(_WaveA, posWS, t);
                posWS += GerstnerSimple(_WaveB, posWS, t);
                posWS += GerstnerSimple(_WaveC, posWS, t);
                o.positionCS = TransformWorldToHClip(posWS);
                o.uv = input.uv;
                return o;
            }

            half4 DepthFrag(Varyings input) : SV_Target
            {
                #ifdef _CIRCULAR_CLIP
                    float2 c = input.uv - 0.5;
                    clip(_CircleRadius - length(c));
                #endif
                return 0;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
