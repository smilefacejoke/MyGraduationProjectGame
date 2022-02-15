using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxFactory : Singleton < TextBoxFactory > {
	public GameObject 	prefab;
	public float 		life = 6f;

	public void BuildText ( ChatData chatData ) {
		GameObject go = Instantiate ( prefab, Vector3.zero, Quaternion.identity );
		TextBox tb = go.GetComponent < TextBox > ( );
		tb.transform.position = new Vector3( 0f, 5f, 0f ) + chatData.parent.position;
		tb.SetTextParent ( chatData.parent );
		tb.life = life;
		tb.SetText ( chatData.text );
	}
}