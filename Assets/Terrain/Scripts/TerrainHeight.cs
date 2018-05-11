using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class TerrainHeight {
	
	public float[,] heights;
	public int resolutionX;
	public int resolutionZ;
	public bool change = false;
	public int function;

	public TerrainHeight(int resolutionX, int resolutionZ) {
		this.resolutionX = resolutionX;
		this.resolutionZ = resolutionZ;
		heights = new float[resolutionX, resolutionZ];
	}

//	public IEnumerator ChangeHeight (int function) {
//		float step = 1 / (float)resolutionX;
//		while (true) {
//			for (int x = 0; x < resolutionX; x++) {
//				for (int z = 0; z < resolutionZ; z++) {
//					if (function == 0)
//						heights[x, z] = MathFunctions.Sine(x * step);
//					else
//						heights[x, z] = 0.5f + MathFunctions.Sine(x * step, z * step);
//				}
//			}
//			yield return null;
//		}
//	}

	public IEnumerator ChangeHeight (float height) {
		float step = 1 / (float)resolutionX;
		while (true) {
			if (change) {
				for (int x = 0; x < resolutionX; x++) {
					for (int z = 0; z < resolutionZ; z++) {
						heights[x, z] = 0.5f + MathFunctions.Sine(x * step, z * step) * height;
					}
				}
			}
			yield return heights;
		}
	}

	public float[,] ChangeHeight (float[,] height, int resX, int resZ, int func) {
		float step = 1 / (float)resX;

		for (int x = 0; x < resX; x++) {
			for (int z = 0; z < resZ; z++) {
				if (func == 0)
					height[x, z] = MathFunctions.Sine(x * step);
				else
					height[x, z] = 0.5f + MathFunctions.Sine(x * step, z * step);
			}
		}
		return height;
	}

	public void StartModifications () {
		Process.Start("C:\\Users\\Nemo\\CLionProjects\\TerrainHeight\\cmake-build-debug\\TerrainHeight.exe");
	}
	
	public float[] ReadHeightMapFile (string fileName) {
		byte[] bytes = File.ReadAllBytes(fileName);
		
		float[] floats = new float[bytes.Length/4];
		for(int i = 0; i < floats.Length; i++)
			floats[i] = BitConverter.ToSingle(bytes, i*4);

		// foreach (float f in floats)
		// 	print(f);

		return floats;
	}
	
	public float[,] ReadHeightMapFile2 (string fileName) {
		byte[] bytes = File.ReadAllBytes(fileName);
		float[] floats = new float[bytes.Length/4];
        
		int size = (int)Math.Sqrt(floats.Length);
		float[,] heights = new float[size, size];
		
		for (int i = 0, k = 0; i < size; i++) {
			for (int j = 0; j < size; j++, k++) {
				heights[i,j] = BitConverter.ToSingle(bytes, k*4);
			}
		}

//		foreach (float f in heights)
//			print(f);
		return heights;
	}
}
