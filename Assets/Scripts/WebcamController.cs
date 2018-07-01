using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WebcamController : MonoBehaviour {

	[SerializeField] private RawImage display;
//	[SerializeField] private Text startStopText;
//	[SerializeField] private Text createJPGText;
//	[SerializeField] private Text createJPG2Text;
	
	private int currentCamIndex;
	private WebCamTexture texture;
	private Texture2D snap;
	private const string path = "Assets/Resources/Snapshots/";

	private void Start() {
		currentCamIndex = WebCamTexture.devices.Length-1;
		StartStopCam_Clicked();
	}

	public void SwitchWebcam () {
		if (WebCamTexture.devices.Length > 0) {
			currentCamIndex += 1;
			currentCamIndex %= WebCamTexture.devices.Length;

			if (texture != null) {
				StopWebcam();
				StartStopCam_Clicked();
			}
		}
	}

	private void StartStopCam_Clicked () {
	
		if (texture != null) {
			StopWebcam();
		} else {
			WebCamDevice device = WebCamTexture.devices[currentCamIndex];
			texture = new WebCamTexture(device.name);
			display.texture = texture;
			
			texture.Play();	
			snap = new Texture2D(texture.width, texture.height);
		}
	}
	
	private void StopWebcam () {
		display.texture = null;
		texture.Stop();
		texture = null;
	}
	
	public byte[] TakeSnapshot() {
		snap.SetPixels(texture.GetPixels());
		snap.Apply();
		return snap.EncodeToJPG();
	}
	
	public void TakeSnapshot(string fileName) {
		snap.SetPixels(texture.GetPixels());
		snap.Apply();
		File.WriteAllBytes(/*path + */fileName, snap.EncodeToJPG());
	}
	
	private void SaveSnapshot(byte[] image, string path, string fileName) {
		File.WriteAllBytes(path + fileName, image);
	}
	
	public void CreateJPG() {
		float initial = Time.realtimeSinceStartup;
		byte[] myImage = TakeSnapshot();
		
		// salva usando funcao da unity
//		SaveSnapshot(myImage, path, "image_unity.jpg");
		
//		createJPGText.text = "GerarJPG: " + (Time.realtimeSinceStartup - initial) * 1000 + " ms";
		
		// a chamada do metodo da dll precisa ser feita dentro desse bloco unsafe
//		unsafe {
//			fixed (byte* data = myImage) {
//				// se o ultimo parametro for true salva imagem no diretorio 'path'.
//				// se for false so atribui o array da imagem pra variavel image da dll
//				DllManager.SetImage(data, myImage.Length, true);
//			}
//		}
	}
	
	public void CreateJPG2() {
		float initial = Time.realtimeSinceStartup;
		byte[] myImage = TakeSnapshot();
		
		// salva usando funcao da unity
		SaveSnapshot(myImage, path, "image_unity.jpg");
		
//		createJPG2Text.text = "GerarJPG 2: " + (Time.realtimeSinceStartup - initial) * 1000 + " ms";
		
		// a chamada do metodo da dll precisa ser feita dentro desse bloco unsafe
//		unsafe {
//			fixed (byte* data = myImage) {
//				// se o ultimo parametro for true salva imagem no diretorio 'path'.
//				// se for false so atribui o array da imagem pra variavel image da dll
//				DllManager.SetImage(data, myImage.Length, true);
//			}
//		}
	}
}
