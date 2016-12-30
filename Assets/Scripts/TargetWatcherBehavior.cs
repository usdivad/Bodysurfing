using UnityEngine;
using System.Collections;

public class TargetWatcherBehavior : MonoBehaviour {
	public GameObject target;
	public Vector3 watchOffset;
	public float distanceThreshold;
	public float rotationSpeed;

	private bool bypass;

	// Use this for initialization
	void Start () {
		this.bypass = false;
	}

	// Update is called once per frame
	void Update () {
		if (this.bypass) {
			return;
		}

		Vector3 relativePos = target.transform.position - this.transform.position;
		float distance = relativePos.magnitude;
		if (distance < distanceThreshold) {
			//this.transform.LookAt (target.transform.position + watchOffset);
			Quaternion lookRot = Quaternion.LookRotation (relativePos);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, lookRot, this.rotationSpeed);
		}
	}

	public bool GetBypass() {
		return this.bypass;
	}
	public void SetBypass(bool b) {
		this.bypass = b;
	}
}
