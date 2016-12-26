using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

	private float curVel;
	private float minVel;
	private float maxVel;
	private float velStep;
	private Vector3 mostRecentForward;
	private Vector3 mostRecentGazePosition; // Last position we were at before we started gazing

	private bool isGazing;
	private Vector3 mostRecentTeleportPosition; // Last position we teleported to
	private bool shouldTeleport;

	// Use this for initialization
	void Start () {
		this.minVel = 0.01f;
		this.maxVel = 0.2f;
		this.curVel = this.minVel;
		this.velStep = 0.001f;
		this.mostRecentForward = new Vector3 (0, 0, 0);
		this.mostRecentGazePosition = new Vector3 (0, 0, 0);

		this.isGazing = false;
		this.mostRecentTeleportPosition = new Vector3 (0, 0, 0);
		this.shouldTeleport = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Setup transform and position vars
		Transform transform = this.GetComponent<Transform> ();
		Vector3 pos = transform.position;
		Debug.Log ("euler angles: " + transform.localEulerAngles);

		// Teleport if necessary
		if (this.shouldTeleport) {
			pos = this.mostRecentTeleportPosition;
			this.shouldTeleport = false;

			// Look in the opposite direction 
			// TODO: fix this!

			//transform.forward = transform.forward * -1;
			//Debug.Log ("teleport: " + transform.forward * -1 + " -> " + transform.forward);

			//transform.Rotate(new Vector3(0, 90, 0));
		}

		// Accelerate forward in the direction that the camera is facing...
		if (transform.forward == this.mostRecentForward) {
			this.curVel += this.velStep;
		}
		else {
			// this.curVel -= this.velStep;
			this.curVel = this.minVel;
		}
		this.curVel = Mathf.Min (Mathf.Max (this.curVel, this.minVel), this.maxVel);

		// ... or steadily back away if I'm gazing
		if (this.isGazing) {
			this.curVel = this.minVel * -1;
		}

		// Create movement vector
		Vector3 mvmt = transform.forward * this.curVel;
		this.mostRecentForward = transform.forward;
		//Quaternion rot = transform.rotation;
		//Debug.Log (rot);
		pos.x += mvmt.x;
		pos.z += mvmt.z;

		// Finally set transform position
		transform.position = pos;
	}

	public void SetIsGazing(bool gazing) {
		if (gazing && !this.isGazing) {
			this.mostRecentGazePosition = this.transform.localPosition;
		}
		this.isGazing = gazing;
	}

	public void SetMostRecentForward(Vector3 forward) {
		this.mostRecentForward = forward;
	}

	public void SetMostRecentTeleportPosition(Vector3 position) {
		this.mostRecentTeleportPosition = position;
	}

	public void SetShouldTeleport(bool teleport) {
		this.shouldTeleport = teleport;
	}

	public Vector3 GetMostRecentGazePosition() {
		return this.mostRecentGazePosition;
	}
}
