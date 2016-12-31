using UnityEngine;
using System.Collections;

public class CardboardToggleBehavior : MonoBehaviour {

	private bool vrModeEnabled;

	// Use this for initialization
	void Start () {
		this.vrModeEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		// Cardboard.SDK.VRModeEnabled = this.vrModeEnabled;
	}

	public void ToggleVRMode() {
		this.vrModeEnabled = !this.vrModeEnabled;
		PlayerPrefs.SetInt ("Vr Mode", this.vrModeEnabled ? 1 : 0);
	}
}
