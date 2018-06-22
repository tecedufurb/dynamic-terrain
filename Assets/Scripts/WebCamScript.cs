using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WebCamScript : MonoBehaviour {

	[SerializeField] private RawImage webcamScreen;
	
	[SerializeField] private Text createJPGText;
	[SerializeField] private Text createJPG2Text;
	
	private WebCamTexture webcamTexture;
	private Texture2D snap;
	private const string path = "Assets/Resources/Snapshots/";
	private Color cor;

	private void Start() {
		StartWebcam();
	}

//	private void Update() {
//		CreateJPG();
//	}

	private void StartWebcam() {
		webcamTexture = new WebCamTexture();
		webcamScreen.material.mainTexture = webcamTexture;
		webcamTexture.Play();
		
		snap = new Texture2D(webcamTexture.width, webcamTexture.height);
	}

	private byte[] TakeSnapshot() {
		snap.SetPixels(webcamTexture.GetPixels());
		snap.Apply();
		return snap.EncodeToJPG();
	}
	
	private void SaveSnapshot(byte[] image, string path, string fileName) {
		File.WriteAllBytes(path + fileName, image);
	}
	
	public void CreateJPG() {
		float initial = Time.realtimeSinceStartup;
		byte[] myImage = TakeSnapshot();
		
		// salva usando funcao da unity
//		SaveSnapshot(myImage, path, "image_unity.jpg");
		
		createJPGText.text = "GerarJPG: " + (Time.realtimeSinceStartup - initial) * 1000 + " ms";
		
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
		SaveSnapshot(myImage, "", "image_unity.jpg");
		
		createJPG2Text.text = "GerarJPG 2: " + (Time.realtimeSinceStartup - initial) * 1000 + " ms";
		
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