using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public GameObject gameOverScreen;
    public Text secondsSurvived;
    bool gameOver;

    void Start() {
        gameOverScreen.SetActive(false);
        FindObjectOfType<Player>().OnPlayerDeath += OnGameOver;    
    }

    void Update() {
        if(gameOver) {
            if(Input.GetKeyDown(KeyCode.Space)) {
                SceneManager.LoadScene(0);
            }
        }
    }

    void OnGameOver() {
        gameOverScreen.SetActive(true);
        secondsSurvived.text = Mathf.RoundToInt(Time.timeSinceLevelLoad).ToString();
        secondsSurvived.color = Color.Lerp(Color.yellow, Color.green, Difficulty.GetDifficulty());
        gameOver = true;
    }

}
