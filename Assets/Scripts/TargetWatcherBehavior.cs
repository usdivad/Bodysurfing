using UnityEngine;
using System.Collections;

public class TargetWatcherBehavior : MonoBehaviour {
	public GameObject target;
	public Vector3 watchOffset;
	public float distanceThreshold;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float distance = (this.transform.position - target.transform.position).magnitude;
		if (distance < distanceThreshold) {
			this.transform.LookAt (target.transform.position + watchOffset);
		}
	}
}
