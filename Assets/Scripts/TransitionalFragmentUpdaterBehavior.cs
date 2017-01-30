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

		// TODO: Fix this so that the tail of the fragment doesn't get cut off
		//if (cameraBehavior.GetIsDisembodied()) {
		//	return;
		//}

		Transform gazeEntity = GameObject.Find ("Entity Manager").GetComponent<GameBehavior> ().GetEntity (this.entityIdx);
		int gazeFrames = gazeEntity.GetComponent<TeleportSelfAndGazer> ().GetFramesGazedAt ();

		this.GetComponent<FMODUnity.StudioEventEmitter> ().SetParameter ("framesGazedAt", gazeFrames);
	}
}
