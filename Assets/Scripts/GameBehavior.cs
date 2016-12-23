using UnityEngine;
using System.Collections;

public class GameBehavior : MonoBehaviour {
	public Transform entity;
	public int numEntities = 50;

	private GameObject camera;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Main Camera");

		for (int i = 0; i < this.numEntities; i++) {
			float x = Random.Range (-10, 10);
			float z = Random.Range (-10, 10);
			Instantiate (this.entity, new Vector3 (x, 1, z), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
