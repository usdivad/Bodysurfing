/*
 * Main game behavior; spawns entities, handles some gaze indexing, holds world boundaries
 *
 */

using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class GameBehavior : MonoBehaviour {
	//public Transform entityType;
	public Transform[] entityTypes;
	//public Material[] entityMaterials; // [CitizenAvatar_Female, CitizenAvatar_Male]
	//public int numEntities; // 50
	public float positionRange;
	public float positionOffset;
	//public float[] worldBoundariesArr;
	public Vector2 worldBoundariesX;
	public Vector2 worldBoundariesZ;

	private GameObject mainCamera;
	private Transform[] entities;
	//private Dictionary<string, float> worldBoundariesDict;

	// Use this for initialization
	void Start () {
		// Set up member variables
		this.mainCamera = GameObject.Find ("Main Camera");
		this.entities = new Transform[this.entityTypes.Length];

		// Instantiate all the entities
		float y = 0.5f;
		for (int i = 0; i < this.entityTypes.Length; i++) {
			// Set up transform position and rotation
			float x = Random.Range (positionRange*-1, positionRange);
			float z = Random.Range (positionRange*-1, positionRange);
			if (x < 0 && x > positionOffset * -1) {
				x -= positionOffset;
			}
			else if (x >= 0 && x < positionOffset) {
				x += positionOffset;
			}
			if (z < 0 && z > positionOffset * -1) {
				z -= positionOffset;
			}
			else if (z >= 0 && z < positionOffset) {
				z += positionOffset;
			}
			Transform entityType = entityTypes [i];
			Transform entity = (Transform) Instantiate (entityType, new Vector3 (x, y, z), Quaternion.identity);
			//entity.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
			entity.Rotate (new Vector3 (0, Random.value * 360.0f, 0));

			// Set up material
			//Material entityMaterial = this.entityMaterials[Random.Range (0, this.entityMaterials.Length)];
			//entity.GetComponent<MeshRenderer> ().material = entityMaterial;

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
				// Set camera's most recent converser
				entity.GetComponent<TargetWatcherBehavior> ().SetBypass(false);
				bool isAboutToConverse = entity.GetComponent<TeleportSelfAndGazer> ().GetIsAboutToConverse ();
				if (isAboutToConverse) {
					cameraIsAboutToConverseWith = i;
				}
			}
			else {
				// Set camera's most recent gazed at entity
				entity.GetComponent<TargetWatcherBehavior> ().SetBypass(true);
				bool isGazedAt = entity.GetComponent<TeleportSelfAndGazer> ().GetIsGazedAt ();
				if (isGazedAt) {
					cameraIsGazingAt = i;
					// break;
				}

				// Reset position if out of bounds
				// TODO: Track down the root cause of this bug; probably to do with conflicting teleport/gaze settings
				if (!this.PositionIsInWorldBoundaries(entity.position))
				{
					entity.GetComponent<TeleportSelfAndGazer> ().Reset ();
				}
			}
		}
		cameraBehavior.SetIsGazingAt (cameraIsGazingAt);
		cameraBehavior.SetIsAboutToConverseWith (cameraIsAboutToConverseWith);
	}

	public void SetEntityPosition(int i, Vector3 pos) { // Previously "TeleportEntity()"
		Vector3 posPrev = this.entities[i].position;
		this.entities [i].position = pos;
		Debug.Log ("entity " + i + ": " + posPrev + "->" + pos);
	}

	public Vector3 GetEntityPosition(int i) {
		return this.entities [i].position;
	}

	public bool PositionIsInWorldBoundaries(Vector3 pos) {
		if (pos.x < this.worldBoundariesX.x || pos.x > this.worldBoundariesX.y) {
			return false;
		}
		if (pos.z < this.worldBoundariesZ.x || pos.z > this.worldBoundariesZ.y) {
			return false;
		}
		return true;
	}

	public void PlayEntityTransitionalFragment(int entityIdx) {
		for (int i = 0; i < this.entities.Length; i++) {
			this.entities [i].GetComponent<FMODUnity.StudioEventEmitter> ().SetParameter ("conversingWith", i == entityIdx ? 1.0f : 0.0f);
			if (i == entityIdx) {
				Debug.Log ("playing " + i + "!");
			}
		}
	}

	public Transform GetEntity(int i) {
		return this.entities[i];
	}
}
