using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RapManager : Singleton < RapManager > {

	public static bool	isRapping { get { return main._isRapping; } }

	public float 				acceptableRange = 0.5f;
	public RapPoint 			rapPointPrefab;
	public float 				distanceRatio = 1f;
	public float 				horizontalRatio = 1f;
	public Transform 			rapPointParent;
	public AudioSource 			audioSource;
	public TextMeshProUGUI 		lyricField;
	public Animator 			rapUIAnim;
	public Animator 			crunchAnim;
	public Animator 			textBoxAnim;
	public Animator 			bgmAnimator;
	public TextMeshProUGUI 		crunchText;
	public float 				scoreNeeded = 0.8f;

	public void AddPoint ( RapPoint point ) {
		if ( !currentPoints.Contains ( point ) ) {
			currentPoints.Add ( point );
		}
	}

	public void SpawnTemplate ( RapBarTemplate template ) {
		if ( _isRapping ) {
			return;
		}
		rapUIAnim.SetInteger ( "phase", 0 );
		didPlayerWin = true;
		zoomed = false;
		templateIndex = 0;
		phaseOne = true;
		endingShown = false;
		hit = 0;
		total = 0;
		_isRapping = true;
		musicProgress = 0;
		this.template = template;
		BPM = template.BPM;
		audioSource.clip = template.clip;
		audioSource.Play();
		bgmAnimator.SetBool ( "on", false );
		for ( int i = 0; i < template.points.Length; i++ ) {
			GameObject go = Instantiate( rapPointPrefab.gameObject, rapPointParent );
			RapPoint newRapPoint = go.GetComponent < RapPoint > ( );
			newRapPoint.Setup ( template.points [ i ].start + template.startBreak, template.points [ i ].direction );
			AddPoint ( newRapPoint );
			newRapPoint.UpdateDistance ( musicProgress , distanceRatio, horizontalRatio );
			newRapPoint.SetLyric ( template.points [ i ].ownText, true );
		}
		backupList = new List < RapPoint > ( currentPoints );
		UpdateLyrics ( );
		BleepManager.main.UpdateEnemyBleeps ( template.bleeps );
	}

	private float 					musicProgress;
	private float 					BPM;
	
	private List < RapPoint > 		currentPoints = new List < RapPoint > ( );
	private List < RapPoint > 		backupList;
	private RapBarTemplate 			template;

	private int 					hit;
	private int 					total;
	private bool 					_isRapping;
	private bool 					phaseOne;
	private bool 					endingShown;
	private int 					templateIndex;
	private bool[] 					failures;
	private bool 					zoomed;
	private bool 					didPlayerWin;

	private void Start ( ) {
		rapUIAnim.playbackTime = 0.5f;
	}

	private void Update ( ) {
		if ( !_isRapping ) {
			return;
		}
		if ( !audioSource.isPlaying ) {
			EndRap ( );
			return;
		}
		float audioSourceProgress = audioSource.time * BPM / 60f;
		if ( phaseOne && !zoomed && audioSourceProgress > template.startBreak - template.startZoom ) {
			CameraManager.main.TargetTransform ( template.opponent );
			zoomed = true;
			textBoxAnim.SetInteger ( "phase", 1 );
		}
		if ( phaseOne && ( template.startBreak + template.phaseOneStart - template.phaseOnePrestart ) < audioSourceProgress ) {
			EndPhaseOne ( );
		}

		int i = 0;
		if ( !phaseOne ) {
			while ( i < currentPoints.Count ) {
				if ( currentPoints [ i ].distance <= - acceptableRange ) {
					OnFailPoint ( currentPoints [ i ] );
				} else {
					i++;
				}
			}
		}
		musicProgress = Mathf.Max( musicProgress, audioSourceProgress );
		foreach ( RapPoint point in currentPoints ) {
			point.UpdateDistance ( phaseOne ? musicProgress : ( musicProgress - template.phaseOneStart), distanceRatio, horizontalRatio );
		}
		if ( phaseOne ) {
			if ( currentPoints.Count > 0 && currentPoints[ 0 ].distance <= 0 ) {
				currentPoints [ 0 ].gameObject.SetActive ( false );
				currentPoints.Remove ( currentPoints[ 0 ] );
				templateIndex ++;
				BleepManager.main.Bleep ( false );
				UpdateLyrics ( );
			}
		} else if ( currentPoints.Count > 0 ) {
			if ( Mathf.Abs ( currentPoints[ 0 ].distance ) <= acceptableRange ) {
				OnPointPressable ( currentPoints[ 0 ] );
			}
		}
		if ( !phaseOne && !endingShown && audioSourceProgress > ( template.startBreak + template.phaseOneStart + template.phaseTwoLength ) ) {
			ShowResults ( );
		}
	}

	private void EndPhaseOne ( ) {
		phaseOne = false;
		currentPoints = new List < RapPoint > ( backupList );
		textBoxAnim.SetInteger ( "phase", 2 );
		for ( int i = 0; i < currentPoints.Count; i++ ) {
			RapPoint point = currentPoints [ i ];
			point.gameObject.SetActive ( true );
			point.UpdateDistance ( ( musicProgress - template.phaseOneStart), distanceRatio, horizontalRatio );
			point.SetLyric ( template.points [ i ].text, false );
		}
		templateIndex = 0;
		UpdateLyrics ( );
		failures = new bool [ currentPoints.Count ];
		CameraManager.main.TargetTransform ( Player.main.transform );
		rapUIAnim.SetInteger ( "phase", 1 );
	}

	private void ShowResults ( ) {
		endingShown = true;
		float scorePercent = ( float ) hit / ( float ) total; 
		didPlayerWin = Mathf.Floor( scorePercent * 100f ) >= Mathf.Floor( scoreNeeded * 100f );
		lyricField.text = GetEndingMessage ( scorePercent );
		CameraManager.main.TargetTransform ( didPlayerWin ? Player.main.transform : template.opponent, 2 );
	}

	private void EndRap ( ) {
		if ( didPlayerWin ) {
			template.onWin.Invoke ( );
		}
		bgmAnimator.SetBool ( "on", true );
		_isRapping = false;
		CameraManager.main.RestoreDefault ( );
		rapUIAnim.SetInteger ( "phase", 2 );
		crunchAnim.SetTrigger ( "crunch" );
		crunchText.text = didPlayerWin ? "Crunch" : "Game Over\nYou got roasted";
		textBoxAnim.SetInteger ( "phase", 0 );
	}

	private void OnPointPressable ( RapPoint point ) {
		int buttonPressed = 0;
		if ( Input.GetKeyDown ( KeyCode.RightArrow ) || Input.GetKeyDown ( KeyCode.D ) ) {
			buttonPressed = 1;
		} else if ( Input.GetKeyDown ( KeyCode.UpArrow ) || Input.GetKeyDown ( KeyCode.W ) ) {
			buttonPressed = 2;
		} else if ( Input.GetKeyDown ( KeyCode.LeftArrow ) || Input.GetKeyDown ( KeyCode.A ) ) {
			buttonPressed = 3;
		} else if ( Input.GetKeyDown ( KeyCode.DownArrow ) || Input.GetKeyDown ( KeyCode.S ) ) {
			buttonPressed = 4;
		}
		if ( buttonPressed > 0 ) {
			if ( buttonPressed == point.buttonNeeded ) {
				OnPassPoint ( point );
				BleepManager.main.Bleep ( true );
			} else {
				OnFailPoint ( point );
			}
		}
	}

	private void OnPassPoint ( RapPoint point ) {
		failures [ total ] = false;
		RemovePoint ( point );
		hit ++;
		total ++;
		templateIndex ++;
		UpdateLyrics ( );
	}

	private void OnFailPoint ( RapPoint point ) {
		failures [ total ] = true;
		RemovePoint ( point );
		total ++;
		templateIndex ++;
		UpdateLyrics ( );
	}

	private void RemovePoint ( RapPoint point ) {
		currentPoints.Remove ( point );
		Destroy ( point.gameObject );
	}

	private void UpdateLyrics ( ) {
		lyricField.text = TemplateToString ( );
	}

	private string TemplateToString ( ) {
		string str = "<color=#2b2b2bff>";
		for ( int i = 0; i < template.points.Length; i++ ) {
			string text = phaseOne ? template.points [ i ].ownText : template.points [ i ].text;
			text = text.Replace("~", "\n");
			if ( templateIndex == i ) {
				text = "<color=#6bb9f0ff>" + text + "<color=#e4e4e4ff>";
			} else if ( !phaseOne && templateIndex > i ) {
				if ( failures [ i ] ) {
					text = "<color=#f56e6eff>" + text;
				} else {
					text = "<color=#2b2b2bff>" + text;
				}
			}
			str = str + text;
		}
		return str;
	}

	private string GetEndingMessage ( float score ) {
		string str = "<color=#2b2b2bff>Score: ";
		str += didPlayerWin ? "<color=#6bb9f0ff>" : "<color=#f56e6eff>";
		str += Mathf.FloorToInt ( score * 100f ).ToString ( ) + "%";
		str += "<color=#2b2b2bff>\nNeeded: " + Mathf.FloorToInt ( scoreNeeded * 100f ).ToString ( ) + "%";
		if ( didPlayerWin ) {
			str += "\n\nYou won!\nYou sacrificially roasted " + template.opponentName + ".";
		} else {
			str += "\n\nYou lost!\nNow " + template.opponentName + " gets to eat you.";
		}
		return str;
	}
}
