using UnityEngine;
using System.Collections;

public class GameBehavior : MonoBehaviour {
	private GameObject camera;
	private float vel;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Main Camera");
		vel = 0.01f;
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
