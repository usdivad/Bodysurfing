/*
 * Handle transitional fragments
 * 
 */

using UnityEngine;
using System.Collections;

public class TransitionalFragmentUpdaterBehavior : MonoBehaviour {
	public int entityIdx;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject mainCamera = GameObject.Find ("Main Camera");
		CameraBehavior cameraBehavior = mainCamera.GetComponent<CameraBehavior> ();
		//int gazeIdx = cameraBehavior.GetMostRecentGazeIndex();
		int gazeFrames = 0;
		int teleportationFramesThreshold = 60;

		// TODO: Fix this so that the tail of the fragment doesn't get cut off
		//if (cameraBehavior.GetIsDisembodied()) {
		//	return;
		//}

		// Query the number of frames gazed at, then set our emitter's parameter
		if (entityIdx < 0) { // Eve (Gazer)
			gazeFrames = GameObject.Find("Gazer Avatar").GetComponent<GazerAvatarGazeBehavior>().GetFramesGazedAt();
			//if (!cameraBehavior.GetIsDisembodied() && cameraBehavior.GetFramesSinceLastTeleportation () < teleportationFramesThreshold) {
			//	return;
			//}
		}
		else { // Everyone else
			Transform gazeEntity = GameObject.Find ("Entity Manager").GetComponent<GameBehavior> ().GetEntity (this.entityIdx);
			gazeFrames = gazeEntity.GetComponent<TeleportSelfAndGazer> ().GetFramesGazedAt ();
		}
		this.GetComponent<FMODUnity.StudioEventEmitter> ().SetParameter ("framesGazedAt", gazeFrames);
	}
}
