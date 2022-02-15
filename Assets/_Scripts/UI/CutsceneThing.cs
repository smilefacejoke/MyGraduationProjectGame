using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneThing : MonoBehaviour {
	public float 				speed;
	public Conversationist	 	con;
	public int 					scene;

	private bool 				nextMenu;

	private void Start ( ) {
		con.Talk ( );
	}

	private void Update ( ) {
		transform.position += Vector3.forward * speed * Time.deltaTime;
		if (Input.GetKeyDown ( KeyCode.Space ) ) { 
			if ( nextMenu ) {
				SceneManager.LoadScene ( scene );
			}
			if ( con.Talk ( ) ) {
				nextMenu = true;
			}
		}
	}
}