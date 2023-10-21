using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    void Start() {
        Data.ACCESS = true;
        Data.SOUND = (PlayerPrefs.GetInt("sounds", 1) == 1) ? true : false;
        }
    public void PlayButton() {
        SceneManager.LoadScene(Application.isMobilePlatform ? "HelpScene" : "GameScene");
        }
    public void ExitGameButton() // ����� �� ����
        {
        Application.Quit();
        }
    public void SettingsButton() {
        SceneManager.LoadScene("SettingsScene");
        }
    public void InfoButton() {
        SceneManager.LoadScene("InfoScene");
        }
    }