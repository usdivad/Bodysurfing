/*
 * Camera behavior; this is the lens through which the player sees. Also handles some avatar placement
 *
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraBehavior : MonoBehaviour {
	public int framesDisembodiedThreshold; // 60 fps * n seconds
	public float minVel; // 0.01f
	public float maxVel; // 0.2f
	public float velStep; // 0.0001f
	public float turnMagnitudeThreshold;
	public float converserDistanceThreshold;

	private float curVel;
	private float curVelMultiplier;
	private Vector3 initialPosition;
	private Vector3 mostRecentForward;
	private Vector3 mostRecentGazePosition; // Last position we were at before we started gazing
	private int mostRecentGazeIndex; // Last entity we gazed at while not disembodied
	private int mostRecentConverserIndex; // Last entity about to engage in conversation with us

	private bool isGazing;
	private bool isConversing;
	private Vector3 mostRecentTeleportPosition; // Last position we teleported to
	private bool shouldTeleport;
	private int framesSinceLastTeleportation;

	private bool isDisembodied;
	private int framesDisembodied;

	// Use this for initialization
	void Start () {
		this.curVel = this.minVel;
		this.curVelMultiplier = 1.0f;
		this.initialPosition = this.GetComponent<Transform> ().position;
		this.mostRecentForward = new Vector3 (0, 0, 0);
		this.mostRecentGazePosition = new Vector3 (0, 0, 0);
		this.mostRecentGazeIndex = -1;
		this.mostRecentConverserIndex = -1;

		this.isGazing = false;
		this.isConversing = false;
		this.mostRecentTeleportPosition = new Vector3 (0, 0, 0);
		this.shouldTeleport = false;
		this.framesSinceLastTeleportation = 0;

		this.isDisembodied = false;
		this.framesDisembodied = 0;

		GameObject.Find ("Gazer Avatar").transform.localPosition = new Vector3 (0, -100, 0);
	}
	
	// Update is called once per frame
	void Update () {
		// Setup transform and position vars
		Transform transform = this.GetComponent<Transform> ();
		Vector3 pos = transform.position;
		// Debug.Log ("euler angles: " + transform.localEulerAngles);
		this.framesSinceLastTeleportation++;
		string dialogueLine = "";
		this.isConversing = false;

		// Teleport if necessary
		if (this.shouldTeleport) {
			pos = this.mostRecentTeleportPosition;
			this.shouldTeleport = false;
			this.isDisembodied = !this.isDisembodied;
			this.framesDisembodied = 0;

			// Adjust avatar positions
			if (this.isDisembodied) {
				GameObject.Find ("Gazer Avatar").transform.localPosition = this.initialPosition;
				//GameObject.Find ("Gazer Avatar").GetComponent<GazerAvatarGazeBehavior> ().MoveSelfAndChildrenTo (this.initialPosition);
				//GameObject.Find ("TreasuerChest_(LOD)").transform.position = this.initialPosition;


				GameObject.Find ("Entity Manager").GetComponent<GameBehavior>().SetEntityPosition (this.mostRecentGazeIndex, new Vector3(25, 0, 0));
			}
			else {
				GameObject.Find ("Gazer Avatar").transform.localPosition = new Vector3 (0, -100, 0);
				//GameObject.Find ("Gazer Avatar").GetComponent<GazerAvatarGazeBehavior> ().MoveSelfAndChildrenTo (new Vector3(25, 0, 0));
				//GameObject.Find ("TreasuerChest_(LOD)").transform.position = new Vector3(25, 0, 0);

				GameObject.Find ("Entity Manager").GetComponent<GameBehavior>().SetEntityPosition (this.mostRecentGazeIndex, pos);
			}

			// Look in the opposite direction 
			// TODO: fix this! has to override some of the VR settings

			//transform.forward = transform.forward * -1;
			//Debug.Log ("teleport: " + transform.forward * -1 + " -> " + transform.forward);

			//transform.Rotate(new Vector3(0, 90, 0));

			this.framesSinceLastTeleportation = 0;
		}

		// Make adjustments based on state of disembodiment
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
				//Debug.Log ("turn magnitude: " + turnMagnitude);
			}
			this.curVel = Mathf.Min (Mathf.Max (this.curVel, this.minVel), this.maxVel);

			// (...but if I'm about to converse, then slow down / stop...)
			if (this.mostRecentConverserIndex >= 0) {
				Vector3 converserPos = GameObject.Find ("Entity Manager").GetComponent<GameBehavior>().GetEntityPosition (this.mostRecentConverserIndex);
				float converserDistance = (this.transform.position - converserPos).magnitude;
				//Debug.Log ("converserDistance: " + converserDistance + " (" + converserPos + ")");

				// Decelerate
				//this.curVel *= Mathf.Min(Mathf.Max ((converserDistance - this.converserDistanceThreshold) * 1.0f, 0f), 1f);
				//this.curVel = Mathf.Max(this.curVel - this.minVel, 0f);

				// Grind to a halt and display dialogue
				if (converserDistance < this.converserDistanceThreshold) {
					//this.curVel = 0;
					this.curVel = Mathf.Max(this.curVel - this.minVel, 0f);

					// Converse! (Update dialogue line)
					//GameObject.Find("Dialogue Manager").GetComponent<DialogueBehavior>().ConverseCharacters(this.mostRecentGazeIndex + 1, this.mostRecentConverserIndex + 1);
					dialogueLine = GameObject.Find("Dialogue Manager").GetComponent<DialogueBehavior>().GetDialogueForCharacter(this.mostRecentGazeIndex + 1, this.mostRecentConverserIndex + 1);
					this.isConversing = true;
					//GameObject.Find ("Entity Manager").GetComponent<GameBehavior> ().PlayEntityTransitionalFragment (this.mostRecentConverserIndex);
				}
			}
		}
		else {
			// ... or steadily back away if I'm gazing
			if (this.isGazing) {
				this.curVel = this.minVel * -1;
			}
			else {
				this.curVel = 0;
				pos = this.initialPosition;
			}

			// Also, update character index
			//this.mostRecentGazeIndex = -1;
		}

		// Create movement vector
		Vector3 mvmt = transform.forward * this.curVel * this.curVelMultiplier;
		this.mostRecentForward = transform.forward;
		//Quaternion rot = transform.rotation;
		//Debug.Log (rot);
		pos.x += mvmt.x;
		pos.z += mvmt.z;

		// Finally set transform position (if position is within world boundaries)
		if (GameObject.Find ("Entity Manager").GetComponent<GameBehavior> ().PositionIsInWorldBoundaries (pos)) {
			transform.position = pos;
		}

		// Set dialogue text (either converser dialogue or empty string)
		GameObject.Find ("Dialogue Text").GetComponent<Text> ().text = dialogueLine;

		// Play interaction fragment music
		GameObject.Find ("Entity Manager").GetComponent<GameBehavior> ().PlayEntityTransitionalFragment (this.isConversing ? this.mostRecentConverserIndex : -1);
			
		Debug.Log ("camera: most recent gaze idx: " + this.mostRecentGazeIndex + ", most recent converser idx: " + this.mostRecentConverserIndex);
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

	public void SetIsAboutToConverseWith(int converserIndex) {
		this.mostRecentConverserIndex = converserIndex;
	}

	public bool GetIsConversing() {
		return this.isConversing;
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

	public void SetMostRecentAvatarPosition(Vector3 position) {
		GameObject.Find ("Entity Manager").GetComponent<GameBehavior> ().SetEntityPosition (this.mostRecentGazeIndex, position);
	}

	public void SetShouldTeleport(bool teleport) {
		this.shouldTeleport = teleport;
	}

	public Vector3 GetMostRecentGazePosition() {
		return this.mostRecentGazePosition;
	}

	public int GetMostRecentGazeIndex() {
		return this.mostRecentGazeIndex;
	}
//	public void SetMostRecentGazeIndex(int idx) {
//		this.mostRecentGazeIndex = idx;
//	}

	public int GetMostRecentConverserIndex() {
		return this.mostRecentConverserIndex;
	}

	public bool GetIsDisembodied() {
		return this.isDisembodied;
	}

	public int GetFramesSinceLastTeleportation() {
		return this.framesSinceLastTeleportation;
	}

//	public float GetGazeAmount() {
//		float amt = 
//	}
}
