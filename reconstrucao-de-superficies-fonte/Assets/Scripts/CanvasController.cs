using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    [SerializeField] private TerrainController terrainController;
    [SerializeField] private GameObject configurationPanel;
    [SerializeField] private Text pointsCountText;
    [SerializeField] private InputField farField;
    [SerializeField] private InputField nearField;
    [SerializeField] private InputField patternProjectionTimeField;
    [SerializeField] private InputField imageProjectionTimeField;
    [SerializeField] private Texture2D[] patterns;
    [SerializeField] private Texture2D hipsometricTexture;
    [SerializeField] private RawImage projectionImage;
    [SerializeField] private Text projectionButtonText;
    [SerializeField] private GameObject displayPanel;
    [SerializeField] private Text displayButtonText;
    [SerializeField] private Text[] coordinatesText;
    [SerializeField] private WebcamController webcam;
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private Text warningPanelText;
    [SerializeField] private GameObject patternFolderPanel;
    [SerializeField] private InputField patternsFolderField;
    [SerializeField] private InputField patternsAmountField;
    [SerializeField] private InputField printsFolderField;
    [SerializeField] private GameObject redDotPrefab;

    private bool terrainMapButton;

    private float projectionTime = 190;
    private float intervalTime = 1000;
    
    public Coordinates coordinates;
    private bool region;
    private byte clicksCount;
    private string patternsFolder;
    private string printsFolder;
    private int patternsAmount;

//    private void Start() {
//        LoadPatterns(patternsFolder, patternsAmount);
//    }

    private void Update() {
        if (Input.GetKeyDown("escape"))
            configurationPanel.SetActive(!configurationPanel.activeSelf);
        
        if (region && Input.GetMouseButtonDown(0))
            SetCoordinates(Input.mousePosition);
    }

    /**
     * Chamado no botao Mapa Hipsometrico do painel de configuracao
     */
    public void ActivateHipsometricMap() {
        terrainMapButton = !terrainMapButton;

        if (terrainMapButton) {
            //float initial = Time.realtimeSinceStartup;
            terrainController.UpdateHeatMap();
            //updateHeatMapText.text = "Mapa hipsimétrico: " + (Time.realtimeSinceStartup - initial) * 1000 + " ms";
        }
        else {
            terrainController.ResetColor();
        }
        configurationPanel.SetActive(false);
    }

    /**
     * Chamado no botao Mostrar terreno do painel de configuracao
     */
    public void StartChanges() {
        //float initial = Time.realtimeSinceStartup;
        if (farField.text != "")
            terrainController.far = Convert.ToSingle(farField.text);

        if (nearField.text != "")
            terrainController.near = Convert.ToSingle(nearField.text);
        
        terrainController.PlotHeight();
        //plotHeightText.text = "Mostrar terreno: " + (Time.realtimeSinceStartup - initial) * 1000 + " ms";
        pointsCountText.text = "Pontos: " + terrainController.pointsCount;

        if (terrainMapButton)
            terrainController.UpdateHeatMap();
        
        configurationPanel.SetActive(false);
    }

    /**
     * Chamado no botao Iniciar projecao do painel de configuracao
     */
    private Coroutine projection;
    public void StartProjection() {
        if (projectionImage.gameObject.activeSelf) {
            StopCoroutine(projection);
            projectionImage.gameObject.SetActive(false);
            projectionButtonText.text = "Ativar projeção";
        } else {

            if (patternsFolder == null || printsFolder == null) {
                Warning("Defina as pastas de destino e origem dos padrões.");
            } else {
                if (patternProjectionTimeField.text != "")
                    projectionTime = Convert.ToSingle(patternProjectionTimeField.text);
                if (imageProjectionTimeField.text != "")
                    intervalTime = Convert.ToSingle(imageProjectionTimeField.text);
            
                projectionImage.gameObject.SetActive(true);
                configurationPanel.SetActive(false);
                projectionButtonText.text = "Desativar projeção";
                projection = StartCoroutine(Projection());
            }
        }
    }
    
    public void ApplyPatternFoldersButton() {
        if (patternsFolderField.text != "" && patternsAmountField.text != "" && printsFolderField.text != "") {
            patternsFolder = patternsFolderField.text;
            patternsAmount = Convert.ToInt32(patternsAmountField.text);
            printsFolder = printsFolderField.text;
            LoadPatterns(patternsFolder, patternsAmount);
            BackPatternFoldersButton();
        }
    }

    public void BackPatternFoldersButton() {
        patternFolderPanel.SetActive(false);
        configurationPanel.SetActive(true);
    }
    
    public void ShowPatternFoldersPanel() {
        configurationPanel.SetActive(false);
        patternFolderPanel.SetActive(true);
    }

    /**
     * Chamado no botao Mostrar webcam do painel de configuracao
     */
    public void ShowDisplayButton() {
        displayButtonText.text = displayPanel.activeSelf ? "Mostrar webcam" : "Esconder webcam";
        displayPanel.SetActive(!displayPanel.activeSelf);
        configurationPanel.SetActive(false);
    }

    /**
     * Chamado no botao Redefinir do painel de configuracao
     */
    public void SetRegionButton() {
        configurationPanel.SetActive(false);
        displayPanel.SetActive(true);
        region = true;
    }

    /**
     * Chamado no botao Trocar webcam do painel de configuracao
     */
    public void SwitchWebcamButton() {
        webcam.SwitchWebcam();
    }

    public void Warning(string text) {
        warningPanelText.text = text;
        configurationPanel.SetActive(false);
        warningPanel.SetActive(true);
    }
    
    public void WarningButton() {
        warningPanelText.text = "";
        warningPanel.SetActive(false);
        configurationPanel.SetActive(true);
    }

    private IEnumerator Projection() {
        WaitForSeconds projection = new WaitForSeconds(projectionTime / 1000);
        WaitForSeconds interval = new WaitForSeconds(intervalTime / 1000);

        int count = 0;
        int i = 0;
        while (true) {
            if (i == patterns.Length)
                i = 0;

            projectionImage.texture = patterns[i++];
            yield return projection;
            webcam.TakeSnapshot(printsFolder + "\\print" + count++ + ".jpg");
            projectionImage.texture = hipsometricTexture;
            yield return interval;
        }
    }

    private GameObject point;
    private void SetCoordinates(Vector3 mousePosition) {
        clicksCount++;
        switch (clicksCount) {
            case 1:
                coordinates.x1 = (int)mousePosition.x;
                coordinates.y1 = (int)mousePosition.y;
                point = Instantiate(redDotPrefab, mousePosition, Quaternion.identity, gameObject.transform);
                break;
            case 2:
                coordinates.x2 = (int)mousePosition.x;
                coordinates.y2 = (int)mousePosition.y;
                clicksCount = 0;
                SetTextCoordinates();
                Destroy(point);
                break;
        }
    }

    private void SetTextCoordinates() {
        region = false;
        coordinatesText[0].text = "X1: " + coordinates.x1;
        coordinatesText[1].text = "Y1: " + coordinates.y1;
        coordinatesText[2].text = "X2: " + coordinates.x2;
        coordinatesText[3].text = "Y2: " + coordinates.y2;
        displayPanel.SetActive(false);
        configurationPanel.SetActive(true);
    }

    private static Texture2D LoadJPG(string filePath) {
        if (!File.Exists(filePath))
            return null;
        
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2, TextureFormat.BGRA32, false);
        texture.LoadImage(fileData);
        
        return texture;
    }

    private void LoadPatterns(string filePath, int amount) {
        patterns = new Texture2D[amount];
        for (int i = 0; i < amount; i++)
            patterns[i] = LoadJPG(filePath + "\\" + (i+1) + ".png");
    }
}
