using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gameLoseScreen;
    public GameObject gameWinScreen;
    public Text pointsText;
    public Text gameModeText;
    public Button gameModeButton;
    public bool isHardModeOn;
    bool gameStopped;

    void Start() {
        PointPlatform.OnGainPoint += OnGainPoint;
        gameLoseScreen.SetActive(false);
        gameWinScreen.SetActive(false);
        Guard.OnPlayerSpotted += OnGameLose;
        PointPlatform.OnPlayerWin += OnGameWin;
        ToggleGameMode(true);
        gameModeButton.onClick.AddListener(() => ToggleGameMode(!isHardModeOn));
    }

    void Update() {
        if (gameStopped) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                SceneManager.LoadScene(0);
            }
        }
    }

    void ToggleGameMode(bool isHardMode) {
        isHardModeOn = isHardMode;
        if (isHardModeOn) {
            gameModeText.text = "Hard";
            gameModeText.color = Color.red;
        } else {
            gameModeText.text = "Easy";
            gameModeText.color = Color.green;
        }
    }

    void OnGainPoint(PointPlatform p) {
        pointsText.text = PointPlatform.points.ToString();
    }

    void OnGameWin() {
        OnGameOver(gameWinScreen);
    }

    void OnGameLose() {
        if (isHardModeOn) {
            OnGameOver(gameLoseScreen);
        }
    }

    void OnGameOver(GameObject gameOverUI) {
        gameOverUI.SetActive(true);
        gameStopped = true;
        Guard.OnPlayerSpotted -= OnGameLose;
        PointPlatform.OnPlayerWin -= OnGameWin;
        PointPlatform.OnGainPoint -= OnGainPoint;
    }
}
