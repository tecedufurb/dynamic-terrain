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
    [SerializeField] private Texture2D[] patterns;
    [SerializeField] private Texture2D hipsometricTexture;
    [SerializeField] private RawImage projectionImage;
    [SerializeField] private Text projectionButtonText;
    [SerializeField] private GameObject displayPanel;
    [SerializeField] private Text displayButtonText;
    [SerializeField] private Text regionButtonText;
    [SerializeField] private Text[] coordinatesText;
    [SerializeField] private WebcamController webcam;
    
    private bool terrainMapButton;

    private float projectionTime = 190;
    private float intervalTime = 1000;

    private void Start() {
        LoadPatterns("C:\\Users\\Nemo\\Desktop\\Patterns\\ConventionalGray", 10);
    }

    private void Update() {
        if (Input.GetKeyDown("escape"))
            configurationPanel.SetActive(!configurationPanel.activeSelf);
    }

    public void ActivateHipsometricMap() {
        terrainMapButton = !terrainMapButton;

        if (terrainMapButton) {
//            float initial = Time.realtimeSinceStartup;
            terrainController.UpdateHeatMap();
//            updateHeatMapText.text = "Mapa hipsimétrico: " + (Time.realtimeSinceStartup - initial) * 1000 + " ms";
        }
        else {
            terrainController.ResetColor();
        }
    }

    public void StartChanges() {
//        float initial = Time.realtimeSinceStartup;
        if (farField != null && farField.text != "")
            terrainController.maxHeight = Convert.ToSingle(farField.text);
        
        terrainController.PlotHeight();
//        plotHeightText.text = "Mostrar terreno: " + (Time.realtimeSinceStartup - initial) * 1000 + " ms";
        pointsCountText.text = "Pontos: " + terrainController.pointsCount;

        if (terrainMapButton)
            terrainController.UpdateHeatMap();
    }

    public void StartProjection() {
        if (projectionImage.gameObject.activeSelf) {
            StopCoroutine(Projection());
            projectionImage.gameObject.SetActive(false);
            projectionButtonText.text = "Ativar projeção";
        } else {
            projectionImage.gameObject.SetActive(true);
            configurationPanel.SetActive(false);
            projectionButtonText.text = "Desativar projeção";
            StartCoroutine(Projection());
        }
    }

    private IEnumerator Projection() {
        
        WaitForSeconds projection = new WaitForSeconds(projectionTime/1000);
        WaitForSeconds interval = new WaitForSeconds(intervalTime/1000);

        int count = 0;
        int i = 0;
        while (true) {
            if (i == patterns.Length)
                i = 0;
            
            projectionImage.texture = patterns[i++];
            yield return projection;
            webcam.TakeSnapshot("C:\\Users\\Nemo\\Desktop\\Patterns\\Output\\pattern" + count++ + ".jpg");
//            image = webcam.TakeSnapshot();
            
            projectionImage.texture = hipsometricTexture;
            yield return interval;
//            File.WriteAllBytes("C:\\Users\\Nemo\\Desktop\\Patterns\\Output\\pattern" + count++ + ".jpg", image);  
        }
    }

    public void ShowDisplay() {
        displayButtonText.text = displayPanel.activeSelf ? "Mostrar webcam" : "Esconder webcam";
        displayPanel.SetActive(!displayPanel.activeSelf);
    }

    public void RegionOfInterestButton() {
        if (!RegionOfInterest.region) {
            configurationPanel.SetActive(false);
            displayPanel.SetActive(true);
            RegionOfInterest.region = true;
            regionButtonText.text = "Redefinir";
        } else {
            RegionOfInterest.region = false;
            displayPanel.SetActive(false);
            regionButtonText.text = "Selecionar";
            coordinatesText[0].text = "X1: " + RegionOfInterest.coordinates.upR[0];
            coordinatesText[1].text = "Y1: " + RegionOfInterest.coordinates.upR[1];
            coordinatesText[2].text = "X2: " + RegionOfInterest.coordinates.downL[0];
            coordinatesText[3].text = "Y2: " + RegionOfInterest.coordinates.downL[1];
        }
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

//    private IEnumerator LoadImage (string filePath) {
//        WWW www = new WWW (filePath);
//        while(!www.isDone)
//            yield return null;
//        patters = www.texture;
//    }
}
