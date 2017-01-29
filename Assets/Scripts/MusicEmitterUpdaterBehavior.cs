/*
 * Updates parameters in the FMOD Music Emitter
 * 1/7/17: Currently attached to Main Camera
 *
 */

using UnityEngine;
using System.Collections;

public class MusicEmitterUpdaterBehavior : MonoBehaviour {
	public FMODUnity.StudioEventEmitter emitter;

	private float framesSinceLastTeleportationThreshold = 300f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject mainCamera = GameObject.Find ("Main Camera");
		CameraBehavior cameraBehavior = mainCamera.GetComponent<CameraBehavior> ();
		float rotationOffset = Mathf.Abs (mainCamera.transform.eulerAngles.y / 360);
		float timeSinceTeleportation = Mathf.Min (cameraBehavior.GetFramesSinceLastTeleportation () / framesSinceLastTeleportationThreshold, 1.0f);
		//float gazeAmount = cameraBehavior.GetGazeAmount ();
		float isDisembodiedFloat = cameraBehavior.GetIsDisembodied() ? 1.0f : 0.0f;
		float characterIndex = (cameraBehavior.GetIsDisembodied() ? cameraBehavior.GetMostRecentGazeIndex () : -1.0f) // Either character at most recent gazed index, or Eve character itself
								+ 1.01f; // Offset since Eve is 0.0-1.0

		// For GazerAmbience
		//emitter.SetParameter ("Rotation Offset", rotationOffset);
		//emitter.SetParameter ("Time Since Teleportation", timeSinceTeleportation);
		//emitter.SetParameter ("Is Disembodied?", isDisembodiedFloat);

		// For AllCharactersMain
		emitter.SetParameter("timeSinceTeleportation", timeSinceTeleportation);
		emitter.SetParameter ("characterIndex", characterIndex);

		Debug.Log ("time since teleportation: " + timeSinceTeleportation + ", character idx: " + characterIndex);
	}
}
