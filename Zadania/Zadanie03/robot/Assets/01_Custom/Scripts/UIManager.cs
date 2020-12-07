using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject automaticRotationBubble;
    public GameObject manualRotationBubble;
    public GameObject musicOnBubble;
    public GameObject musicOffBubble;
    public GameObject musicOffButton;
    public GameObject musicOnButton;
    public Button manualMovementButton;
    public AudioSource audioSrc;


    private void Start() {
        manualMovementButton.Select();
        manualMovementButton.interactable = false;
    }

    private void ToggleObject(GameObject visible, GameObject hidden) {
        visible.SetActive(true);
        hidden.SetActive(false);
    }
    
    private void Show(GameObject obj) { obj.SetActive(true); }

    private void Hide(GameObject obj) { obj.SetActive(false); }

    public void ShowAutomaticRotationBubble() { Show(automaticRotationBubble); }
    public void HideAutomaticRotationBubble() { Hide(automaticRotationBubble); }
    
    public void ShowManualRotationBubble() { Show(manualRotationBubble); }
    public void HideManualRotationBubble() { Hide(manualRotationBubble); }

    
    public void ShowMusicOnBubble() { Show(musicOnBubble); }
    public void HideMusicOnBubble() { Hide(musicOnBubble); }

    public void ShowMusicOffBubble() { Show(musicOffBubble); }
    public void HideMusicOffBubble() { Hide(musicOffBubble); }
    
    public void Mute() {
        ToggleObject(musicOffButton,musicOnButton);
        audioSrc.volume = 0.0f;
    }

    public void Unmute() {
        ToggleObject(musicOnButton,musicOffButton);
        audioSrc.volume = 0.3f;
    }
    
}
