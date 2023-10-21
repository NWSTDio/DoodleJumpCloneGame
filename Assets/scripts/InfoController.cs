using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoController : MonoBehaviour {
	void Awake() {
		if (!Data.ACCESS)
			BackToMenu();
		}
	public void BackToMenu() {
		SceneManager.LoadScene("MenuScene");
		}
	}