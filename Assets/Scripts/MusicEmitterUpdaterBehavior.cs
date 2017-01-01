// Updates parameters in the FMOD Music Emitter

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
		float rotationOffset = Mathf.Abs (this.transform.eulerAngles.y / 360);
		float timeSinceTeleportation = Mathf.Min (this.GetComponent<CameraBehavior> ().GetFramesSinceLastTeleportation () / framesSinceLastTeleportationThreshold, 1.0f);
		emitter.SetParameter ("Rotation Offset", rotationOffset);
		emitter.SetParameter ("Time Since Teleportation", timeSinceTeleportation);
	}
}
