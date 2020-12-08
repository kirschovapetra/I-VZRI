using UnityEngine;
using UnityEngine.UI;

// menu
public class UIManager : MonoBehaviour {
    // kamera
    public GameObject mainCamera;
    // tlacidla na zapnutie/vypnutie hudby + bubliny s hintmi
    public GameObject musicOnButton;
    public GameObject musicOnBubble;
    public GameObject musicOffButton;
    public GameObject musicOffBubble;
    // napoveda - bublina s hintom, okno s popisom ovladania
    public GameObject helpBubble;
    public GameObject helpPanel;
    // manualna rotacia - tlacidlo + bublina s hintom
    public Button manualRotationButton;
    public GameObject manualRotationBubble;
    // automaticka rotacia - tlacidlo + bublina s hintom
    public Button automaticRotationButton;
    public GameObject automaticRotationBubble;
    
    private AudioSource audioSrc;
    private CameraController camController;    // script na ovladanie kamery
    
    private void Start() {
        // inicializacia
        audioSrc = mainCamera.GetComponent<AudioSource>();
        camController = mainCamera.GetComponent<CameraController>();
        
        // na zaciatku je selectnuta manualna rotacia
        manualRotationButton.Select();
        manualRotationButton.interactable=false;
    }

    // ************************ funkcie vyvolane stlacenim UI tlacidiel ************************

    public void Exit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit ();
#endif
    
    }
    
    // zobrazenie hintov v bublinach
    public void ShowAutomaticRotationBubble() { Show(automaticRotationBubble); }
    public void HideAutomaticRotationBubble() { Hide(automaticRotationBubble); }
    
    public void ShowManualRotationBubble() { Show(manualRotationBubble); }
    public void HideManualRotationBubble() { Hide(manualRotationBubble); }
    
    public void ShowMusicOnBubble() { Show(musicOnBubble); }
    public void HideMusicOnBubble() { Hide(musicOnBubble); }

    public void ShowMusicOffBubble() { Show(musicOffBubble); }
    public void HideMusicOffBubble() { Hide(musicOffBubble); }

    public void ShowHelpBubble() { Show(helpBubble); }
    public void HideHelpBubble() { Hide(helpBubble); }

    
    // okno s napovedou
    public void ShowHelpPanel() { Show(helpPanel); }
    public void HideHelpPanel() { Hide(helpPanel); }
    

    // zvuk
    public void Mute() {
        ToggleObject(musicOffButton,musicOnButton);
        audioSrc.volume = 0.0f;
    }
    public void Unmute() {
        ToggleObject(musicOnButton,musicOffButton);
        audioSrc.volume = 0.3f;
    }


    // prepinanie automatickej/ manualnej rotacie
    public void AutomaticRotation() {
        manualRotationButton.interactable = true;
        automaticRotationButton.interactable = false;
        camController.rotate = true;
    }

    public void ManualRotation() {
        manualRotationButton.interactable = false;
        automaticRotationButton.interactable = true;
        camController.rotate = false;
    }
    
        
    // ******************* pomocne funkcie na prepinanie zobrazenia objektov *******************
    private void ToggleObject(GameObject visible, GameObject hidden) {
        visible.SetActive(true);
        hidden.SetActive(false);
    }
    private void Show(GameObject obj) { obj.SetActive(true); }
    private void Hide(GameObject obj) { obj.SetActive(false); }

    
}
