using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversationist : MonoBehaviour {
	public ChatData[] chatData;

	public void Talk ( int index ) {
		TextBoxFactory.main.BuildText ( chatData [ index ] );
	}

	public bool Talk ( ) {
		TextBoxFactory.main.BuildText ( chatData [ index ] );
		index++;
		if ( index >= chatData.Length ) {
			return true;
		} 	
		return false;
	}

	private int index;
}
[System.Serializable]
public struct ChatData {
	public string text;
	public Transform parent;
}