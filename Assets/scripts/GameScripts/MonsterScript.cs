using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class MonsterScript : MonoBehaviour {
	AudioSource source;
	float minDistPlaySound = 7f;

	float moveSpeed = 1;
	float startPosX;
	float distance = .15f;
	void Start() {
		source = GetComponent<AudioSource>();
		startPosX = transform.position.x;
		}
	void Update() {
		float soundDistance = Vector3.Distance(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0), transform.position);
		if (soundDistance <= minDistPlaySound && !source.isPlaying && Data.SOUND)
			source.Play();

		float x = transform.position.x + (moveSpeed * Time.deltaTime);

		if (x > startPosX + distance)
			moveSpeed *= -1;
		else if (x < startPosX - distance)
			moveSpeed *= -1;

		transform.position = new Vector3(x, transform.position.y);
		}
	}