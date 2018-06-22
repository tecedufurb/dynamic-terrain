using UnityEngine;

public class CanvasController : MonoBehaviour {

	[SerializeField] private TerrainController terrainController;
    [SerializeField] private GameObject containerPanel;

    private bool terrainMapButton;
    private bool sineFunctionButton;
    private bool playButton;
	private bool kinematic = true;
    private Coroutine terrainMapCoroutine;

	public void ActivateHipsometricMap() {
        terrainMapButton = !terrainMapButton;

        if(terrainMapButton)
            terrainController.UpdateHeatMap();
        else
            terrainController.ResetColor();
        
    }

    public void HideButton() {
        if (containerPanel.activeSelf)
            containerPanel.SetActive(false);
        else
            containerPanel.SetActive(true);
    }

    public void StartChanges() {
        terrainController.GetHeight();
    }
}
