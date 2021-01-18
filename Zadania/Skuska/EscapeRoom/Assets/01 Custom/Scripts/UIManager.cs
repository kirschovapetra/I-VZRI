using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// GUI
public class UIManager : MonoBehaviour {
    
    public Boolean mainMenu;
    public GameObject menuCanvas;
    public GameObject settingsPanel;
    [Header("Kurzory")]
    public Texture2D cursorSelect;       
    public Texture2D cursorPointer;
    
    public static Boolean menuVisible = false;
    
    // audio
    private AudioManager audioManager;
    private Slider musicSlider;
    private Slider effectsSlider;

    private TextMeshProUGUI musicPercent;
    private TextMeshProUGUI effectsPercent;
    
    // screen resolution
    private Toggle fullScreenToggle;
    private TMP_Dropdown resolutionDropdown;
    private Boolean changed;

    
    void Start() {
        Screen.SetResolution(1920, 1080, true);
        // fullScreenToggle.isOn = true;
        audioManager = GetComponent<AudioManager>();

        // slidery
        musicSlider = settingsPanel.transform.Find("MusicSlider").GetComponent<Slider>();
        effectsSlider = settingsPanel.transform.Find("EffectsSlider").GetComponent<Slider>();
        // percenta - text
        musicPercent = settingsPanel.transform.Find("MusicPercentText").GetComponent<TextMeshProUGUI>();
        effectsPercent = settingsPanel.transform.Find("EffectsPercentText").GetComponent<TextMeshProUGUI>();
      
        // screen resolution
        fullScreenToggle = settingsPanel.transform.Find("FullScreenToggle").GetComponent<Toggle>();
        resolutionDropdown = settingsPanel.transform.Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();
        
        if (mainMenu) {
            // kurzor = pointer, pohybuje sa po celej obrazovke
            SetCursorConfined();
        } 
        
        // nastavenie hlasitosti a dropdownu na hodnoty ulozene v PlayerPrefs
        resolutionDropdown.value = PlayerPrefs.GetInt("dropdownValue", 0);
        fullScreenToggle.isOn = PlayerPrefs.GetInt("toggleIsOn",1) == 1;
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume",audioManager.defaultVolume);
        effectsSlider.value = PlayerPrefs.GetFloat("effectsVolume",audioManager.defaultVolume);
        audioManager.SetVolume();
        

        
    }

    private void ChangeDropdownVisibility(Boolean interactable) {
        resolutionDropdown.enabled = interactable;
        
        // ked je dropdown enabled, ma silnejsie farby a naopak
        if (interactable) {
            resolutionDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>().alpha = 1.0f;
            resolutionDropdown.transform.Find("Arrow").GetComponent<Image>().color = Color.white;
        }
        else {
            resolutionDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>().alpha = 0.2f;
            resolutionDropdown.transform.Find("Arrow").GetComponent<Image>().color = new Color(0.34f, 0.34f, 0.34f);
        }
    }
    
    
    private void Update() {
        
        musicPercent.SetText(
            Math.Round(musicSlider.value*100.0f) + "%");
        effectsPercent.SetText(
            Math.Round(effectsSlider.value*100.0f) + "%");
        
        // prepnutie full screen / windowed mode, visibility dropdownu
        if(fullScreenToggle.isOn) {
            PlayerPrefs.SetInt("toggleIsOn", 1);
            ChangeDropdownVisibility(false);
            PlayerPrefs.SetInt("dropdownValue", 6);
            resolutionDropdown.value = 6;
            Screen.SetResolution(1920, 1080, true);
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        } else {
            PlayerPrefs.SetInt("toggleIsOn", 0);
            ChangeDropdownVisibility(true);
            Screen.fullScreenMode = FullScreenMode.Windowed;
            if (!changed) {
                ChangeResolution();
                changed = true;
            }
        }
        
        // zobrazenie/skrytie menu po stlaceni 'Esc' iba pocas hry
        if (!mainMenu && Input.GetKeyDown(KeyCode.Escape) && !Inventory.inventoryVisible) 
            ToggleMenuOverlay();
        
    }


    // zobrazenie/skrytie menu (maleho okna)
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
    public void StartGame() {
        GetComponent<Fade>().FadeOut();
        StartCoroutine(GameManager.WaitAndLoadScene("Escape Room", 2.5f));
    }

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
        GameManager.exitingToMenu = true;
        GetComponent<Fade>().FadeOut();
        StartCoroutine(GameManager.WaitAndLoadScene("Main Menu", 2.5f));
        ToggleMenuOverlay();
    }
    
    
    //nastavenie hlasitosti hudby 
    public void UpdateVolume() {
        GameObject slider = EventSystem.current.currentSelectedGameObject;

        if (slider == null) return;
        
        if (slider.name.Equals("MusicSlider")) {
            PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        }
        else if (slider.name.Equals("EffectsSlider")) {
            PlayerPrefs.SetFloat("effectsVolume", effectsSlider.value);
        }
        
        audioManager.SetVolume();
    }

    // default nastavenia
    public void ResetSettings() {
        
        PlayerPrefs.SetInt("toggleIsOn", 1);
        fullScreenToggle.isOn = true;
        
        PlayerPrefs.SetInt("dropdownValue", 0);
        resolutionDropdown.value = 0;

        PlayerPrefs.SetFloat("musicVolume", audioManager.defaultVolume);
        musicSlider.value = audioManager.defaultVolume;
   
        PlayerPrefs.SetFloat("effectsVolume", audioManager.defaultVolume);
        effectsSlider.value = audioManager.defaultVolume;
        
        audioManager.SetVolume();

        fullScreenToggle.isOn = true;
        
    }

    // zmena rozlisenia obrazovky
    public void ChangeResolution() {
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
        // zmena ikonky kurzora        
        if (hit.transform.CompareTag("Interactable") || hit.transform.CompareTag("Collectable"))
            Cursor.SetCursor(cursorSelect, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(cursorPointer, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
    }
}
