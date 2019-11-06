// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UnityCoder/MatrixPlayground"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_rX ("RotateX", float) = 0
		_rY ("RotateY", float) = 0
		_rZ ("RotateZ", float) = 0
		_enableXSwirl ("Enable X Swirl", float) = 0
		_enableYSwirl ("Enable Y Swirl", float) = 0
		_enableZSwirl ("Enable Z Swirl", float) = 0
		_Displacement ("Displacement", float) = 0
		_Exp ("Exponetial", Int) = 2

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		//Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _tX,_tY,_tZ; 
			float _sX,_sY,_sZ;
			float _rX,_rY,_rZ;

			float _enableXSwirl;
			float _enableYSwirl;
			float _enableZSwirl;
			float _Displacement;
			int _Exp =2;

			v2f vert (appdata v)
			{
				v2f o;


  				float4 localVertexPos = v.vertex;


  				if(_enableXSwirl != 0){
					float1 height = v.vertex.x * 20;
					int1 e = 1;
					float1 sinTime = pow (sin(_Time.w + height), 10);
					float angleX = radians(_rX) + sinTime * _enableXSwirl;
					float c = cos(angleX);
					float s = sin(angleX);
					float4x4 rotateXMatrix	= float4x4(	1,	0,	0,	0,
												 		0,	c,	-s,	0,
									  					0,	s,	c,	0,
									  					0,	0,	0,	1);
	  				localVertexPos = mul(localVertexPos,rotateXMatrix);
				}

				if(_enableYSwirl != 0){
					float1 height = v.vertex.y * _Displacement;
					float1 sinTime = pow (sin(_Time.w + height), 5);
					float1 angleY = radians(_rY) + sinTime * _enableYSwirl;
					float1 c = cos(angleY);
					float1 s = sin(angleY);
				 	float4x4 rotateYMatrix	= float4x4(	c,	0,	s,	0,
												 		0,	1,	0,	0,
									  					-s,	0,	c,	0,
									  					0,	0,	0,	1);
  					localVertexPos = mul(localVertexPos,rotateYMatrix);
				}

				if(_enableZSwirl != 0){
					float1 height = v.vertex.z;
					float1 sinTime = pow (sin(_Time.w + height), 2);
					float1 angleZ = radians(_rZ) + sinTime * _enableZSwirl;
					float1 c = cos(angleZ);
					float1 s = sin(angleZ);
					float4x4 rotateZMatrix	= float4x4(	c,	-s,	0,	0,
												 		s,	c,	0,	0,
									  					0,	0,	1,	0,
									  					0,	0,	0,	1);
  					localVertexPos = mul(localVertexPos, rotateZMatrix);
				}

				o.vertex = UnityObjectToClipPos(localVertexPos);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}