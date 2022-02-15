using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleepManager : Singleton < BleepManager > {

	public AudioSource 	source;
	public AudioClip[] 	playerBleeps;
	public AudioClip[] 	enemyBleeps;
	public float 		pitchVariation;

	public void UpdateEnemyBleeps ( AudioClip[] newBleeps ) {
		enemyBleeps = newBleeps;
	}

	public void Bleep ( bool isPlayer ) {
		AudioClip[] bleeps = isPlayer ? playerBleeps : enemyBleeps;
		source.pitch = Random.Range ( 1f / ( 1f + pitchVariation ), 1f + pitchVariation );
		source.PlayOneShot ( bleeps [ Random.Range ( 0, bleeps.Length ) ] );
	}
}