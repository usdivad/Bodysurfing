using UnityEngine;
using System.Collections;

public class GameBehavior : MonoBehaviour {
	public Transform entity;
	public int numEntities = 50;

	private GameObject camera;
	private float vel;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Main Camera");
		vel = 0.01f;

		for (int i = 0; i < numEntities; i++) {
			float x = Random.Range (-10, 10);
			float z = Random.Range (-10, 10);
			Instantiate (entity, new Vector3 (x, 1, z), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// move forward in the direction that the camera is facing
		Transform transform = camera.GetComponent<Transform> ();
		Vector3 pos = transform.position;
		Vector3 mvmt = transform.forward * vel;
		//Quaternion rot = transform.rotation;
		//Debug.Log (rot);
		pos.x += mvmt.x;
		pos.z += mvmt.z;
		transform.position = pos;
	}
}
