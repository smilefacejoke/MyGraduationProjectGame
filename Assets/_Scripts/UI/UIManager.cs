using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton < UIManager > {
	public Animation 		animation;
	public Image 			enemyImage;
	public TextMeshProUGUI 	enemyText;

	public void PlayIntro ( ) {
		animation.Play();
	}

	public void PlayIntro ( string enemyName, Sprite enemySprite ) {
		SetEnemyName ( enemyName );
		SetEnemySprite ( enemySprite );
		animation.Play ( );
	}

	public void SetEnemyName ( string enemyName ) {
		enemyText.SetText ( enemyName );
	}

	public void SetEnemySprite ( Sprite enemySprite ) {
		enemyImage.sprite = enemySprite;
	}
}