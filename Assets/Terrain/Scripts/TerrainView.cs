using UnityEngine;
using UnityEngine.UI;

public class TerrainView : MonoBehaviour {

	public TerrainController terrainController;
    public GameController gameController;
    public GameObject containerPanel;
	public Text playButtonText;
	public Text gravityButtonText;
    public Button addButton;
    public Button removeButton;
    public Button moveButton;

    private bool terrainMapButton;
    private bool sineFunctionButton;
    private bool playButton;
	private bool kinematic = true;
    private Coroutine terrainMapCoroutine;

	public void ActivateHipsometricMap() {
        terrainMapButton = !terrainMapButton;

        if(terrainMapButton) {
            terrainMapCoroutine = StartCoroutine(terrainController.UpdateHeatMap());
        } else {
            StopCoroutine(terrainMapCoroutine);
            terrainController.ResetColor();
        }
    }

    public void ActivateSineFunction() {
        // terrainController.ResetCollider();
        gameController.ResetObjectsCollider();
        sineFunctionButton = !sineFunctionButton;
		playButton = false;
        if (sineFunctionButton) {
            terrainController.utils.function = (int)FunctionOption.Sine;
            terrainController.StartChanges();
        } else {
            terrainController.StopChanges();
        }            
    }

    public void ChangeTerrainFunction() {
        // terrainController.ResetCollider();
        gameController.ResetObjectsCollider();
		playButton = !playButton;
        sineFunctionButton = false;
		if (playButton) {
            terrainController.utils.function = (int)FunctionOption.ComplexSine;
            terrainController.StartChanges();
			playButtonText.text = "Pausar";
        } else {
            terrainController.StopChanges();
			playButtonText.text = "Iniciar";
        }
    }

    public void HideButton() {
        if(containerPanel.activeSelf)
            containerPanel.SetActive(false);
        else
            containerPanel.SetActive(true);
    }

	public void ActivateGravity () {
		kinematic = !kinematic;
		gameController.ActivateGravity(kinematic);
		if (kinematic)
			gravityButtonText.text = "Ativar gravidade";
		else
			gravityButtonText.text = "Desativar gravidade";
	}

	public void AddBuildingsButton () {
		gameController.Add();
        addButton.GetComponent<Image>().color = Color.gray;
        removeButton.GetComponent<Image>().color = Color.white;
        moveButton.GetComponent<Image>().color = Color.white;
	}

    public void RemoveBuildingsButton () {
		gameController.Remove();
        removeButton.GetComponent<Image>().color = Color.gray;
        addButton.GetComponent<Image>().color = Color.white;
        moveButton.GetComponent<Image>().color = Color.white;
	}

    public void MoveBuildingsButton () {
		gameController.Move();
        moveButton.GetComponent<Image>().color = Color.gray;
        addButton.GetComponent<Image>().color = Color.white;
        removeButton.GetComponent<Image>().color = Color.white;
	}
}
