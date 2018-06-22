using System;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

	[SerializeField] private TerrainController terrainController;
    [SerializeField] private GameObject containerPanel;
    [SerializeField] private Text plotHeightText;
    [SerializeField] private Text updateHeatMapText;
    [SerializeField] private Text pointsCountText;
    [SerializeField] private InputField farField;
    
    private bool terrainMapButton;

    public void ActivateHipsometricMap() {
        terrainMapButton = !terrainMapButton;

        if (terrainMapButton) {
            float initial = Time.realtimeSinceStartup;
            terrainController.UpdateHeatMap();
            updateHeatMapText.text = "Mapa hipsimétrico: " + (Time.realtimeSinceStartup - initial) * 1000 + " ms";
        }
        else {
            terrainController.ResetColor();
        }
    }

    public void HideButton() {
        containerPanel.SetActive(!containerPanel.activeSelf);
    }

    public void StartChanges() {
        float initial = Time.realtimeSinceStartup;
        if (farField != null && farField.text != "")
            terrainController.maxHeight = Convert.ToSingle(farField.text);
//        StartCoroutine(terrainController.PlotHeight());
        terrainController.PlotHeight();
        plotHeightText.text = "Mostrar terreno: " + (Time.realtimeSinceStartup - initial) * 1000 + " ms";
        pointsCountText.text = "Pontos: " + terrainController.pointsCount;
        
        if (terrainMapButton)
            terrainController.UpdateHeatMap();
    }
}
