/*
 * Behavior to handle GazerAvatar teleportation
 * 
 */

using UnityEngine;
using System.Collections;

public class GazerAvatarGazeBehavior : MonoBehaviour {
	public int framesGazedAtThreshold;
	public float distanceThreshold;
	public float scaleFactor; // 0.5f

	private int framesGazedAt;
	private bool isGazedAt;
	private Vector3 startingScale;

	// Use this for initialization
	void Start () {
		this.framesGazedAt = 0;
		this.isGazedAt = false;
		this.startingScale = this.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		CameraBehavior gazer = GameObject.Find ("Main Camera").GetComponent<CameraBehavior>();

		// Update how long we've been gazed at
		if (this.isGazedAt && gazer.GetIsDisembodied()) {
			this.framesGazedAt++;
			Debug.Log ("framesGAzedAt: " + this.framesGazedAt);
		}

		// Adjust dimensions based on gazed time
		//transform.localScale = ((float) this.framesGazedAt / this.framesGazedAtThreshold) * (scaleFactor * this.startingScale) + this.startingScale;

		// Calculate distance from player (camera)
		float distance = (gazer.transform.position - this.transform.position).magnitude;

		// Teleport if we're over the gazed threshold or under the distance threshold
		if (this.framesGazedAt >= this.framesGazedAtThreshold || distance < distanceThreshold) {
			Teleport();
		}
	}

	public void OnGazeEnter() {
		this.isGazedAt = true;
	}

	public void OnGazeExit() {
		this.isGazedAt = false;

		this.framesGazedAt = 0;
	}

	public void Teleport() {
		// Teleport gazer
		CameraBehavior gazer = GameObject.Find ("Main Camera").GetComponent<CameraBehavior>();
		//Vector3 gazerPrevPosition = GameObject.Find ("Main Camera").transform.localPosition;
		Vector3 gazerPrevPosition = gazer.transform.position;
		Debug.Log ("gazerPrevPosition before: " + gazerPrevPosition);


		float distanceOffset = this.distanceThreshold;
		float distance = (gazer.transform.position - this.transform.position).magnitude;

		if (distance < this.distanceThreshold) {
			if (gazerPrevPosition.x < this.transform.position.x) {
				gazerPrevPosition.x -= distanceOffset;
			}
			else {
				gazerPrevPosition.x += distanceOffset;
			}

			if (gazerPrevPosition.z < this.transform.position.z) {
				gazerPrevPosition.z -= distanceOffset;
			}
			else {
				gazerPrevPosition.z += distanceOffset;
			}
		}
		Debug.Log ("gazerPrevPosition after: " + gazerPrevPosition);


		gazer.SetMostRecentTeleportPosition (gazerPrevPosition);
		//gazer.SetMostRecentAvatarPosition (gazerPrevPosition);
		gazer.SetShouldTeleport (true);

		// Teleport self outside world bounds (disappear)
		//transform.localPosition = new Vector3 (25, 0, 0);
	}

	public void MoveSelfAndChildrenTo(Vector3 pos) {
		this.transform.position = pos;
		Transform[] myChildren = GetComponentsInChildren<Transform> ();
		for (int i=0; i<myChildren.Length; i++) {
			myChildren [i].position = pos;
			Debug.Log (i);
		}
	}

	public int GetFramesGazedAt() {
		return this.framesGazedAt;
	}
}
