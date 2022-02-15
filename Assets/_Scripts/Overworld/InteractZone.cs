using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractZone : MonoBehaviour {

	public UnityEvent 	onInteract;
	public Animator 	anim;
	
	private bool 		isEntered;
	private bool 		hideAnim;

	private void Update ( ) {
		if ( isEntered && !RapManager.isRapping ) {
			if (Input.GetKeyDown ( KeyCode.Space ) ) {
				onInteract.Invoke ( );
        		anim.SetBool ( "show", false );
			}
		}
	}

	private void OnTriggerEnter ( Collider coll ) {
        isEntered = true;
        anim.SetBool ( "show", true );
    }

    private void OnTriggerExit ( Collider other ) {
        isEntered = false;
        anim.SetBool ( "show", false );
    }
}
