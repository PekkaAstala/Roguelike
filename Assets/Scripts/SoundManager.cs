using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public enum GameAudioEvent {
		Eat, Drink, Walk, EnemyAttack, PlayerAttack, GameOver
	}

	public AudioClip[] eatSounds;
	public AudioClip[] drinkSounds;
	public AudioClip[] walkSounds;
	public AudioClip[] enemyAttackSounds;
	public AudioClip[] playerAttackSounds;
	public AudioClip[] gameOverSounds;

	public AudioSource efxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;

	public float lowPitchRange = 0.95f;
	public float highPitchRange = 1.05f;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public void PlaySound(GameAudioEvent eventType) {
		switch (eventType) 
		{
			case GameAudioEvent.Eat:
			{
				RandomizeSfx (eatSounds);
				return;
			}
			case GameAudioEvent.Drink:
			{
				RandomizeSfx (drinkSounds);
				return;
			}
			case GameAudioEvent.Walk:
			{
				RandomizeSfx (walkSounds);
				return;
			}
			case GameAudioEvent.EnemyAttack:
			{
				RandomizeSfx (enemyAttackSounds);
				return;
			}
			case GameAudioEvent.PlayerAttack:
			{
				RandomizeSfx (playerAttackSounds);
				return;
			}
			case GameAudioEvent.GameOver:
			{
				RandomizeSfx (gameOverSounds);
				return;
			}
		}
	}
	
	public void PlaySingle(AudioClip clip) {
		efxSource.clip = clip;
		efxSource.Play();
	}

	private void RandomizeSfx (params AudioClip [] clips) {
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		efxSource.pitch = randomPitch;
		efxSource.clip = clips[randomIndex];
		efxSource.Play();
	}

}
