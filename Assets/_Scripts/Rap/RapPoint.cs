using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class RapPoint : MonoBehaviour {
	
	public float 			distance;
	public int 				buttonNeeded;
	public TextMeshProUGUI 	lyricField;
	public Transform 		arrow;
	
	public void Setup ( float startDistance, int direction ) {
		this.startDistance = startDistance;
		if ( direction <= 0 ) {
			buttonNeeded = Random.Range ( 1, 5 );
		} else {
			buttonNeeded = direction;
		}
		arrow.localEulerAngles = new Vector3 ( 0f, 0f, ( buttonNeeded - 1 ) * 90f );
	}

	public void UpdateDistance ( float musicProgress, float distanceRatio, float horizontalRatio ) {
		distance = startDistance - musicProgress;
		transform.localPosition = Vector3.down * distance * distanceRatio + Vector3.right * horizontalRatio * offsetNums [ buttonNeeded - 1 ];
	}

	public void SetLyric ( string lyric, bool isPhaseOne ) {
		lyricField.transform.localPosition = Vector3.left * ( ( isPhaseOne ? 200f : -85f ) + transform.localPosition.x );
		lyric = cleanRap.Replace( lyric, "" );
		lyricField.text = lyric;
	}

	private float[] offsetNums;
	private Regex cleanRap = new Regex ( "[^a-zA-Z0-9 -]" );

	private void Awake ( ) {
		 offsetNums = new float [ 4 ] { 1.5f, 0.5f, -1.5f, -0.5f };
	}

	[SerializeField] private float 	startDistance;
}