using UnityEngine;
using System.Collections;
using LibNoise.Generator;
using SimplexNoise;

public class BasicNoiseProvider : INoiseProvider {

	private Perlin PerlinNoiseGenerator = new Perlin();

	public BasicNoiseProvider(){		
	}

	#region INoiseProvider implementation

	public float GetValue (float x, float z)
	{
//		float result = truenoise(new Vector3(x,0,z),8, 0.8f, 0.003f);
//		return result;
		float baseHeight = Fbm2DNoise(new Vector3(x,0,z) + World.Current.Grain0Offset,8, 96, 0.7f);
		baseHeight *= ((Fbm2DNoise(new Vector3(x,0,z) + World.Current.Grain0Offset,8, 64, 0.7f) / 2) + 0.5f);
		baseHeight *= ((Fbm2DNoise(new Vector3(x,0,z) + World.Current.Grain0Offset,8, 32, 0.7f) / 2) + 0.5f);
		baseHeight *= ((Fbm2DNoise(new Vector3(x,0,z) + World.Current.Grain0Offset,4, 16, 0.7f) / 2) + 0.5f);
		//baseHeight *= ((Fbm2DNoise(new Vector3(x,0,z) + World.Current.Grain0Offset,8, 8, 0.7f) / 2) + 0.5f);
		return baseHeight;
		//return Fbm2DNoise(new Vector3(x,0,z) + World.Current.Grain0Offset,8, 64, 0.7f);
		//return 
		//return Mathf.PerlinNoise(x,z);
		//return (float)(PerlinNoiseGenerator.GetValue(x, 0, z) / 2f) + 0.5f;
	}

	#endregion

	public float Fbm2DNoise(Vector3 pos, int o, float f, float g) {
		float NoiseX = Mathf.Abs(pos.x + World.Current.Grain0Offset.x);
		float NoiseZ = Mathf.Abs(pos.z + World.Current.Grain0Offset.z);
		float result = 0;
		float gain = g;
		float frequency = 1.0f / f;
		float amplitude = gain;
		float lacunarity = 2.0f;

		for (int i = 0; i < o; i++) {
			result += Noise.Generate((float)NoiseX * frequency, (float)NoiseZ * frequency) * amplitude;
			frequency *= lacunarity;
			amplitude *= gain;
		}
		return result;
	}

	float truenoise(Vector3 position, int octaves, float frequency, float persistence) {
		float total = 0.0f;
		float maxAmplitude = 0.0f;
		float amplitude = 1.0f;
		for (int i = 0; i < octaves; i++) {
			total += Noise.Generate(position.x * frequency,position.y * frequency, position.z * frequency) * amplitude;
			frequency *= 2.0f;
			maxAmplitude += amplitude;
			amplitude *= persistence;
		}
		return total / maxAmplitude;
	}

}
