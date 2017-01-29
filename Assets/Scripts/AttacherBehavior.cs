/*
 * Attaches current position to another entity's position.
 * (e.g. for FMOD music emitter attaching to camera)
 */

using UnityEngine;
using System.Collections;

public class AttacherBehavior : MonoBehaviour {
	public GameObject entityAttachedTo;
	public float positionOffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Move us to the right position, taking offset into account
		Vector3 entityPos = this.entityAttachedTo.transform.position;
		Vector3 entityFwd = this.entityAttachedTo.transform.forward;
		this.transform.position = entityPos + (entityFwd * this.positionOffset);

		// Rotate us in the right direction (this is only for the dialogue text at the moment)
		//this.transform.LookAt(entityFwd * -1);
		//this.transform.LookAt (this.transform.position - entityPos);
		this.transform.rotation = Quaternion.LookRotation(this.transform.position - entityPos); // Face the entity
	}
}
