using System.Collections;
using UnityEngine;

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

	public IEnumerator ChangeHeight(int function) {
		float step = 1 / (float)resolutionX;
		while (true) {
			for (int x = 0; x < resolutionX; x++) {
				for (int z = 0; z < resolutionZ; z++) {
					if (function == 0)
						heights[x, z] = MathFunctions.Sine(x * step);
					else
						heights[x, z] = 0.5f + MathFunctions.Sine(x * step, z * step);
				}
			}
			yield return null;
		}
	}

	public IEnumerator ChangeHeight(float height) {
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

	public float[,] ChangeHeight (float[,] height, int resolutionX, int resolutionZ, int function) {
		float step = 1 / (float)resolutionX;

		for (int x = 0; x < resolutionX; x++) {
			for (int z = 0; z < resolutionZ; z++) {
				if (function == 0)
					height[x, z] = MathFunctions.Sine(x * step);
				else
					height[x, z] = 0.5f + MathFunctions.Sine(x * step, z * step);
			}
		}
		return height;
	}
}
