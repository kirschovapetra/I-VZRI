/************************** Menu *****************************/

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    
    public static bool menuVisible;
    public bool mainMenu; // T/F podla toho, v akej scene sa nachadzam
    public GameObject menuCanvas;
    public GameObject settingsPanel; 
    [Header("Kurzory")]
    public Texture2D cursorSelect;
    public Texture2D cursorPointer;

    // audio
    private AudioManager audioManager;
    private Slider musicSlider;
    private Slider effectsSlider;
    // % hlasitosti
    private TextMeshProUGUI musicPercent;
    private TextMeshProUGUI effectsPercent;
    // screen resolution
    private Toggle fullScreenToggle;
    private TMP_Dropdown resolutionDropdown;
    private bool changed;
    // fade script
    private Fade fade;
    
    void Start() {
        audioManager = GetComponent<AudioManager>();
        fade = GetComponent<Fade>();
        changed = false;
        menuVisible = false;
        
        // slidery
        musicSlider = settingsPanel.transform.Find("MusicSlider").GetComponent<Slider>();
        effectsSlider = settingsPanel.transform.Find("EffectsSlider").GetComponent<Slider>();
        
        // percenta - text
        musicPercent = settingsPanel.transform.Find("MusicPercentText").GetComponent<TextMeshProUGUI>();
        effectsPercent = settingsPanel.transform.Find("EffectsPercentText").GetComponent<TextMeshProUGUI>();
      
        // screen resolution
        fullScreenToggle = settingsPanel.transform.Find("FullScreenToggle").GetComponent<Toggle>();
        resolutionDropdown = settingsPanel.transform.Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();

        // kurzor = pointer, pohybuje sa po celej obrazovke
        if (mainMenu) SetCursorConfined();

        // nastavenie hlasitosti a dropdownu na hodnoty ulozene v PlayerPrefs
        fullScreenToggle.isOn = PlayerPrefs.GetInt("toggleIsOn",1) == 1;
        resolutionDropdown.value = PlayerPrefs.GetInt("dropdownValue",0);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume",audioManager.defaultVolume);
        effectsSlider.value = PlayerPrefs.GetFloat("effectsVolume",audioManager.defaultVolume);
        audioManager.SetVolume();

        // pri spusteni novej hry sa resetuju nastavenia
        if (NewGameCheck.newGameStarted) {
            ResetSettings();
            NewGameCheck.newGameStarted = false;
        }
    }

    
    void Update() {

        // percento podla vysky hlasitosti
        musicPercent.SetText(Math.Round(musicSlider.value*100.0f) + "%");
        effectsPercent.SetText(Math.Round(effectsSlider.value*100.0f) + "%");
        
        // prepnutie full screen / window mode
        if(fullScreenToggle.isOn) 
            SetFullScreen();
        else {
            SetWindowedScreen();
            if (!changed) {
                ChangeResolution();
                changed = true;
            }
        }

        // zobrazenie/skrytie menu po stlaceni 'Esc' iba pocas hry
        if (!mainMenu && Input.GetKeyDown(KeyCode.Escape) && !Inventory.inventoryVisible) 
            ToggleMenuOverlay();
        
    }
    
    /************************************ PRIVATE *************************************/
    
    
    // zosvetlenie a zoslabenie farieb Dropdownu
    private void ChangeDropdownVisibility(bool isEnabled) {
        resolutionDropdown.enabled = isEnabled;
        
        // text, sipka na dropdowne
        var label = resolutionDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>();
        var arrow = resolutionDropdown.transform.Find("Arrow").GetComponent<Image>();
        
        // ked je dropdown enabled, ma silnejsie farby a naopak
        if (isEnabled) {
            label.alpha = 1.0f;
            arrow.color = Color.white;
        } else {
            label.alpha = 0.2f;
            arrow.color = new Color(0.34f, 0.34f, 0.34f);
        }
    }
    
    // full screen
    private void SetFullScreen() {
        Screen.SetResolution(1920, 1080, true);
        
        fullScreenToggle.isOn = true;
        PlayerPrefs.SetInt("toggleIsOn", 1);
        
        resolutionDropdown.value = 0;
        PlayerPrefs.SetInt("dropdownValue", 0);

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        
        ChangeDropdownVisibility(false);

        changed = false;
    }

    
    // okno
    private void SetWindowedScreen() {
        fullScreenToggle.isOn = false;
        PlayerPrefs.SetInt("toggleIsOn", 0);

        Screen.fullScreenMode = FullScreenMode.Windowed;
        
        ChangeDropdownVisibility(true);
    }

    /************************************ PUBLIC *************************************/

    // zobrazenie/skrytie menu (maleho okna v hre)
    public void ToggleMenuOverlay() {
        // default - menu visible, ostatne hidden
        menuCanvas.transform.Find("MenuPanel").gameObject.SetActive(true);
        menuCanvas.transform.Find("SettingsPanel").gameObject.SetActive(false);
        menuCanvas.transform.Find("TutorialPanel").gameObject.SetActive(false);
        
        menuVisible = !GameManager.exitingToMenu && !menuCanvas.activeSelf;
        
        // hra je pauznuta, ked sa zobrazuje menu
        if (menuVisible) 
            GameManager.PauseGame();
        else 
            GameManager.ResumeGame();
        
        menuCanvas.SetActive(menuVisible);
    }

    // spustenie hry
    public void StartGame() { StartCoroutine(fade.FadeOutWithMessages()); }

    // ukoncenie hry
    public void Exit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit ();
#endif
    }
    
    // hlavne menu
    public void ExitToMainMenu() {
        fade.FadeOut();
        StartCoroutine(GameManager.WaitAndLoadScene("Main Menu", 2.5f));
        ToggleMenuOverlay();
    }
    
    //nastavenie hlasitosti hudby podla polohy slidera
    public void UpdateVolume() {
        // zakliknuty slider
        GameObject slider = EventSystem.current.currentSelectedGameObject;

        if (slider == null) return;
        
        if (slider.name.Equals("MusicSlider")) {
            PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        }
        else if (slider.name.Equals("EffectsSlider")) {
            PlayerPrefs.SetFloat("effectsVolume", effectsSlider.value);
        }
        
        // set hlasitosti v PlayerPrefs a mixeri
        audioManager.SetVolume();
    }

    // default nastavenia
    public void ResetSettings() {
        SetFullScreen();

        PlayerPrefs.SetFloat("musicVolume", audioManager.defaultVolume);
        musicSlider.value = audioManager.defaultVolume;
   
        PlayerPrefs.SetFloat("effectsVolume", audioManager.defaultVolume);
        effectsSlider.value = audioManager.defaultVolume;
        
        audioManager.SetVolume();
    }

    // zmena rozlisenia obrazovky
    public void ChangeResolution() {
        // resolution podla zakliknutej option z dropdownu
        var resolution = resolutionDropdown.options[resolutionDropdown.value];
        string[] splitString = resolution.text.Split('x');
        
        Screen.SetResolution(Int32.Parse(splitString[0]), Int32.Parse(splitString[1]), false);
        PlayerPrefs.SetInt("dropdownValue", resolutionDropdown.value);
        
    }

    public void SetCursorConfined() {
        // kurzorom sa da hybat, ikonka = pointer
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.SetCursor(cursorPointer, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
    }

    public void SetCursorLocked(RaycastHit hit) {
        // kurzor je locknuty v strede obrazovky
        Cursor.lockState = CursorLockMode.Locked;
        // zmena ikonky kurzora podla typu objektu        
        if (hit.transform.CompareTag("Interactable") || hit.transform.CompareTag("Collectable"))
            Cursor.SetCursor(cursorSelect, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(cursorPointer, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
    }
    
}
