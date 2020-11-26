using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// management hry
public class GameManager : MonoBehaviour {
    // stav hry / hraca
    public bool gameOver; 
    public bool finished;
    public bool win;
    public bool hit;
    
    // score, pocet zivotov
    public int score = 0;
    public int lives = 3;
    
    // UI
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public Image[] livesDisplay;
    
    
    // particle system v zavere hry
    public ParticleSystem finishParticleSystem;
    
    // animacia otvorenia truhlice
    public Animator chestAnimator;
    
    // stav, ci bola/nebola otvorena truhlica
    private bool opened = false;
    
    void Start() {
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        scoreText.text = "Score: " + score + "/50";
    }

    void Update() {
        scoreText.text = "Score: " + score + "/50"; 

        // hrac narazil do prisery => odpocitaju sa zivoty
        if (hit) {
            lives--;
            hit = false;
            SetLivesDisplay();
            gameOver = (lives <= 0) ? true : gameOver;
        }

        // game over => spadol do jamy alebo prisiel o vsetky zivoty
        if (gameOver) {
            gameOverPanel.SetActive(true);
            Invoke(nameof(ReloadScene), 1.0f);
        }

        // vyhra => zozbieral 50 coinov a presiel triggerom zaverecnej casti sceny
        if (score >= 50 && finished) {
            // otvorenie truhlice s pokladom
            if (!opened) {
                finishParticleSystem.Play();
                chestAnimator.SetTrigger("open");
                opened = true;
            }
        }

        // win => hrac vzal poklad z truhlice
        if (win) 
            winPanel.SetActive(true);
    }
    
    public void ReloadScene() { SceneManager.LoadScene("Game"); }
    
    // zobrazenie zivotov na canvas
    public void SetLivesDisplay() {
        switch (lives) {
            case 0:
                livesDisplay[0].enabled = false;
                livesDisplay[1].enabled = false;
                livesDisplay[2].enabled = false;
                break;
            case 1:
                livesDisplay[0].enabled = false;
                livesDisplay[1].enabled = false;
                break;
            case 2:
                livesDisplay[0].enabled = false;
                break;
            default:
                livesDisplay[0].enabled = true;
                livesDisplay[1].enabled = true;
                livesDisplay[2].enabled = true;
                break;
        }
    }

}