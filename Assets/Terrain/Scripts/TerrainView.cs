using UnityEngine;
using UnityEngine.UI;

public class TerrainView : MonoBehaviour {

	[SerializeField] private TerrainController terrainController;
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject containerPanel;
	[SerializeField] private Text playButtonText;
    [SerializeField] private Button gravityButton;
    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button moveButton;

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
		//kinematic = !kinematic;
		gameController.gravity = !gameController.gravity;
        gameController.ActivateGravity(gameController.gravity);
		if (gameController.gravity) {
            gravityButton.transform.GetChild(0).GetComponent<Text>().text = "Ativar gravidade";
            gravityButton.GetComponent<Image>().color = Color.white;
		} else {
            gravityButton.transform.GetChild(0).GetComponent<Text>().text = "Desativar gravidade";
            gravityButton.GetComponent<Image>().color = Color.gray;
        }
    }

	public void AddBuildingsButton () {
        if (!gameController.addBuildings) {
            gameController.addBuildings = true;
            gameController.removeBuildings = false;
            gameController.moveBuildings = false;
            
            addButton.GetComponent<Image>().color = Color.gray;
            removeButton.GetComponent<Image>().color = Color.white;
            moveButton.GetComponent<Image>().color = Color.white;
        } else {
            gameController.addBuildings = false;
            addButton.GetComponent<Image>().color = Color.white;
        }
        
	}

    public void RemoveBuildingsButton () {
		if (!gameController.removeBuildings) {
            gameController.removeBuildings = true;
            gameController.addBuildings = false;
            gameController.moveBuildings = false;

            removeButton.GetComponent<Image>().color = Color.gray;
            addButton.GetComponent<Image>().color = Color.white;
            moveButton.GetComponent<Image>().color = Color.white;
        } else {
            gameController.removeBuildings = false;
            removeButton.GetComponent<Image>().color = Color.white;
        }
	}

    public void MoveBuildingsButton () {
        if (!gameController.moveBuildings) {
            gameController.moveBuildings = true;
            gameController.addBuildings = false;		
            gameController.removeBuildings = false;

            moveButton.GetComponent<Image>().color = Color.gray;
            addButton.GetComponent<Image>().color = Color.white;
            removeButton.GetComponent<Image>().color = Color.white;
        } else {
            gameController.moveBuildings = false;
            moveButton.GetComponent<Image>().color = Color.white;
        }
	}
}
