using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class TextBox : MonoBehaviour {

	public static TextBox		main;

	public TextMeshProUGUI 		textField;
	public string 				textToShow;
	public RectTransform 		background;
	public int 					maxWidth = 20;
	public Animator 			anim;
	public bool 				destroy;
	public float 				life = 5f;
	
	private IEnumerator 	coroutine;
	private Transform 		parent;
	private Vector3 		offset;
	private Vector3 		smooth;
	private float 			smoothTime = 0.1f;


	private void Start ( ) {
		textField.text = "";
		if ( main != null ) {
			main.Close ( );
		}
		main = this;

		if  ( textToShow.Length > 0 ) {
			SetText ( textToShow );
		}
	}

	public void SetTextParent ( Transform newParent ) {
		parent = newParent;
		offset = transform.position - parent.position;
	}

	public void SetText ( string text ) {

		textToShow = text;
		text = AddLineBreaks ( text );

		if ( coroutine != null ) {
			StopCoroutine ( coroutine );
		}
		coroutine = ShowText ( text );
		StartCoroutine ( coroutine );
	}

	void Update ( ) {
		if ( parent ) {
			transform.position = Vector3.SmoothDamp ( transform.position, parent.position + offset, ref smooth, smoothTime );
		}
		if ( destroy ) {
			Destroy ( gameObject );
		}
	}

	public void Close ( ) {
		anim.SetBool ( "fade", true );
	}

	IEnumerator ShowText ( string text ) {
		textField.SetText( "" );
		for ( int i = 0; i < text.Length + 1; i++ ) {
			yield return new WaitForSeconds ( 0.01f );
			textField.text = text.Substring ( 0, i ) + "<color=#00000000>" + text.Substring ( i, text.Length - i );
		}
		yield return new WaitForSeconds ( life );
		anim.SetBool ( "fade", true );
	}

	string AddLineBreaks ( string text ) {
		string output = "";
		int lastSpace = -1;
		int currentLength = 0;
		for ( int i = 0; i < text.Length; i++ ) {
			output += text [ i ];
			if ( ( "" + text [ i ] ).Equals ( " " ) ) {
				lastSpace = output.Length - 1;
			}
			currentLength ++;

			if ( currentLength > maxWidth && lastSpace >= 0 ) {
				string newString = output.Remove( lastSpace, 1 );
				newString  = newString.Insert( lastSpace, "\n" );
				output = newString;
				lastSpace = -1;
				currentLength = 0;
			}
		}
		return output;
	}
}