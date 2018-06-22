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

	private static Coordinates coordinates;
	
	private new Camera camera;
	private Vector3 lineStartPoint;
	private LineRenderer line;
	private GameObject lineObject;
	
	private void Start () {
		camera = FindObjectOfType<Camera>();
	}
	
	private void Update () {
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
		if (lineObject!=null)
			Destroy(lineObject);
	}

	private void SetFinalCoordinates() {
		Vector3 lineEndPoint = GetMouseCameraPoint();
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
		
		coordinates.upR = new float[] {lineStartPoint.x, lineStartPoint.z};
		coordinates.upL = new float[] {lineStartPoint.x, lineEndPoint.z};
		coordinates.downR = new float[] {lineEndPoint.x, lineStartPoint.z};
		coordinates.downL = new float[] {lineEndPoint.x, lineEndPoint.z};
		
		lineStartPoint = Vector3.zero;
	}

	private Vector3 GetMouseCameraPoint() {
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		return ray.origin + ray.direction * depth;
	}
}
