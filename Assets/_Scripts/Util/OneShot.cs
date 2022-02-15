using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class OneShot : MonoBehaviour {
	public AudioSource 		source;
	public bool 			play;

	private void Update ( ) {
		if ( play ) {
			if ( !source.isPlaying ) {
				source.Play ( );
			}
			play = false;
		}
	}
}