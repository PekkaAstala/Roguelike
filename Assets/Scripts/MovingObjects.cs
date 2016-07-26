using UnityEngine;
using System.Collections;

public abstract class MovingObjects : MonoBehaviour {

	public float moveTime = 0.1f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rigidBody;

	protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D>();
		rigidBody = GetComponent<Rigidbody2D>();
	}

	protected Transform Move (int xDir, int yDir) {
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2(xDir, yDir);

		boxCollider.enabled = false;
		RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null) {
			StartCoroutine(SmoothMovement (end));
		}

		return hit.transform;
	}

	protected virtual void AttemptMove <T> (int xDir, int yDir) where T : Component {
		Transform obstaclesInWay = Move (xDir, yDir);

		if (obstaclesInWay == null) {
			return;
		}

		T hitComponent = obstaclesInWay.GetComponent<T>();
		if (hitComponent != null) {
			HitObstacle(hitComponent);
		}
	}

	protected abstract void HitObstacle <T> (T component) where T : Component;

	private IEnumerator SmoothMovement(Vector3 end) {
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards (rigidBody.position, end, Time.deltaTime / moveTime);
			rigidBody.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}
}
