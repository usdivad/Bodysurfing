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
		this.startingScale = this.transform.localPosition;
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
		gazer.SetMostRecentTeleportPosition (transform.localPosition);
		gazer.SetShouldTeleport (true);

		// Teleport self outside world bounds (disappear)
		//transform.localPosition = new Vector3 (25, 0, 0);
	}
}
