using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainView : MonoBehaviour {

	public TerrainController terrainController;
    public GameController gameController;
    public GameObject containerPanel;
    private bool _terrainMapButton;
    private bool _sineFunctionButton;
    private bool _sineFunction2Button;
    private Coroutine _terrainMapCoroutine;

	 public void ActivateTerrainMap() {
        _terrainMapButton = !_terrainMapButton;

        if(_terrainMapButton) {
            _terrainMapCoroutine = StartCoroutine(terrainController.UpdateHeatMap());
        } else {
            StopCoroutine(_terrainMapCoroutine);
            terrainController.ResetColor();
        }
    }

    public void ActivateSineFunction() {
        // terrainController.ResetCollider();
        gameController.ResetObjectsCollider();
        _sineFunctionButton = !_sineFunctionButton;
        _sineFunction2Button = false;
        if (_sineFunctionButton) {
            terrainController.utils.function = (int)FunctionOption.Sine;
            terrainController.StartChanges();
        } else {
            terrainController.StopChanges();
        }            
    }

    public void ActivateComplexSineFunction() {
        // terrainController.ResetCollider();
        gameController.ResetObjectsCollider();
        _sineFunction2Button = !_sineFunction2Button;
        _sineFunctionButton = false;
        if (_sineFunction2Button) {
            terrainController.utils.function = (int)FunctionOption.ComplexSine;
            terrainController.StartChanges();
        } else {
            terrainController.StopChanges();
        }
    }

    public void HideButton() {
        if(containerPanel.activeSelf)
            containerPanel.SetActive(false);
        else
            containerPanel.SetActive(true);
    }
}
