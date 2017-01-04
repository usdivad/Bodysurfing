using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {
	public int framesDisembodiedThreshold; // 60 fps * n seconds
	public float minVel; // 0.01f
	public float maxVel; // 0.2f
	public float velStep; // 0.0001f
	public float turnMagnitudeThreshold;

	private float curVel;
	private float curVelMultiplier;
	private Vector3 mostRecentForward;
	private Vector3 mostRecentGazePosition; // Last position we were at before we started gazing
	private int mostRecentGazeIndex;

	private bool isGazing;
	private Vector3 mostRecentTeleportPosition; // Last position we teleported to
	private bool shouldTeleport;
	private int framesSinceLastTeleportation;

	private bool isDisembodied;
	private int framesDisembodied;

	// Use this for initialization
	void Start () {
		this.curVel = this.minVel;
		this.curVelMultiplier = 1.0f;
		this.mostRecentForward = new Vector3 (0, 0, 0);
		this.mostRecentGazePosition = new Vector3 (0, 0, 0);
		this.mostRecentGazeIndex = -1;

		this.isGazing = false;
		this.mostRecentTeleportPosition = new Vector3 (0, 0, 0);
		this.shouldTeleport = false;
		this.framesSinceLastTeleportation = 0;

		this.isDisembodied = false;
		this.framesDisembodied = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// Setup transform and position vars
		Transform transform = this.GetComponent<Transform> ();
		Vector3 pos = transform.position;
		// Debug.Log ("euler angles: " + transform.localEulerAngles);
		this.framesSinceLastTeleportation++;

		// Teleport if necessary
		if (this.shouldTeleport) {
			pos = this.mostRecentTeleportPosition;
			this.shouldTeleport = false;
			this.isDisembodied = !this.isDisembodied;
			this.framesDisembodied = 0;

			if (!this.isDisembodied) {
				GameObject.Find ("Gazer Avatar").transform.localPosition = new Vector3 (25, 0, 0);
				GameObject.Find ("Entity Manager").GetComponent<GameBehavior>().teleportEntity (this.mostRecentGazeIndex, this.transform.position);
			}

			// Look in the opposite direction 
			// TODO: fix this! has to override some of the VR settings

			//transform.forward = transform.forward * -1;
			//Debug.Log ("teleport: " + transform.forward * -1 + " -> " + transform.forward);

			//transform.Rotate(new Vector3(0, 90, 0));

			this.framesSinceLastTeleportation = 0;
		}

		if (this.isDisembodied) {
			this.framesDisembodied++;
			// Debug.Log ("frames disembodied: " + this.framesDisembodied);
			if (this.framesDisembodied > this.framesDisembodiedThreshold) {
				this.SetShouldTeleport (true);
				this.mostRecentTeleportPosition = this.mostRecentGazePosition;
			}

			// Accelerate forward in the direction that the camera is facing...
			float turnMagnitude = (transform.forward - this.mostRecentForward).magnitude;
			if (turnMagnitude < this.turnMagnitudeThreshold) {
				this.curVel += this.velStep;
			} else {
				//this.curVel -= this.velStep * (turnMagnitude / this.turnMagnitudeThreshold);
				this.curVel = this.minVel;
				Debug.Log ("turn magnitude: " + turnMagnitude);
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
		Vector3 mvmt = transform.forward * this.curVel * this.curVelMultiplier;
		this.mostRecentForward = transform.forward;
		//Quaternion rot = transform.rotation;
		//Debug.Log (rot);
		pos.x += mvmt.x;
		pos.z += mvmt.z;

		// Finally set transform position
		transform.position = pos;
	}

	public void SetIsGazingAt(int gazingAt) {
		bool gazing = gazingAt >= 0;

		if (this.isDisembodied) {
			this.isGazing = false;
			return;
		}

		if (gazing && !this.isGazing) {
			this.mostRecentGazePosition = this.transform.localPosition;
			this.mostRecentGazeIndex = gazingAt;
		}
		this.isGazing = gazing;
	}

	public float GetCurVelMultiplier() {
		return this.curVelMultiplier;
	}

	public void SetCurVelMultiplier(float mul) {
		this.curVelMultiplier = mul;
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

	public int GetFramesSinceLastTeleportation() {
		return this.framesSinceLastTeleportation;
	}
}
