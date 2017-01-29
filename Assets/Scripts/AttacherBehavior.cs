/*
 * Attaches current position to another entity's position.
 * (e.g. for FMOD music emitter attaching to camera)
 */

using UnityEngine;
using System.Collections;

public class AttacherBehavior : MonoBehaviour {
	public GameObject entityAttachedTo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = this.entityAttachedTo.transform.position;

		// TODO:
		// - Position offsets
		// - Rotations (for text)
	}
}
