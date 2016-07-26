using UnityEngine;
using System.Collections;

public class InnerWall : MonoBehaviour {

	public Sprite dmgSprite;
	public int hp = 4;

	private SpriteRenderer spriteRenderer;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void DamageWall (int loss) {
		SoundManager.instance.PlaySound (SoundManager.GameAudioEvent.PlayerAttack);
		spriteRenderer.sprite = dmgSprite;
		hp -= loss;
		if (hp <= 0) {
			gameObject.SetActive(false);
		}
	}

}
