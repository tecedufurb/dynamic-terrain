using UnityEngine;

public class ObjectController : MonoBehaviour {

	private bool isGround = false;

    public bool IsGround {
        get {
            return isGround;
        }
        set {
            isGround = value;
        }
    }

	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Surface"))
			IsGround = true;
	}

	/// <summary>
	/// OnCollisionExit is called when this collider/rigidbody has
	/// stopped touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionExit(Collision other) {
		if (other.gameObject.CompareTag("Surface"))
			IsGround = false;
	}
}
