using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject[] buildings;
	private bool addBuildings;
	private bool removeBuildings;
	private bool moveBuildings;

	private List<ObjectController> buildingList;
	private TerrainController terrainController;

	void Start() {
		buildingList = new List<ObjectController>();
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
		foreach (ObjectController building in buildingList) {
			if (!building.IsGround) {
				building.gameObject.GetComponent<Rigidbody>().useGravity = false;
				terrainController.ResetCollider();
				building.gameObject.GetComponent<Rigidbody>().useGravity = true;
			}
		}
	} 

	private void InstantiateBuilding () {
		Ray ray;
		RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 100.0f)) {
			if (hit.collider.CompareTag("Surface")) {
				int rand = Random.Range(0, buildings.Length);
				Vector3 position = new Vector3(hit.point.x, 
					hit.point.y + buildings[rand].transform.localScale.y, hit.point.z);
				GameObject newBuilding = Instantiate(buildings[rand], position, Quaternion.identity);
				buildingList.Add(newBuilding.GetComponent<ObjectController>());
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
				buildingList.Remove(hit.collider.gameObject.GetComponent<ObjectController>());
			}
		}
	}

	private void MoveBuilding () {
		// TODO - drag on click
	}

	public void ActivateGravity (bool value) {
		foreach (ObjectController o in buildingList) {
			o.gameObject.GetComponent<Rigidbody>().isKinematic = value;
		}
	}

	public void Add () {
		addBuildings = true;
		removeBuildings = false;
		moveBuildings = false;
	}

	public void Remove () {
		removeBuildings = true;
		addBuildings = false;
		moveBuildings = false;
	}

	public void Move () {
		moveBuildings = true;
		addBuildings = false;		
		removeBuildings = false;
	}
}
