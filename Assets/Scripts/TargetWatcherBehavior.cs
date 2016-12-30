using UnityEngine;
using System.Collections;

public class TargetWatcherBehavior : MonoBehaviour {
	public GameObject target;
	public Vector3 watchOffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt (target.transform.position + watchOffset);
	}
}
