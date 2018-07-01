using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TerrainController : MonoBehaviour {
    
//    public float maxHeight = 470;
    public float far = 470;
    public float near;
    public int pointsCount;
    
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

    public void PlotHeight (string fileName = "Assets\\Resources\\result3.xyz") {
//        string[] lines = File.ReadAllLines(fileName);
        string[] lines = File.ReadAllLines("result3.xyz");

        pointsCount = 0;
        foreach (string line in lines) {
            string[] values = line.Split(' ');
            
            int x = int.Parse(values[0]) + offsetX;
            int y = int.Parse(values[1]) + offsetY;
            float z = float.Parse(values[2]);

            // inverte z
            z = 0 - z;
            
            // limita por near e far
            if (z < near || z > far)
                z = 0;
            else
                pointsCount++;
            
            // normaliza
            z = z / far;
            heights[x, y] = z;
        }
        terrain.terrainData.SetHeights(0, 0, heights);
    }
    
    public void PlotHeight2() {
        float[,] heights = DllManager.GetHeight();
        terrain.terrainData.SetHeights(0, 0, heights);
    }
    
//    public IEnumerator PlotHeight (string fileName = "Assets\\Resources\\result3.xyz") {
//        
//        float initial = Time.realtimeSinceStartup;
//        
//        string[] lines = File.ReadAllLines(fileName);
////        string[] lines = File.ReadAllLines("result3.xyz");
//
//        pointsCount = 0;
//        foreach (string line in lines) {
//            string[] values = line.Split(' ');
//            
//            int x = int.Parse(values[0]) + offsetX;
//            int y = int.Parse(values[1]) + offsetY;
//            float z = float.Parse(values[2]);
//            z = Math.Abs(z);
//            z = z / maxHeight;
//
//            if (z <= 1) {
//                heights[x, y] = z;
//                pointsCount++;
//            } else {
//                heights[x, y] = 0;
//            }
//
//            yield return null;
//        }
//        terrain.terrainData.SetHeights(0, 0, heights);
//        plotHeightText.text = "Mostrar terreno: " + (Time.realtimeSinceStartup - initial) * 1000 + " ms";
//        pointsCountText.text = "Pontos: " + pointsCount;
//    }
}
