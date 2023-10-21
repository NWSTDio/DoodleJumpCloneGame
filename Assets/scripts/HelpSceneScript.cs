using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpSceneScript : MonoBehaviour {
	void Awake() {
		if (!Data.ACCESS)
			SceneManager.LoadScene("MenuScene");
		}
	public void PlayClick() {
		SceneManager.LoadScene("GameScene");
		}
	}