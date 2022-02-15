using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotatingEvent : MonoBehaviour {

	public UnityEvent [ ] 	events;
	public bool 			loops = true;
	public bool 			blocked;

	public void Invoke ( ) {
		if ( blocked ) {
			return;
		}
		if ( events.Length > 0 ) {
			events [ index ].Invoke ( );
			index = ( index + 1 );
			if ( index >= events.Length ) {
				if ( !loops ) {
					blocked = true;
				}
				index -= events.Length;
			}
		}
	}

	private int 			index;
}
