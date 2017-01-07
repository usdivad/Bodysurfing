// Updates parameters in the FMOD Music Emitter
// 1/7/17: Currently attached to Main Camera

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

		emitter.SetParameter ("Rotation Offset", rotationOffset);
		emitter.SetParameter ("Time Since Teleportation", timeSinceTeleportation);
		emitter.SetParameter ("Is Disembodied?", isDisembodiedFloat);
	}
}
