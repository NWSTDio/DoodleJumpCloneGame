using System.Collections;
using UnityEngine;

public class BallMovedScript : MonoBehaviour {

	[SerializeField] float speed = 140f;
	[SerializeField] float destroyTime = 3f;

	private void Start() {
		StartCoroutine(Destroyer());
		}
	private void Update() {
		transform.Translate(speed * Time.deltaTime * Vector2.right);
		}
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag.Equals("Monster")) {
			Destroy(collision.gameObject);
			Destroy(gameObject);
			}
		}

	private IEnumerator Destroyer() {
		yield return new WaitForSeconds(destroyTime);

		Destroy(gameObject);
		}

	}