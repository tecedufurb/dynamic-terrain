using UnityEngine;

public class DragDropScript : MonoBehaviour {

    //Initialize variables
    private GameObject getTarget;
    private bool isMouseDragging;
    private Vector3 offsetValue;
    private Vector3 positionOfScreen;
	private GameController gameController;

	void Start () {
		gameController = GameObject.FindObjectOfType<GameController>();
	}

    void Update() {

		if (gameController.moveBuildings) {
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit hitInfo;
				getTarget = ReturnClickedObject(out hitInfo);
				if (getTarget != null) {
					isMouseDragging = true;
					//Converting world position to screen position.
					positionOfScreen = Camera.main.WorldToScreenPoint(getTarget.transform.position);
					offsetValue = getTarget.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
				}
			} else if (Input.GetMouseButtonUp(0)) {
				isMouseDragging = false;
			}

			//Is mouse moving
			if (isMouseDragging) {
				//tracking mouse position.
				Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z);

				//converting screen position to world position with offset changes.
				Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offsetValue;

				//It will update target gameobject's current postion.
				getTarget.transform.position = currentPosition;
			}
		}
    }

    //Method to return clicked object
    private GameObject ReturnClickedObject(out RaycastHit hit) {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
            target = hit.collider.gameObject;
        return target;
    }

}