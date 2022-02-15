using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton < Player > {

	public float 		maxSpeed;
	public float 		accelTime;
	public LayerMask 	layerMask;

	public Animator  	anim;

	private Vector2 	velocity;
	private Vector2 	refVelocity;

	private void Update ( ) {
		Vector2 targetVelocity = Vector2.zero;
		if ( !RapManager.isRapping ) {
			float x = 0f;
			float y = 0f;
			if ( Input.GetKey ( KeyCode.RightArrow ) || Input.GetKey ( KeyCode.D ) ) {
				x += 1f;
			} if ( Input.GetKey ( KeyCode.UpArrow ) || Input.GetKey ( KeyCode.W ) ) {
				y += 1f;
			} if ( Input.GetKey ( KeyCode.LeftArrow ) || Input.GetKey ( KeyCode.A ) ) {
				x -= 1f;
			} if ( Input.GetKey ( KeyCode.DownArrow ) || Input.GetKey ( KeyCode.S ) ) {
				y -= 1f;
			}
			targetVelocity = new Vector2 ( x, y ).normalized * maxSpeed;
		}
		velocity = Vector2.SmoothDamp(velocity, targetVelocity, ref refVelocity, accelTime);
		Vector3 turnOffset = new Vector3 ( velocity.x, 0f, velocity.y ) * Time.deltaTime;
		if ( velocity.magnitude > 0f && Physics.BoxCast( transform.position, Vector3.one * 1.2f, turnOffset, Quaternion.identity, turnOffset.magnitude, layerMask) ) {
			velocity = Vector2.zero;
			turnOffset = new Vector3 ( velocity.x, 0f, velocity.y ) * Time.deltaTime;
		}
		transform.position += turnOffset;
		anim.SetBool( "running", velocity.magnitude > 0.5f );
		if ( velocity.x > 0f ) {
			transform.localScale = new Vector3 ( 1f, 1f, 1f );
		} else if ( velocity.x < 0f ) {
			transform.localScale = new Vector3 ( -1f, 1f, 1f );
		}
	}
}