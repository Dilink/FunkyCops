// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader_dancefloor"
{
	Properties
	{
		_Image_alltiles("Image_alltiles", 2D) = "white" {}
		_Trame("Trame", 2D) = "white" {}
		[Toggle(_USE_TEXT_GEN_ON)] _Use_text_gen("Use_text_gen", Float) = 0
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _USE_TEXT_GEN_ON
		#pragma surface surf StandardCustomLighting keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv2_texcoord2;
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _Image_alltiles;
		uniform sampler2D _Trame;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			c.rgb = 0;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			float4 temp_cast_0 = (0.49).xxxx;
			float4 color19 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			#ifdef _USE_TEXT_GEN_ON
				float4 staticSwitch16 = color19;
			#else
				float4 staticSwitch16 = tex2D( _Image_alltiles, i.uv2_texcoord2 );
			#endif
			float2 uv_TexCoord2 = i.uv_texcoord * float2( 3,3 );
			float4 tex2DNode5 = tex2D( _Trame, uv_TexCoord2 );
			float4 temp_output_7_0 = step( temp_cast_0 , ( staticSwitch16 * tex2DNode5 ) );
			float4 temp_cast_1 = (0.49).xxxx;
			float4 lerpResult13 = lerp( ( temp_output_7_0 * 2.82 ) , ( tex2DNode5 * 0.06 ) , ( 1.0 - temp_output_7_0 ));
			o.Emission = lerpResult13.rgb;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
0;0;1920;1019;1479.535;602.8479;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1294.286,-399.258;Float;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1136.877,144.5068;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-961.6196,-427.6622;Float;True;Property;_Image_alltiles;Image_alltiles;0;0;Create;True;0;0;False;0;4f9b9ea0ec1a1504ca4296b9f6e9076b;4f9b9ea0ec1a1504ca4296b9f6e9076b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;19;-862.8627,-226.5556;Float;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;16;-611.0414,-316.392;Float;False;Property;_Use_text_gen;Use_text_gen;2;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;5;-777.877,158.5068;Float;True;Property;_Trame;Trame;1;0;Create;True;0;0;False;0;d61fdecf04c1ff848bd486d0048e53b0;d61fdecf04c1ff848bd486d0048e53b0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-305.7778,-55.46136;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-246.7778,-198.4614;Float;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;0.49;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-511.8232,409.8814;Float;False;Constant;_Float2;Float 2;2;0;Create;True;0;0;False;0;0.06;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;178.463,16.4658;Float;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;2.82;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;7;-78.77783,-77.46136;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-75.12231,505.5565;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;432.5575,-41.04529;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;14;225.4718,167.5546;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;11;-133.5336,266.4889;Float;True;Darken;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;13;730.2302,120.9073;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1001.356,-78.49928;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Shader_dancefloor;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;1;1;0
WireConnection;16;1;4;0
WireConnection;16;0;19;0
WireConnection;5;1;2;0
WireConnection;6;0;16;0
WireConnection;6;1;5;0
WireConnection;7;0;8;0
WireConnection;7;1;6;0
WireConnection;15;0;5;0
WireConnection;15;1;12;0
WireConnection;9;0;7;0
WireConnection;9;1;10;0
WireConnection;14;0;7;0
WireConnection;11;0;12;0
WireConnection;11;1;5;0
WireConnection;13;0;9;0
WireConnection;13;1;15;0
WireConnection;13;2;14;0
WireConnection;0;2;13;0
ASEEND*/
//CHKSM=0E47C8816F90F0C465743AA4E19041BE5A7A93CB