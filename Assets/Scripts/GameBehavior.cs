using UnityEngine;
using System.Collections;

public class GameBehavior : MonoBehaviour {
	public Transform entity;
	public int numEntities = 50;

	private GameObject camera;
	private Transform[] entities;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Main Camera");
		this.entities = new Transform[numEntities];

		// Instantiate all the entities
		for (int i = 0; i < this.numEntities; i++) {
			float x = Random.Range (-10, 10);
			float z = Random.Range (-10, 10);
			Transform entity = (Transform) Instantiate (this.entity, new Vector3 (x, 1, z), Quaternion.identity);
			this.entities [i] = entity;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Set camera's isGazing by querying entities
		bool cameraIsGazing = false;
		for (int i = 0; i < this.entities.Length; i++) {
			Transform entity = this.entities [i];
			bool isGazedAt = entity.GetComponent<TeleportSelfAndGazer> ().GetIsGazedAt ();
			if (isGazedAt) {
				cameraIsGazing = true;
				break;
			}
		}
		camera.GetComponent<CameraBehavior> ().SetIsGazing (cameraIsGazing);
	}
}
