using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject[] buildings;
	public bool addBuildings;
	public bool removeBuildings;
	public bool moveBuildings;
	public bool gravity = true;

	private List<GameObject> buildingList;
	private TerrainController terrainController;

	void Start() {
		buildingList = new List<GameObject>();
		terrainController = GameObject.FindObjectOfType<TerrainController>();
	}

	void FixedUpdate () {
		
		if(Input.GetButtonDown("Fire1")) {
			if (addBuildings)
				InstantiateBuilding();
			else if (removeBuildings)
				DestroyBuilding();
			else if (moveBuildings)
				MoveBuilding();
		}
			
	}

	public void ResetObjectsCollider () {
		ActivateGravity(!gravity);
		ActivateGravity(gravity);
	}

	private void InstantiateBuilding () {
		Ray ray;
		RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 100.0f)) {
			if (hit.collider.CompareTag("Surface")) {
				int rand = Random.Range(0, buildings.Length);
				Vector3 position = new Vector3(hit.point.x, 
					hit.point.y + 1.3f/*buildings[rand].transform.localScale.y/2*/, hit.point.z);
				GameObject newBuilding = Instantiate(buildings[rand], position, Quaternion.identity);
				newBuilding.GetComponent<Rigidbody>().isKinematic = gravity;
				buildingList.Add(newBuilding);
			}
		}
	}

	private void DestroyBuilding () {
		Ray ray;
		RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 100.0f)) {
			if (hit.collider.CompareTag("Building")) {
				Destroy(hit.collider.gameObject);
				buildingList.Remove(hit.collider.gameObject);
			}
		}
	}

	private void MoveBuilding () {
		
	}

	public void ActivateGravity (bool value) {
		foreach (GameObject o in buildingList)
			o.GetComponent<Rigidbody>().isKinematic = value;
	}
}
