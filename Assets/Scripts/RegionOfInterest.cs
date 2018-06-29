using UnityEngine;

public struct Coordinates {
	public float[] upL;
	public float[] upR;
	public float[] downL;
	public float[] downR;
}

public class RegionOfInterest : MonoBehaviour {
	
	[SerializeField] private Material lineMaterial;
	[SerializeField] private float lineWidth;
	[SerializeField] private float depth = 5;

	public static Coordinates coordinates;
	
	private new Camera camera;
	private Vector3 lineStartPoint;
	private Vector3 pixelStartPoint;
	private LineRenderer line;
	private GameObject lineObject;
	public static bool region;
	
	private void Start () {
		camera = FindObjectOfType<Camera>();
	}
	
	private void Update () {
		if (!region)
			return;
		
		if (Input.GetMouseButtonDown(0)) {
			SetInitialCoordinates();
		} else if (Input.GetMouseButtonUp(0)) {
			if (lineStartPoint == Vector3.zero)
				return;
			SetFinalCoordinates();
		}
	}

	private void SetInitialCoordinates() {
		lineStartPoint = GetMouseCameraPoint();
		pixelStartPoint = Input.mousePosition;
		print(lineStartPoint);
		if (lineObject!=null)
			Destroy(lineObject);
	}

	private void SetFinalCoordinates() {
		Vector3 lineEndPoint = GetMouseCameraPoint();
		Vector3 pixelEndPoint = Input.mousePosition;
		lineEndPoint.y = lineStartPoint.y;

		lineObject = new GameObject("RegionOfInterest"); 
		line = lineObject.AddComponent<LineRenderer>();
		line.material = lineMaterial;
		line.startWidth = lineWidth;
		line.endWidth = lineWidth;

		line.positionCount = 5;
		line.SetPosition(0, lineStartPoint);
		line.SetPosition(1, new Vector3(lineStartPoint.x, lineStartPoint.y, lineEndPoint.z));
		line.SetPosition(2, lineEndPoint);
		line.SetPosition(3, new Vector3(lineEndPoint.x, lineStartPoint.y, lineStartPoint.z));
		line.SetPosition(4, lineStartPoint);
		
		coordinates.upR = new float[] {pixelStartPoint.x, pixelStartPoint.y};
		coordinates.upL = new float[] {pixelStartPoint.x, pixelEndPoint.y};
		coordinates.downR = new float[] {pixelEndPoint.x, pixelStartPoint.y};
		coordinates.downL = new float[] {pixelEndPoint.x, pixelEndPoint.y};
		
		print(coordinates.upR[0]);
		print(coordinates.upR[1]);
		print(coordinates.downL[0]);
		print(coordinates.downL[1]);
		
		lineStartPoint = Vector3.zero;
	}

	private Vector3 GetMouseCameraPoint() {
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		return ray.origin + ray.direction * depth;
	}
}
