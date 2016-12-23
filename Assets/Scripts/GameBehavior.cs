using UnityEngine;
using System.Collections;

public class GameBehavior : MonoBehaviour {
	public Transform entity;
	public int numEntities = 50;

	private GameObject camera;
	private float curVel;
	private float minVel;
	private float maxVel;
	private float velStep;
	private Vector3 prevForward;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Main Camera");
		this.minVel = 0.01f;
		this.maxVel = 0.2f;
		this.curVel = this.minVel;
		this.velStep = 0.001f;
		this.prevForward = new Vector3 (0, 0, 0);

		for (int i = 0; i < this.numEntities; i++) {
			float x = Random.Range (-10, 10);
			float z = Random.Range (-10, 10);
			Instantiate (this.entity, new Vector3 (x, 1, z), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// move forward in the direction that the camera is facing
		Transform transform = camera.GetComponent<Transform> ();
		Vector3 pos = transform.position;

		if (transform.forward == this.prevForward) {
			this.curVel += this.velStep;
		}
		else {
			this.curVel -= this.velStep;
			// this.curVel = this.minVel;
		}

		// TODO: adjust if I'm gazing

		this.curVel = Mathf.Min (Mathf.Max (this.curVel, this.minVel), this.maxVel);

		Vector3 mvmt = transform.forward * this.curVel;
		this.prevForward = transform.forward;
		//Quaternion rot = transform.rotation;
		//Debug.Log (rot);
		pos.x += mvmt.x;
		pos.z += mvmt.z;
		transform.position = pos;
	}
}
