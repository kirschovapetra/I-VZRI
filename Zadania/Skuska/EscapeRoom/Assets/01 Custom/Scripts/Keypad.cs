using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Keypad : MonoBehaviour {
    public GameManager gameManager;
    public TextMeshProUGUI keypadText;

    private void Start() {
        keypadText.text = "";
        keypadText.color = Color.white;
    }

    private void OnMouseDown() {
        PressKey();
        switch (gameObject.name) {
            case "Green":
                Check();
                break;
            case "Red":
                Clear();
                break;
            default:
                AddNumber();
                break;
        }
    }

    private void Update() {
        if (keypadText.text.Length>6) {
            // TODO bad sound
            keypadText.color = Color.red;
            Invoke("Clear",0.5f);
        }
    }

    private void PressKey(){
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetTrigger("Push");
    }

    private void AddNumber() {
        keypadText.text += gameObject.name;
    }
    
    private void Clear(){
        keypadText.text = "";
        keypadText.color = Color.white;
    }
    private void Check(){
        Boolean result = keypadText.text.Equals("4703");
        gameManager.correctKeypadCode = result;
        
        if (result) {
            // TODO good sound
            keypadText.color = Color.green;
        } else {
            // TODO bad sound
            keypadText.color = Color.red;
            Invoke("Clear",0.5f);

        }
    }
    
    
}