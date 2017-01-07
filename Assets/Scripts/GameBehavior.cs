using UnityEngine;
using System.Collections;

public class GameBehavior : MonoBehaviour {
	public Transform entityType;
	public Material[] entityMaterials; // [CitizenAvatar_Female, CitizenAvatar_Male]
	public int numEntities; // 50
	public float positionRange;
	public float positionOffset;

	private GameObject mainCamera;
	private Transform[] entities;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find ("Main Camera");
		this.entities = new Transform[numEntities];
		float y = 0.5f;

		// Instantiate all the entities
		for (int i = 0; i < this.numEntities; i++) {
			// Set up transform position and rotation
			float x = Random.Range (positionRange*-1, positionRange);
			float z = Random.Range (positionRange*-1, positionRange);
			if (x < 0 && x > positionOffset * -1) {
				x -= positionOffset;
			}
			else if (x < positionOffset) {
				x += positionOffset;
			}
			if (z < 0 && z > positionOffset * -1) {
				z -= positionOffset;
			}
			else if (z < positionOffset) {
				z += positionOffset;
			}
			Transform entity = (Transform) Instantiate (this.entityType, new Vector3 (x, y, z), Quaternion.identity);
			//entity.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
			entity.Rotate (new Vector3 (0, Random.value * 360.0f, 0));

			// Set up material
			Material entityMaterial = this.entityMaterials[Random.Range (0, this.entityMaterials.Length)];
			entity.GetComponent<MeshRenderer> ().material = entityMaterial;

			// Set up target watcher behavior
			TargetWatcherBehavior twb = entity.GetComponent<TargetWatcherBehavior> ();
			twb.target = mainCamera;
			twb.watchOffset = new Vector3 (0, 0, 0);
			twb.watchDistanceThreshold = 5.0f;
			twb.rotationSpeed = Random.value * 0.2f + 0.1f;

			this.entities [i] = entity;
		}
	}
	
	// Update is called once per frame
	void Update () {
		CameraBehavior cameraBehavior = mainCamera.GetComponent<CameraBehavior> ();

		// Set camera's isGazing by querying entities
		int cameraIsGazingAt = -1;
		int cameraIsAboutToConverseWith = -1;
		for (int i = 0; i < this.entities.Length; i++) {
			Transform entity = this.entities [i];

			if (cameraBehavior.GetIsDisembodied ()) {
				entity.GetComponent<TargetWatcherBehavior> ().SetBypass(false);
				bool isAboutToConverse = entity.GetComponent<TeleportSelfAndGazer> ().GetIsAboutToConverse ();
				if (isAboutToConverse) {
					cameraIsAboutToConverseWith = i;
				}
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
		cameraBehavior.SetIsGazingAt (cameraIsGazingAt);
		cameraBehavior.SetIsAboutToConverseWith (cameraIsAboutToConverseWith);
	}

	public void SetEntityPosition(int i, Vector3 pos) { // Previously "TeleportEntity()"
		this.entities [i].position = pos;
	}

	public Vector3 GetEntityPosition(int i) {
		return this.entities [i].position;
	}
}
