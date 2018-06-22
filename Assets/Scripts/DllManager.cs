using System.Runtime.InteropServices;

// esse é um header necessário pra acessar as funcoes da dll.
// as funcoes da dll nao podem ser chamadas diretamente entao chama atraves dessa classe
internal static class DllManager {
	[DllImport("OpenCV24")]
	internal static extern unsafe bool SetImage(byte* data, int size, bool save);

	[DllImport("OpenCV24")]
	internal static extern float[,] GetHeight ();
}