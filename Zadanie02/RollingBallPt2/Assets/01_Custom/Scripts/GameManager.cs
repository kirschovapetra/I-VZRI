using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public bool gameOver = false;
    public bool finished = false;
    public int score = 0;
    public TextMeshProUGUI countText; 
    public TextMeshProUGUI winText;
    public Animator chestAnimator;
    private bool opened = false;
    public ParticleSystem finishParticleSystem;
    public AudioSource finishAudio;
    
    void Start() {
        countText.text = "Score: " + score;
        winText.text = "";
    }

    // Update is called once per frame
    void Update() {
        countText.text = "Score: " + score;
        
        if (gameOver) {
            SceneManager.LoadScene("Game");
        }

        if (score >= 50 && finished) {
            finishAudio.Play();
            winText.text = "You Win!";
            
            if (!opened) {
                finishParticleSystem.Play();
                chestAnimator.SetTrigger("open");
                opened = true;
            }
        }
        
    }
    

}