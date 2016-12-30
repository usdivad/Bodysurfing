using UnityEngine;
using System.Collections;

public class GameBehavior : MonoBehaviour {
	public Transform entityType;
	public Material[] entityMaterials; // [CitizenAvatar_Female, CitizenAvatar_Male]
	public int numEntities; // 50
	public float positionRange;

	private GameObject camera;
	private Transform[] entities;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Main Camera");
		this.entities = new Transform[numEntities];

		// Instantiate all the entities
		for (int i = 0; i < this.numEntities; i++) {
			// Instantiate and transform
			float x = Random.Range (positionRange*-1, positionRange);
			float z = Random.Range (positionRange*-1, positionRange);
			Material entityMaterial = this.entityMaterials[Random.Range (0, this.entityMaterials.Length)];
			Transform entity = (Transform) Instantiate (this.entityType, new Vector3 (x, 1, z), Quaternion.identity);
			//entity.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
			entity.Rotate (new Vector3 (0, Random.value * 360.0f, 0));
			entity.GetComponent<MeshRenderer> ().material = entityMaterial;

			// Set up target watcher behavior
			TargetWatcherBehavior twb = entity.GetComponent<TargetWatcherBehavior> ();
			twb.target = camera;
			twb.watchOffset = new Vector3 (0, 0, 0);
			twb.distanceThreshold = 5.0f;
			twb.rotationSpeed = Random.value * 0.5f;

			this.entities [i] = entity;
		}
	}
	
	// Update is called once per frame
	void Update () {
		CameraBehavior cameraBehavior = camera.GetComponent<CameraBehavior> ();

		// Set camera's isGazing by querying entities
		int cameraIsGazingAt = -1;
		for (int i = 0; i < this.entities.Length; i++) {
			Transform entity = this.entities [i];

			if (cameraBehavior.GetIsDisembodied ()) {
				entity.GetComponent<TargetWatcherBehavior> ().SetBypass(false);
			}
			else {
				entity.GetComponent<TargetWatcherBehavior> ().SetBypass(true);
				bool isGazedAt = entity.GetComponent<TeleportSelfAndGazer> ().GetIsGazedAt ();
				if (isGazedAt) {
					cameraIsGazingAt = i;
					// break;
				}
			}
		}
		camera.GetComponent<CameraBehavior> ().SetIsGazingAt (cameraIsGazingAt);
	}

	public void teleportEntity(int i, Vector3 pos) {
		this.entities [i].position = pos;
	}
}
