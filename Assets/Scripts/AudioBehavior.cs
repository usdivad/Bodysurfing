/*
 * Audio based on native Unity sound engine. Currently unused
 */

using UnityEngine;
using System.Collections;

public class AudioBehavior : MonoBehaviour {
	// Use this for initialization
	void Start () {
		// testing mic input
		foreach (string device in Microphone.devices) {
			Debug.Log ("mic: " + device);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
