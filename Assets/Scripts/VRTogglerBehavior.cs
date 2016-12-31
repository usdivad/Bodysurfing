using UnityEngine;
using System.Collections;

public class VRTogglerBehavior : MonoBehaviour {

	private bool isGazedAt;
	private int framesGazedAtThreshold;
	private int framesGazedAt;

	// Use this for initialization
	void Start () {
		this.isGazedAt = false;
		this.framesGazedAtThreshold = 60;
		this.framesGazedAt = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.isGazedAt) {
			this.framesGazedAt++;
		}

		if (this.framesGazedAt > this.framesGazedAtThreshold) {
			this.ToggleVRMode ();
			this.framesGazedAt = 0;
		}
	}

	public void ToggleVRMode() {
		GvrViewer.Instance.VRModeEnabled = !GvrViewer.Instance.VRModeEnabled;
	}

	public void OnGazeEnter() {
		this.SetGazedAt (true);
	}

	public void OnGazeExit() {
		this.SetGazedAt (false);
	}

	public void SetGazedAt(bool gazedAt) {
		this.isGazedAt = gazedAt;
		this.framesGazedAt = 0;
		GetComponent<Renderer>().material.color = gazedAt ? Color.magenta : Color.white;
	}
}
