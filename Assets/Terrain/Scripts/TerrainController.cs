using System;
using System.IO;
using UnityEngine;

public class TerrainController : MonoBehaviour {

    private float maxHeight = 470;
    private int offsetX = 200;
    private int offsetY = 250;
    
	private Terrain terrain;
    private static int resolution;
	private static float[,] heights;
    
    private void Start () {        
		terrain = GetComponent<Terrain>();
        resolution = terrain.terrainData.heightmapHeight;
		heights = terrain.terrainData.GetHeights(0, 0, resolution, resolution);

        ResetHeight();
        ResetColor();
    }

	private void ResetHeight () {
        for (int x = 0; x < resolution-1; x++) {
            for (int z = 0; z < resolution-1; z++) {
                heights[x, z] = 0;
            }
        }
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void ResetColor () {
        float[, ,] alphaMap = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight);
        
        for (int x = 0; x < resolution-1; x++) {
            for (int y = 0; y < resolution-1; y++) {
                alphaMap[x, y, 0] = 0;
                alphaMap[x, y, 1] = 0;
                alphaMap[x, y, 2] = 1;
            }
        }
        terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
    }
    
    public void UpdateHeatMap() {
        HideDirt();
        float[,,] alphaMap = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth,
            terrain.terrainData.alphamapHeight);

        for (int x = 0; x < resolution - 1; x++) {
            for (int y = 0; y < resolution - 1; y++) {
                alphaMap[x, y, 0] = 1.0f - heights[x, y];
                alphaMap[x, y, 1] = heights[x, y];
            }
        }

        terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
    }

    private void HideDirt () {
        float[, ,] alphaMap = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight);
        
        for (int x = 0; x < resolution-1; x++) {
            for (int y = 0; y < resolution-1; y++) {
                alphaMap[x, y, 2] = 0;
            }
        }
        terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
    }

    public void GetHeight (string fileName = "Assets\\Terrain\\Resources\\result3.xyz") {
        string[] lines = File.ReadAllLines(fileName);
        foreach (string line in lines) {
            string[] values = line.Split(' ');
            
            int x = Int32.Parse(values[0]);
            int y = Int32.Parse(values[1]);
            float z = float.Parse(values[2]);
            z = Math.Abs(z);
            heights[x+offsetX, y+offsetY] = (z / maxHeight) >= 1 ? 0 : (z / maxHeight);
        }
        terrain.terrainData.SetHeights(0, 0, heights);
    }
}
