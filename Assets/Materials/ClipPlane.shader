Shader "IppokratisBournellis/SectionPlane"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _section ("Sectionplane", vector) = (90,0,0,1)
        _Color ("Section Color", Color) = (1,1,1,0)
    }
       
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Cull Off
       
        CGPROGRAM
        #pragma surface surf Lambert
     
        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 viewDir;
            float3 worldNormal;
        };
         
        sampler2D _MainTex;
        sampler2D _BumpMap;
        float4 _section;
        fixed4 _Color;
         
        void surf (Input IN, inout SurfaceOutput o)
        {
        float toClip = _section.x * 0.1 * IN.worldPos.x +
                        _section.y * 0.1 * IN.worldPos.y +
                        _section.z * 0.1 * IN.worldPos.z +
                        _section.w;
                           
        clip( toClip);
             
        float fd = dot( IN.viewDir, IN.worldNormal);
     
        if (fd.x > 0)
        {
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
            return;
        }
           
        o.Emission = _Color;
        }
        ENDCG
    }
    Fallback "Diffuse"
   }
