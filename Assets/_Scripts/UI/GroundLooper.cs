using System.Collections.Generic;
using UnityEngine;

public class GroundLooper : MonoBehaviour {
	public Transform 	trackThing;
	public float 		jumpDistance;

	private void Update ( ) {
		if ( transform.position.z + 100f < trackThing.position.z ) {
			transform.position += Vector3.forward * jumpDistance;
		}
	}
}