using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour {
	[SerializeField] Sprite[] sprites;
	[SerializeField] Image ButtonImage;
	void Awake() {
		if (!Data.ACCESS)
			BackToMenu();
		}
	void Start() {
		_changeImageSoundButton();
		}
	public void BackToMenu() {
		SceneManager.LoadScene("MenuScene");
		}
	public void ChangeSoundButton() {
		int sounds = PlayerPrefs.GetInt("sounds") == 0 ? 1 : 0;
		Data.SOUND = (sounds == 1) ? true : false;
		PlayerPrefs.SetInt("sounds", sounds);
		_changeImageSoundButton();
		}
	void _changeImageSoundButton() {
		ButtonImage.sprite = sprites[PlayerPrefs.GetInt("sounds", 1)];
		}
	}