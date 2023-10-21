using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    [SerializeField] public bool FREEZE = false;
    private enum POSITION { LLLEFT = -3, LLEFT, LEFT, CENTER, RIGHT, RRIGHT, RRRIGHT }

    [SerializeField] private GameObject platformPrefab, springPrefab, destroyPlatformPrefab, hidePlatformPrefab;
    [SerializeField] private GameObject movedPlatformPrefab, movedPlatform2Prefab, movedSpringPlatformPrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject monster1Prefab;
    [SerializeField] private GameObject itemShieldPrefab, itemHelmetPrefab, itemSpringPrefab;
    [SerializeField] private Transform obstaclesContainer, itemsContainer;
    [SerializeField] private Transform startPos;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject GamePanel, PausePanel;

    [SerializeField] private PlayerScript player;

    private POSITION position = POSITION.CENTER;
    private Vector3 lastPosition;

    private Quaternion calibrateAccel;
    [SerializeField] private Text ScopeText;
    private float scope = 0;

    [SerializeField] private float cameraSpeed = 3;

    private bool cameraMoved = false;
    private float cameraYAxis = 0;

    private void Awake() {
        if (!Data.ACCESS)
            ToMenu();
        }

    private void Start() {
        Data.RUNNING = true;
        changeScopeText(scope);
        calibrateAccel = _calibrateAccel();
        lastPosition = startPos.position;
        _addObstacle(platformPrefab);
        SpawnObstacle(-1);
        }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("GameScene");
        if (cameraMoved) {
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + cameraSpeed * Time.deltaTime, cam.transform.position.z);
            if (cam.transform.position.y >= cameraYAxis) {
                cam.transform.position = new Vector3(cam.transform.position.x, cameraYAxis, cam.transform.position.z);
                cameraMoved = false;
                }
            }
        }
    public void SpawnObstacle(int type = 0) {
        type = (type == 0) ? Random.Range(0, 8) : ((type == -1) ? 0 : type);

        if (type == 0) {
            int counter = Random.Range(40, 101);

            int chance_plat = Random.Range(2, 6);
            int chance_desplat = Random.Range(7, 10);
            int chance_hideplat = Random.Range(4, 7);

            int j = 0;

            for (int i = 0; i < counter; i++, j++) {
                _changePos(.2f, true);
                if (Random.Range(0, chance_plat) == 0 || j >= 5) {
                    j = 0;
                    if (Random.Range(0, 3) == 0)
                        _changePosX();
                    _changePos(.2f);
                    _addObstacle(platformPrefab);
                    if (Random.Range(0, 100) == 0) {
                        GameObject tmp = Instantiate(itemSpringPrefab, new Vector3(lastPosition.x, lastPosition.y + .3f, lastPosition.z), Quaternion.identity) as GameObject;
                        tmp.transform.SetParent(itemsContainer);
                        }
                    }
                else if (Random.Range(0, chance_desplat) == 0) {
                    j++;
                    _changePosX();
                    _addObstacle(destroyPlatformPrefab);
                    }
                else if (Random.Range(0, chance_hideplat) == 0) {
                    _changePos(.2f);
                    _addObstacle(hidePlatformPrefab);
                    }
                }
            if (j >= 5)
                _changePos(-.2f, true);
            }
        else if (type == 4) {
            int counter = Random.Range(160, 401);

            int chance_plat = Random.Range(2, 51);
            int chance_desplat = Random.Range(7, 10);
            int chance_hideplat = Random.Range(5, 7);

            for (int i = 0, j = 0; i < counter; i++, j++) {
                if (Random.Range(0, chance_plat) == 0 || j >= 4) {
                    j = 0;
                    _changePos(.2f);
                    _addObstacle(platformPrefab);
                    if (Random.Range(0, 20) == 0) {
                        GameObject tmp = Instantiate(itemSpringPrefab, new Vector3(lastPosition.x, lastPosition.y + .3f, lastPosition.z), Quaternion.identity) as GameObject;
                        tmp.transform.SetParent(itemsContainer);
                        }
                    }
                else if (Random.Range(0, chance_desplat) == 0) {
                    _changePos(.2f);
                    _addObstacle(destroyPlatformPrefab);
                    }
                else if (Random.Range(0, chance_hideplat) == 0) {
                    _changePos(.2f);
                    _addObstacle(hidePlatformPrefab);
                    }
                }
            }
        else if (type == 5) {
            int counter = Random.Range(40, 101);
            int chance_plat = Random.Range(2, 6);
            int j = 0;
            for (int i = 0; i < counter; i++, j++) {
                _changePos(.2f, true);
                if (Random.Range(0, chance_plat) == 0 || j >= 5) {
                    j = 0;
                    _changePosX();
                    _addObstacle(platformPrefab);
                    }
                }
            if (j >= 5)
                _changePos(-.2f, true);
            }
        else if (type == 1) {
            int counter = Random.Range(20, 41);
            for (int i = 0; i < counter; i++) {
                if (Random.Range(0, 3) == 0)
                    _changePosX();
                _changePos(1f);
                _addObstacle(Random.Range(0, 11) == 0 ? movedSpringPlatformPrefab : springPrefab);
                if (Random.Range(0, 3) == 0)
                    _changePosX();
                _changePos(1.6f);
                if (Random.Range(0, 11) == 0)
                    _addObstacle(Random.Range(0, 2) == 0 ? movedPlatform2Prefab : movedPlatformPrefab);
                else
                    _addObstacle(Random.Range(0, 4) == 0 ? hidePlatformPrefab : platformPrefab);
                }
            }
        else if (type == 2) {
            int counter = Random.Range(5, 11);
            for (int i = 0, j = 0; i < counter; i++, j++) {
                if (Random.Range(0, 2) == 0 || j >= 2) {
                    j = 0;
                    _changePos(.5f);
                    _addObstacle((Random.Range(0, 3) == 0) ? movedSpringPlatformPrefab : movedPlatformPrefab);
                    }
                else if (Random.Range(0, 3) == 0) {
                    _changePos(.5f);
                    GameObject obstacle = _addObstacle(destroyPlatformPrefab);
                    obstacle.AddComponent<PlatformMovedScript>();
                    obstacle.GetComponent<PlatformMovedScript>().ChangeDirection(PlatformMovedScript.DIRECTION.HORIZONTAL);
                    }
                else if (Random.Range(0, 3) == 0) {
                    _changePos(.5f);
                    GameObject obstacle = _addObstacle(hidePlatformPrefab);
                    obstacle.AddComponent<PlatformMovedScript>();
                    obstacle.GetComponent<PlatformMovedScript>().ChangeDirection(PlatformMovedScript.DIRECTION.HORIZONTAL);
                    }
                }
            }
        else if (type == 3) {
            int counter = Random.Range(5, 10);
            for (int i = 0; i < counter; i++) {
                if (Random.Range(0, 3) == 0)
                    _changePosX();
                _changePos(1f);
                _addObstacle(platformPrefab);
                if (Random.Range(0, 21) == 0) {
                    GameObject tmp = Instantiate(itemShieldPrefab, new Vector3(lastPosition.x, lastPosition.y + .3f, lastPosition.z), Quaternion.identity) as GameObject;
                    tmp.transform.SetParent(itemsContainer);
                    }
                }
            _changePos(1f);
            _addObstacle(monster1Prefab);
            _changePos(.5f);
            _addObstacle(platformPrefab);
            }
        else if (type == 6) {
            int counter = Random.Range(1, 6);
            for (int i = 0; i < counter; i++) {
                if (Random.Range(0, 3) == 0)
                    _changePosX();
                _changePos(1f);
                _addObstacle(movedPlatform2Prefab);
                }
            }
        else if (type == 7) {
            int counter = Random.Range(10, 21);
            for (int i = 0; i < counter; i++) {
                if (Random.Range(0, 2) == 0)
                    _changePosX();
                _changePos(Random.Range(0, 3) == 0 ? 1f : .5f);
                _addObstacle(hidePlatformPrefab);
                }
            }

        _changePos(1f);
        _addObstacle(platformPrefab, true);
        }
    public void GameOver() {
        SceneManager.LoadScene("GameScene");
        }
    private GameObject _addObstacle(GameObject obstacle, bool spawner = false) {
        if (Random.Range(0, 100) == 0) {
            GameObject tmp1 = Instantiate(itemHelmetPrefab, new Vector3(lastPosition.x, lastPosition.y + .3f, lastPosition.z), Quaternion.identity) as GameObject;
            tmp1.transform.SetParent(itemsContainer);
            }
        GameObject tmp = Instantiate(obstacle, lastPosition, Quaternion.identity) as GameObject;
        tmp.transform.SetParent(obstaclesContainer);
        if (spawner)
            tmp.tag = "SpawnerPlatform";
        return tmp;
        }
    private void _changeNewPosition() {
        int pos = Random.Range(0, 2) == 0 ? (int)position - 1 : (int)position + 1;

        if (pos < (int)POSITION.LLLEFT)
            pos = (int)POSITION.RRRIGHT;
        if (pos > (int)POSITION.RRRIGHT)
            pos = (int)POSITION.LLLEFT;

        if ((int)position == pos) {
            if (pos == (int)POSITION.LLLEFT)
                pos = (int)position + 1;
            else if (pos == (int)POSITION.RRRIGHT)
                pos = (int)position - 1;
            }

        position = (POSITION)pos;
        }
    private void _changePosX() {
        _changeNewPosition();

        float pos = ((int)position * .6f) + (Random.Range(-2, 2) * .1f);

        if (pos <= (int)POSITION.LLLEFT * .6f)
            pos = (int)POSITION.LLLEFT * .6f;

        if (pos >= (int)POSITION.RRRIGHT * .6f)
            pos = (int)POSITION.RRRIGHT * .6f;

        lastPosition.x = pos;
        }
    private void _changePos(float y, bool onlyY = false) {
        lastPosition.y += y;
        if (onlyY)
            return;
        _changePosX();
        }
    private Quaternion _calibrateAccel() {
        Quaternion rotate = Quaternion.FromToRotation(new Vector3(0, 0, -1f), Input.acceleration);
        return Quaternion.Inverse(rotate);
        }
    public Vector3 FixAccel(Vector3 acceleration) {
        return calibrateAccel * acceleration;
        }
    public void changeScopeText(float scope) {
        this.scope = scope;
        float result = (float)System.Math.Round(this.scope, 2);
        ScopeText.text = result + " m";
        }
    public float GetScope() {
        return scope;
        }
    public void MoveCamera(float pos, bool chase = false) {
        if (chase) {
            cameraMoved = false;
            cam.transform.position = new Vector3(cam.transform.position.x, pos, cam.transform.position.z);
            return;
            }
        cameraMoved = true;
        cameraYAxis = pos;
        }
    public void GamePause() {
        Data.RUNNING = false;
        GamePanel.SetActive(false);
        PausePanel.SetActive(true);
        }
    public void GameResume() {
        Data.RUNNING = true;
        GamePanel.SetActive(true);
        PausePanel.SetActive(false);
        }
    public void ToMenu() {
        SceneManager.LoadScene("MenuScene");
        }
    public void Shoot() {
        player.Fire();
        }

    }