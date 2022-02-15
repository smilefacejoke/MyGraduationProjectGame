using UnityEngine;

public class Singleton <T> : MonoBehaviour where T : Singleton<T> {

	public static T main;

	protected virtual void Awake ( ) {
		if ( main != null ) {
			Debug.Log ( "Instance of " + typeof ( T ).ToString ( ) + " already exists!" );
		}
		main = this as T;
	}
}
