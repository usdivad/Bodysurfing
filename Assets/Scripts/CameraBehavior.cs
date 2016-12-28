using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {
	public int framesDisembodiedThreshold; // 60 fps * n seconds
	public float minVel; // 0.01f
	public float maxVel; // 0.2f
	public float velStep; // 0.0001f

	private float curVel;
	private Vector3 mostRecentForward;
	private Vector3 mostRecentGazePosition; // Last position we were at before we started gazing

	private bool isGazing;
	private Vector3 mostRecentTeleportPosition; // Last position we teleported to
	private bool shouldTeleport;

	private bool isDisembodied;
	private int framesDisembodied;

	// Use this for initialization
	void Start () {
		this.curVel = this.minVel;
		this.mostRecentForward = new Vector3 (0, 0, 0);
		this.mostRecentGazePosition = new Vector3 (0, 0, 0);

		this.isGazing = false;
		this.mostRecentTeleportPosition = new Vector3 (0, 0, 0);
		this.shouldTeleport = false;
		this.isDisembodied = false;
		this.framesDisembodied = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// Setup transform and position vars
		Transform transform = this.GetComponent<Transform> ();
		Vector3 pos = transform.position;
		// Debug.Log ("euler angles: " + transform.localEulerAngles);

		// Teleport if necessary
		if (this.shouldTeleport) {
			pos = this.mostRecentTeleportPosition;
			this.shouldTeleport = false;
			this.isDisembodied = !this.isDisembodied;
			this.framesDisembodied = 0;

			if (!this.isDisembodied) {
				GameObject.Find ("GazerAvatar").transform.localPosition = new Vector3 (25, 0, 0);
			}

			// Look in the opposite direction 
			// TODO: fix this!

			//transform.forward = transform.forward * -1;
			//Debug.Log ("teleport: " + transform.forward * -1 + " -> " + transform.forward);

			//transform.Rotate(new Vector3(0, 90, 0));
		}

		if (this.isDisembodied) {
			this.framesDisembodied++;
			// Debug.Log ("frames disembodied: " + this.framesDisembodied);
			if (this.framesDisembodied > this.framesDisembodiedThreshold) {
				this.SetShouldTeleport (true);
				this.mostRecentTeleportPosition = this.mostRecentGazePosition;
			}

			// Accelerate forward in the direction that the camera is facing...
			if (transform.forward == this.mostRecentForward) {
				this.curVel += this.velStep;
			} else {
				// this.curVel -= this.velStep;
				this.curVel = this.minVel;
			}
			this.curVel = Mathf.Min (Mathf.Max (this.curVel, this.minVel), this.maxVel);
		}
		else {
			// ... or steadily back away if I'm gazing
			if (this.isGazing) {
				this.curVel = this.minVel * -1;
			}
			else {
				this.curVel = 0;
			}
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
		if (this.isDisembodied) {
			this.isGazing = false;
			return;
		}

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

	public bool GetIsDisembodied() {
		return this.isDisembodied;
	}
}
