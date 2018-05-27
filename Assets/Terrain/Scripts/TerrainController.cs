using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TerrainController : MonoBehaviour {

	private Terrain _terrain;
    private static int _resolution;
	private static float[,] _heights;
    private static int resolutionX = 640;
    private static int resolutionY = 640;
    private static float[,] heights = new float[resolutionX, resolutionY];
    
    private void Start () {        
		_terrain = GetComponent<Terrain>();
        _resolution = _terrain.terrainData.heightmapHeight;
		_heights = _terrain.terrainData.GetHeights(0, 0, _resolution, _resolution);

        ResetHeight();
        ResetColor();

        GetHeight("Assets\\Resources\\result3.xyz");
        
//        StartModifications();
//        StartCoroutine(ReadHeightMapFile("Assets\\Resources\\height_map.txt"));
//        StartCoroutine(ChangeHeight(.5f));
    }

	private void ResetHeight () {
        for (int x = 0; x < _resolution; x++) {
            for (int z = 0; z < _resolution; z++)
//                _heights[x, z] = .5f;
                _heights[x, z] = 0;
        }
        _terrain.terrainData.SetHeights(0, 0, _heights);
    }
    
    public void UpdateHeatMap () {
        HideDirt();
        float[, ,] alphaMap = _terrain.terrainData.GetAlphamaps(0, 0, _terrain.terrainData.alphamapWidth, _terrain.terrainData.alphamapHeight);
        /*while(true) {
            for (int x = 0; x < _resolution-1; x++) {
                for (int y = 0; y < _resolution-1; y++) {
                    alphaMap[x, y, 0] = 1.0f - _heights[x, y];
                    alphaMap[x, y, 1] = _heights[x, y];
                }
            }
            _terrain.terrainData.SetAlphamaps(0, 0, alphaMap); 
            yield return null;
        }*/
        
        for (int x = 0; x < resolutionX-1; x++) {
            for (int y = 0; y < resolutionY-1; y++) {
                 alphaMap[x, y, 0] = 1.0f - heights[x, y];
                 alphaMap[x, y, 1] = heights[x, y];
            }
        }
            _terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
    }

    public void ResetColor () {
        float[, ,] alphaMap = _terrain.terrainData.GetAlphamaps(0, 0, _terrain.terrainData.alphamapWidth, _terrain.terrainData.alphamapHeight);
        
        /*for (int x = 0; x < _heights.GetLength(0)-1; x++) {
            for (int y = 0; y < _heights.GetLength(1)-1; y++) {
                alphaMap[x, y, 0] = 0;
                alphaMap[x, y, 1] = 0;
                alphaMap[x, y, 2] = 1;
            }
        }*/
        
        for (int x = 0; x < resolutionX-1; x++) {
            for (int y = 0; y < resolutionY-1; y++) {
                alphaMap[x, y, 0] = 0;
                alphaMap[x, y, 1] = 0;
                alphaMap[x, y, 2] = 1;
            }
        }
        _terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
    }

    private void HideDirt () {
        float[, ,] alphaMap = _terrain.terrainData.GetAlphamaps(0, 0, _terrain.terrainData.alphamapWidth, _terrain.terrainData.alphamapHeight);
        
        /*for (int x = 0; x < _heights.GetLength(0)-1; x++) {
            for (int y = 0; y < _heights.GetLength(1)-1; y++) {
                alphaMap[x, y, 2] = 0;
            }
        }*/
        
        for (int x = 0; x < resolutionX-1; x++) {
            for (int y = 0; y < resolutionY-1; y++) {
                alphaMap[x, y, 2] = 0;
            }
        }
        _terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
    }

    private IEnumerator ReadHeightMapFile (string fileName) {
        byte[] bytes = File.ReadAllBytes(fileName);

        int size = bytes.Length/4;
        size = (int)Math.Sqrt(size);
        _heights = new float[size, size];

        while (true) {
            try {
                bytes = File.ReadAllBytes(fileName);
                for (int i = 0, k = 0; i < size; i++) {
                    for (int j = 0; j < size; j++, k++) {
                        _heights[i, j] = BitConverter.ToSingle(bytes, k * 4)/10;
                    }
                }
                _terrain.terrainData.SetHeights(0, 0, _heights);
            } catch (IOException) {
                
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    private void StartModifications () {
		Process.Start("C:\\Users\\Nemo\\CLionProjects\\TerrainHeight\\cmake-build-debug\\TerrainHeight.exe");
	}
    
    public IEnumerator ChangeHeight (float height) {
        float step = 1 / (float)_resolution;
        float t = 0;
        while (true) {
            t += step;
            for (int x = 0; x < _resolution; x++) {
                for (int z = 0; z < _resolution; z++) {
                    _heights[x, z] = 0.5f + MathFunctions.Sine(x * step, z * step, t) * height;
                }
            }
            _terrain.terrainData.SetHeights(0, 0, _heights);
            yield return null;
        }
    }
    public void GetHeight (string fileName, float max = 470) {
        string[] lines = File.ReadAllLines(fileName);
        foreach (string line in lines) {
            string[] values = line.Split(' ');
            
            int x = Int32.Parse(values[0]);
            int y = Int32.Parse(values[1]);
            float z = float.Parse(values[2]);
            z = Math.Abs(z);
            heights[x, y] = z / max >= 1 ? 0 : z/max;
        }
        _terrain.terrainData.SetHeights(0, 0, heights);
    }
}
