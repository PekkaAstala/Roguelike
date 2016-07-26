﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObjects {

	public int wallDamage = 1;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public float restartLevelDelay = 1f;
	public Text foodText;

	private Animator animator;
	private int food;

	protected override void Start () {
		animator = GetComponent<Animator>();

		food = GameManager.instance.playerFoodPoints;

		foodText.text = "Food: " + food;

		base.Start();
	}

	private void OnDisable() {
		GameManager.instance.playerFoodPoints = food;
	}

	void Update () {
		if (!GameManager.instance.playersTurn) {
			return;
		}

		int horizontal = (int) Input.GetAxisRaw("Horizontal");
		int vertical = (int) Input.GetAxisRaw("Vertical");

		if (horizontal != 0 && vertical != 0) {
			vertical = 0; // no diagonal movement
		}

		if (horizontal != 0 || vertical != 0) {
			AttemptMove<InnerWall> (horizontal, vertical);
		}

	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Exit") {
			Invoke("Restart", restartLevelDelay);
			enabled = false;
		} else if (other.tag == "Food") {
			food += pointsPerFood;
			foodText.text = "+" + pointsPerFood + " Food: " + food;
			SoundManager.instance.PlaySound (SoundManager.GameAudioEvent.Eat);
			other.gameObject.SetActive(false);
		} else if (other.tag == "Soda") {
			food += pointsPerSoda;
			foodText.text = "+" + pointsPerSoda + " Food: " + food;
			SoundManager.instance.PlaySound (SoundManager.GameAudioEvent.Drink);
			other.gameObject.SetActive(false);
		} 
	}

	protected override void OnCantMove<T> (T component) {
		InnerWall hitWall = component as InnerWall;
		hitWall.DamageWall(wallDamage);
		animator.SetTrigger("playerChop");
	}

	private void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void LoseFood(int loss) {
		animator.SetTrigger("playerHit");
		food -= loss;
		foodText.text = "-" + loss + " Food: " + food;
		CheckIfGameOver();
	}

	protected override void AttemptMove<T> (int xDir, int yDir) {
		food--;
		foodText.text = "Food: " + food;

		base.AttemptMove<T> (xDir, yDir);

		RaycastHit2D hit;
		if (Move (xDir, yDir, out hit)) {
			SoundManager.instance.PlaySound (SoundManager.GameAudioEvent.Walk);
		}

		CheckIfGameOver();

		GameManager.instance.playersTurn = false;
	}

	private void CheckIfGameOver() {
		if (food <= 0) {
			SoundManager.instance.PlaySound (SoundManager.GameAudioEvent.GameOver);
			SoundManager.instance.musicSource.Stop();
			GameManager.instance.GameOver();
		}
	}

}
