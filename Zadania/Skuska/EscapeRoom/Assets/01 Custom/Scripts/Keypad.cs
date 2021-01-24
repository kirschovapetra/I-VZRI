using System;
using UnityEngine;
using TMPro;

// zadavanie kodu na keypade
public class Keypad : MonoBehaviour {
    public GameManager gameManager;
    public TextMeshProUGUI keypadText;
    
    private AudioSource[] sounds;
    private void Start() {
        keypadText.text = "";
        keypadText.color = Color.white;
        
        // zelene tlacitko ma 2 zvuky: spravny/nespravny
        if (transform.name=="Green")
            sounds = GetComponents<AudioSource>();
    }

    private void OnMouseDown() {
        // pri pauze hry sa nic nedeje
        if (GameManager.paused) return;

        PressKey();
        
        switch (gameObject.name) {
            case "Green":
                Check(); // vyhodnotenie
                break;
            case "Red":
                Clear(); // zmazanie displeja
                break;
            default:
                keypadText.text += gameObject.name; // pridanie noveho cisla
                break;
        }
    }

    private void Update() {

        // ked prekroci pocet znakov -> nespravny kod, displej sa vymaze
        if (keypadText.text.Length>6) {
            keypadText.color = Color.red;
            Invoke("Clear",0.5f);
        }
    }

    // stlacenie tlacidla
    private void PressKey(){
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetTrigger("Push");
    }

    // vymazanie displeja
    private void Clear(){
        keypadText.text = "";
        keypadText.color = Color.white;
    }
    
    // vyhodnotenie kodu
    private void Check(){
        bool result = keypadText.text.Equals("4703");
        
        // set premennej correctKeypadCode v GameManageri, ci je kod spravny/nespravny
        gameManager.correctKeypadCode = result;    
        
        // zmena farby displeja, zvukove efekty
        if (result) {
            sounds[1].Play();
            keypadText.color = Color.green;
        } else {
            sounds[0].Play();
            keypadText.color = Color.red;
            Invoke(nameof(Clear),0.5f);

        }
    }
    
    
}