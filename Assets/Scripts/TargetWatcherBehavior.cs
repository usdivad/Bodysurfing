using UnityEngine;
using System.Collections;

public class TargetWatcherBehavior : MonoBehaviour {
	// NOTE: For CitizenAvatars, these values get overridden in the main GameBehavior.Start()
	//       method, no matter what you set them to in the CitizenAvatar prefab
	public GameObject target;
	public Vector3 watchOffset;
	public float watchDistanceThreshold;
	public float rotationSpeed;
	// public float stopDistanceThreshold;
	// public bool stopTarget;

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
		if (distance < this.watchDistanceThreshold) {
			//this.transform.LookAt (target.transform.position + watchOffset);
			Quaternion lookRot = Quaternion.LookRotation (relativePos);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, lookRot, this.rotationSpeed);

			// This doesn't really work with >1 watcher... nice try though
			/*
			if (this.stopTarget && target.GetComponent<CameraBehavior>() != null) {
				float targetVelMultiplier = Mathf.Min(Mathf.Max(distance - this.stopDistanceThreshold, 0f), 1f);
				target.GetComponent<CameraBehavior> ().SetCurVelMultiplier (targetVelMultiplier);
				Debug.Log ("distance: " + distance + ", vel mul: " + target.GetComponent<CameraBehavior> ().GetCurVelMultiplier ());
			}
			*/
		}
	}

	public bool GetBypass() {
		return this.bypass;
	}
	public void SetBypass(bool b) {
		this.bypass = b;
	}
}
