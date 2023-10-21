using UnityEngine;

public class SpawnerObjectsScript : MonoBehaviour {
	[SerializeField] bool SPAWN_OFF = false;
	[SerializeField] GameController controller;
	private void OnTriggerEnter2D(Collider2D collision) {
		string tag = collision.gameObject.tag;
		if (tag.Equals("SpawnerPlatform") && !SPAWN_OFF)
			controller.SpawnObstacle();
		}
	}