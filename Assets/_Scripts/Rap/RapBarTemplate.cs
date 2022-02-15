using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RapBarTemplate : MonoBehaviour {

	public float 					BPM = 90f;
	public float 					startBreak;
	public float 					phaseOneStart;
	public float 					phaseOnePrestart;
	public float 					phaseTwoLength;
	public float 					startZoom = 6f;
	public AudioClip 				clip;
	public RapPointTemplate[] 		points;
	public Transform 				opponent;
	public string 					opponentName;
	public UnityEvent 				onWin;
	public AudioClip[] 				bleeps;

	public void Begin ( ) {
		RapManager.main.SpawnTemplate ( this );
	}
}

[System.Serializable]
public struct RapPointTemplate {
	public int 		direction;
	public float 	start;
	public string 	ownText;
	public string 	text;
}