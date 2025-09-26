// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SimpleAll"
{
	Properties
	{
		[Enum(AlphaBlend,10,Additive,1)]_Dst("材质模式", Float) = 1
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("剔除模式", Float) = 0
		[Toggle(_A_R_ON)] _A_R("A_R", Float) = 1
		[HDR]_Maincolor("Maincolor", Color) = (1,1,1,1)
		[Header(MainTex)]_Maintex("Maintex", 2D) = "white" {}
		[Toggle(_ONE_UV_ON)] _one_UV("one_UV", Float) = 0
		_Main_u_speed("Main_u_speed", Float) = 0
		_Main_v_speed("Main_v_speed", Float) = 0
		[Header(MASKTEX)]_MASKTEX("MASKTEX", 2D) = "white" {}
		[Toggle(_ONE_UV_M_ON)] _one_UV_M("one_UV_M", Float) = 0
		[Header(DissovleTex)]_DissovleTex("DissovleTex", 2D) = "white" {}
		[Toggle(_USE_DISSLOVE_ON)] _use_disslove("use_disslove", Float) = 0
		[Toggle(_DISSSC_ON)] _DissSC("Diss,S/C", Float) = 0
		_smooth("smooth", Range( 0.5 , 1)) = 0.5
		_Disspower("Disspower", Float) = 0
		_Dissovle_U_speed("Dissovle_U_speed", Float) = 0
		_Dissovle_V_speed("Dissovle_V_speed", Float) = 0
		[Header(NIUQU_Tex)]_NIUQU_Tex("NIUQU_Tex", 2D) = "white" {}
		[Toggle(_NIUQUONOFF_ON)] _NIUQUONOFF("NIUQU,ON/OFF", Float) = 0
		_NIUQU_Power("NIUQU_Power", Float) = 0
		_Niuqu_U_speed("Niuqu_U_speed", Float) = 0
		_Niuqu_V_speed("Niuqu_V_speed", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull [_CullMode]
		ZWrite Off
		Blend SrcAlpha [_Dst]
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _ONE_UV_ON
		#pragma shader_feature _NIUQUONOFF_ON
		#pragma shader_feature _USE_DISSLOVE_ON
		#pragma shader_feature _DISSSC_ON
		#pragma shader_feature _A_R_ON
		#pragma shader_feature _ONE_UV_M_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv2_tex4coord2;
			float4 vertexColor : COLOR;
		};

		uniform half _Dst;
		uniform half _CullMode;
		uniform sampler2D _Maintex;
		uniform half _Main_u_speed;
		uniform half _Main_v_speed;
		uniform float4 _Maintex_ST;
		uniform half _NIUQU_Power;
		uniform sampler2D _NIUQU_Tex;
		uniform half _Niuqu_U_speed;
		uniform half _Niuqu_V_speed;
		uniform float4 _NIUQU_Tex_ST;
		uniform half _smooth;
		uniform sampler2D _DissovleTex;
		uniform half _Dissovle_U_speed;
		uniform half _Dissovle_V_speed;
		uniform float4 _DissovleTex_ST;
		uniform half _Disspower;
		uniform half4 _Maincolor;
		uniform sampler2D _MASKTEX;
		uniform float4 _MASKTEX_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			half2 appendResult12 = (half2(( _Main_u_speed * _Time.y ) , ( _Main_v_speed * _Time.y )));
			float2 uv_Maintex = i.uv_texcoord * _Maintex_ST.xy + _Maintex_ST.zw;
			half2 appendResult51 = (half2(i.uv2_tex4coord2.x , i.uv2_tex4coord2.y));
			#ifdef _ONE_UV_ON
				half2 staticSwitch52 = ( uv_Maintex + appendResult51 );
			#else
				half2 staticSwitch52 = ( appendResult12 + uv_Maintex );
			#endif
			half2 appendResult179 = (half2(_Niuqu_U_speed , _Niuqu_V_speed));
			float2 uv_NIUQU_Tex = i.uv_texcoord * _NIUQU_Tex_ST.xy + _NIUQU_Tex_ST.zw;
			half2 panner175 = ( 1.0 * _Time.y * appendResult179 + uv_NIUQU_Tex);
			#ifdef _NIUQUONOFF_ON
				half staticSwitch169 = ( _NIUQU_Power * tex2D( _NIUQU_Tex, panner175 ).r );
			#else
				half staticSwitch169 = 0.0;
			#endif
			half nq197 = staticSwitch169;
			half4 tex2DNode1 = tex2D( _Maintex, ( staticSwitch52 + nq197 ) );
			half2 appendResult186 = (half2(_Dissovle_U_speed , _Dissovle_V_speed));
			float2 uv_DissovleTex = i.uv_texcoord * _DissovleTex_ST.xy + _DissovleTex_ST.zw;
			half2 panner183 = ( 1.0 * _Time.y * appendResult186 + ( nq197 + uv_DissovleTex ));
			#ifdef _DISSSC_ON
				half staticSwitch171 = i.uv2_tex4coord2.w;
			#else
				half staticSwitch171 = _Disspower;
			#endif
			half smoothstepResult41 = smoothstep( ( 1.0 - _smooth ) , _smooth , ( ( tex2D( _DissovleTex, panner183 ).r + 1.0 ) - ( staticSwitch171 * 2.0 ) ));
			#ifdef _USE_DISSLOVE_ON
				half staticSwitch47 = smoothstepResult41;
			#else
				half staticSwitch47 = 1.0;
			#endif
			half Rongjie190 = staticSwitch47;
			o.Emission = ( tex2DNode1 * Rongjie190 * _Maincolor * i.vertexColor ).rgb;
			#ifdef _A_R_ON
				half staticSwitch187 = tex2DNode1.a;
			#else
				half staticSwitch187 = tex2DNode1.r;
			#endif
			float2 uv_MASKTEX = i.uv_texcoord * _MASKTEX_ST.xy + _MASKTEX_ST.zw;
			#ifdef _ONE_UV_M_ON
				half2 staticSwitch193 = ( appendResult51 + uv_MASKTEX );
			#else
				half2 staticSwitch193 = uv_MASKTEX;
			#endif
			o.Alpha = ( i.vertexColor.a * _Maincolor.a * staticSwitch187 * tex2D( _MASKTEX, staticSwitch193 ).r * Rongjie190 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
152;530;1431;503;2095.913;725.4384;2.550189;True;True
Node;AmplifyShaderEditor.CommentaryNode;181;-2810.158,-647.6354;Inherit;False;1361.22;385.8978;扭曲;10;166;167;168;169;172;175;174;179;177;176;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-2760.158,-466.3362;Inherit;False;Property;_Niuqu_U_speed;Niuqu_U_speed;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;177;-2758.829,-387.5987;Inherit;False;Property;_Niuqu_V_speed;Niuqu_V_speed;22;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;179;-2564.14,-427.5479;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;174;-2757.067,-597.1648;Inherit;False;0;166;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;175;-2446.823,-545.1917;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;166;-2301.789,-460.5051;Inherit;True;Property;_NIUQU_Tex;NIUQU_Tex;18;1;[Header];Create;True;1;NIUQU_Tex;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;172;-2263.697,-579.3358;Inherit;False;Property;_NIUQU_Power;NIUQU_Power;20;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;167;-1863.954,-527.7875;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;168;-1889.797,-624.8085;Inherit;False;Constant;_Float6;Float 6;43;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;169;-1702.953,-617.6961;Inherit;False;Property;_NIUQUONOFF;NIUQU,ON/OFF;19;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;44;-2824.977,379.8776;Inherit;False;1897.539;553.5128;溶解;19;171;47;48;41;43;42;36;34;37;35;33;39;183;38;186;94;185;184;196;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;197;-1583.071,-385.9896;Inherit;False;nq;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;184;-2806.656,641.5533;Inherit;False;Property;_Dissovle_U_speed;Dissovle_U_speed;16;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;185;-2812.771,727.7333;Inherit;False;Property;_Dissovle_V_speed;Dissovle_V_speed;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;198;-2922.959,447.3665;Inherit;False;197;nq;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;94;-2927.075,519.0681;Inherit;False;0;33;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;196;-2685.323,405.1784;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;186;-2595.241,644.4484;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;30;-2806.188,-1278.837;Inherit;False;1151.51;591.7782;UV流动;11;52;53;5;15;51;12;11;10;14;9;13;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;50;-3642.406,-666.3568;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-2424.803,603.9864;Inherit;False;Property;_Disspower;Disspower;15;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;183;-2536.051,445.6718;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-2729.592,-1134.83;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-2731.663,-1236.465;Inherit;False;Property;_Main_u_speed;Main_u_speed;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-2759.865,-1005.541;Inherit;False;Property;_Main_v_speed;Main_v_speed;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;33;-2348.661,410.9811;Inherit;True;Property;_DissovleTex;DissovleTex;11;1;[Header];Create;True;1;DissovleTex;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;171;-2262.217,674.0524;Inherit;False;Property;_DissSC;Diss,S/C;13;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-2220.284,595.0814;Inherit;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-2228.435,772.3314;Inherit;False;Constant;_Float1;Float 1;11;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2450.446,-1223.8;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-2454.605,-1125.625;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-2043.704,435.4482;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-2036.107,687.2724;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;12;-2243.188,-1171.196;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;51;-2386.668,-836.1641;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1797.112,695.2025;Inherit;False;Property;_smooth;smooth;14;0;Create;True;0;0;0;False;0;False;0.5;0.5;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-2514.421,-1010.456;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;5;-2050.673,-1114.263;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-2166.511,-913.1276;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;43;-1601.835,557.8445;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;136;-1631.665,-115.8333;Inherit;False;854.5182;282.0116;MASK;4;130;192;193;45;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;36;-1826.16,437.601;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;52;-1889.739,-940.2698;Inherit;True;Property;_one_UV;one_UV;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;130;-1607.011,-58.24353;Inherit;False;0;45;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;200;-1517.122,-851.1436;Inherit;False;197;nq;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;41;-1437.486,539.0125;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1370.796,419.6349;Inherit;False;Constant;_Float2;Float 2;14;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;192;-1503.703,58.41986;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;47;-1149.914,485.1422;Inherit;False;Property;_use_disslove;use_disslove;12;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;173;-1258.691,-976.6785;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-908.5502,-474.257;Inherit;True;Property;_Maintex;Maintex;5;1;[Header];Create;True;1;MainTex;0;0;False;0;False;-1;None;3b1bb54513b72984c916f39d8599f486;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;193;-1382.676,-70.74695;Inherit;True;Property;_one_UV_M;one_UV_M;10;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;190;-894.0723,496.9554;Inherit;False;Rongjie;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;45;-1136.09,-63.27232;Inherit;True;Property;_MASKTEX;MASKTEX;9;1;[Header];Create;True;1;MASKTEX;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-66.93941,-132.2552;Inherit;False;Property;_Maincolor;Maincolor;4;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;3;167.5866,-126.8699;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;188;29.18023,-389.6449;Inherit;False;190;Rongjie;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;187;-457.7045,-259.4776;Inherit;False;Property;_A_R;A_R;3;0;Create;True;0;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;142;1294.243,-130.3792;Inherit;False;Property;_CullMode;剔除模式;1;1;[Enum];Create;False;0;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;412.3716,112.2879;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;135;1292.759,-213.4108;Inherit;False;Property;_Dst;材质模式;0;1;[Enum];Create;False;0;2;AlphaBlend;10;Additive;1;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;374.7549,-445.47;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;844.1683,-192.7203;Half;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SimpleAll;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;True;135;0;0;False;-1;0;False;-1;0;True;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;2;-1;-1;-1;0;False;0;0;True;142;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;179;0;176;0
WireConnection;179;1;177;0
WireConnection;175;0;174;0
WireConnection;175;2;179;0
WireConnection;166;1;175;0
WireConnection;167;0;172;0
WireConnection;167;1;166;1
WireConnection;169;1;168;0
WireConnection;169;0;167;0
WireConnection;197;0;169;0
WireConnection;196;0;198;0
WireConnection;196;1;94;0
WireConnection;186;0;184;0
WireConnection;186;1;185;0
WireConnection;183;0;196;0
WireConnection;183;2;186;0
WireConnection;33;1;183;0
WireConnection;171;1;38;0
WireConnection;171;0;50;4
WireConnection;10;0;13;0
WireConnection;10;1;9;0
WireConnection;11;0;14;0
WireConnection;11;1;9;0
WireConnection;34;0;33;1
WireConnection;34;1;35;0
WireConnection;37;0;171;0
WireConnection;37;1;39;0
WireConnection;12;0;10;0
WireConnection;12;1;11;0
WireConnection;51;0;50;1
WireConnection;51;1;50;2
WireConnection;5;0;12;0
WireConnection;5;1;15;0
WireConnection;53;0;15;0
WireConnection;53;1;51;0
WireConnection;43;0;42;0
WireConnection;36;0;34;0
WireConnection;36;1;37;0
WireConnection;52;1;5;0
WireConnection;52;0;53;0
WireConnection;41;0;36;0
WireConnection;41;1;43;0
WireConnection;41;2;42;0
WireConnection;192;0;51;0
WireConnection;192;1;130;0
WireConnection;47;1;48;0
WireConnection;47;0;41;0
WireConnection;173;0;52;0
WireConnection;173;1;200;0
WireConnection;1;1;173;0
WireConnection;193;1;130;0
WireConnection;193;0;192;0
WireConnection;190;0;47;0
WireConnection;45;1;193;0
WireConnection;187;1;1;1
WireConnection;187;0;1;4
WireConnection;137;0;3;4
WireConnection;137;1;2;4
WireConnection;137;2;187;0
WireConnection;137;3;45;1
WireConnection;137;4;190;0
WireConnection;4;0;1;0
WireConnection;4;1;188;0
WireConnection;4;2;2;0
WireConnection;4;3;3;0
WireConnection;0;2;4;0
WireConnection;0;9;137;0
ASEEND*/
//CHKSM=0DD3A60CE72C48142724D2CA7F9AF19D8E93CB7E