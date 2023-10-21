using UnityEngine;

public class PlayBgSoundScript : MonoBehaviour {
	AudioSource source;
	void Start() {
		if (Data.SOUND) {
			source = GetComponent<AudioSource>();
			source.Play();
			}
		}
	}