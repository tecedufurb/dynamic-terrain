using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class TerrainController : MonoBehaviour {

    // public FunctionOption function;
    public TerrainHeight utils;
	private Terrain _terrain;
	private static int _resolutionX;
    private static int _resolutionZ;
	private static float[,] _heights;
    // private Coroutine _changeHeight;
    // private bool _changed = false;

    void Start () {        
		_terrain = GetComponent<Terrain>();
		_resolutionX = _terrain.terrainData.heightmapWidth;
        _resolutionZ = _terrain.terrainData.heightmapHeight;
		_heights = _terrain.terrainData.GetHeights(0, 0, _resolutionX, _resolutionZ);

        utils = new TerrainHeight(_resolutionX, _resolutionZ);

        StartModifications();
        
//        utils.StartModifications();
//        utils.readMatrixFromFile("Assets\\Resources\\height_map.txt");
//        utils.ReadHeightMapFile2("Assets\\Resources\\height_map.txt");

        ResetHeight();
        ResetColor();
        StartCoroutine(ReadHeightMapFile("Assets\\Resources\\height_map.txt"));
//        StartCoroutine(utils.ChangeHeight(.5f));
	}

//	void Update () {
//        if (utils.change)
//            _terrain.terrainData.SetHeights(0, 0, utils.heights);
//    }

    public void StartChanges () {
        utils.change = true;
    }

    public void StopChanges () {
        utils.change = false;
    }

	private void ResetHeight () {
        for (int x = 0; x < _resolutionX; x++) {
            for (int z = 0; z < _resolutionZ; z++)
                _heights[x, z] = .5f;
        }
        _terrain.terrainData.SetHeights(0, 0, _heights);
    }
    
    public IEnumerator UpdateHeatMap () {
        HideDirt();
        float[, ,] alphaMap = _terrain.terrainData.GetAlphamaps(0, 0, _terrain.terrainData.alphamapWidth, _terrain.terrainData.alphamapHeight);
        while(true) {
            for (int x = 0; x < _resolutionX-1; x++) {
                for (int y = 0; y < _resolutionZ-1; y++) {
                    alphaMap[x, y, 0] = 1.0f - utils.heights[x, y];
                    alphaMap[x, y, 1] = utils.heights[x, y];
                }
            }
            _terrain.terrainData.SetAlphamaps(0, 0, alphaMap); 
            yield return null;
        }
    }

    public void ResetColor () {
        float[, ,] alphaMap = _terrain.terrainData.GetAlphamaps(0, 0, _terrain.terrainData.alphamapWidth, _terrain.terrainData.alphamapHeight);
        
        for (int x = 0; x < _heights.GetLength(0)-1; x++) {
            for (int y = 0; y < _heights.GetLength(1)-1; y++) {
                alphaMap[x, y, 0] = 0;
                alphaMap[x, y, 1] = 0;
                alphaMap[x, y, 2] = 1;
            }
        }
        _terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
    }

    private void HideDirt () {
        float[, ,] alphaMap = _terrain.terrainData.GetAlphamaps(0, 0, _terrain.terrainData.alphamapWidth, _terrain.terrainData.alphamapHeight);
        
        for (int x = 0; x < _heights.GetLength(0)-1; x++) {
            for (int y = 0; y < _heights.GetLength(1)-1; y++) {
                alphaMap[x, y, 2] = 0;
            }
        }
        _terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
    }

    private IEnumerator ReadHeightMapFile (string fileName) {
        byte[] bytes = File.ReadAllBytes(fileName);

        int size = bytes.Length/4;
        size = (int)Math.Sqrt(size);
        float[,] heights = new float[size, size];

        while (true) {
            try {
                bytes = File.ReadAllBytes(fileName);
            } catch (IOException) {
                continue;
            }
            for (int i = 0, k = 0; i < size; i++) {
                for (int j = 0; j < size; j++, k++) {
                    heights[i, j] = BitConverter.ToSingle(bytes, k * 4)/10;
                }
            }
            _terrain.terrainData.SetHeights(0, 0, heights);
            yield return null;
        }
    }

    private void StartModifications () {
		Process.Start("C:\\Users\\Nemo\\CLionProjects\\TerrainHeight\\cmake-build-debug\\TerrainHeight.exe");
	}
    
//    public void ResetCollider () {
//        GetComponent<TerrainCollider>().enabled = false;
//        GetComponent<TerrainCollider>().enabled = true;
//    }
}
