/************************* management hry ****************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [Header("Main kamera")]
    public Camera cam;
    // bool premenne
    [HideInInspector] public bool correctKeypadCode;
    public static bool gameOver;
    public static bool paused;       
    public static bool exitingToMenu;
    private bool alreadyPlayed;
    // raycast
    private Ray ray;
    private RaycastHit hit;
    // spravny kod knih
    private readonly bool[] bookCode = {true, false, true, true, false, false};
    // animatory
    private Animator fuseBoxAnimator;
    private Animator musicBoxAnimator;
    // globalne premenne
    private GlobalObjectsContainer GOC;    
    // UI
    private UIManager uiManager;

    void Start() {
        GOC = GetComponent<GlobalObjectsContainer>();
        uiManager = GetComponent<UIManager>();
        
        // default hodnoty
        exitingToMenu = false;
        correctKeypadCode = false;
        gameOver = false;
        paused = false;       
        exitingToMenu = false;
        alreadyPlayed = false;
        
        // kurzor locknuty v strede obrazovky
        Cursor.lockState = CursorLockMode.Locked;        
        Cursor.visible = true;

        // animatory
        fuseBoxAnimator = GOC.fuseBox.GetComponent<Animator>();
        musicBoxAnimator = GOC.musicBox.GetComponent<Animator>();
    }

    void Update() {

        // koniec hry po vyprsani casu
        if (gameOver) {
            StartCoroutine(GameOver());
            gameOver = false;
        } 
        
        // raycast podla otocenia kamery
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, 10.0f)) return;
        
        SetCursor(); // nastavenie kurzora               
        
        if (paused) return;
        
       // OnClick pre interaktivne predmety
       if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("Interactable")) 
           ObjectOnClick(hit.transform.name);

       // spravny kod knih -> posunutie obrazu
       if (CorrectBookCode()) {
           if (!alreadyPlayed) {
               StartCoroutine(nameof(MovePicture));
               alreadyPlayed = true;
           }
       }

       // spravny kod keypadu -> odomknutie suflika
       if (correctKeypadCode) { 
           StartCoroutine(UnlockDrawer());
           correctKeypadCode = false;
       }

       // hracia skrinka dohrala -> vysunutie kluca
        MusicBoxFinish(musicBoxAnimator.GetBool("SwitchOn") &&
                       !GOC.musicBox.GetComponent<AudioSource>().isPlaying);

    }
    
    
    /************************************ PRIVATE *************************************/
    
    private IEnumerator UnlockDrawer() {

        // odomknutie
        GOC.unlockAudio.Play(); 
        GOC.drawerLocked.GetComponent<Interact>().locked = false;
        
        // delay
        yield return new WaitForSeconds(1.0f);
        
        // otvorenie
        GOC.moveAudio.Play();
        Animator animator = GOC.drawerLocked.GetComponent<Animator>();
        animator.SetBool("Interact", true);
        
    }

    private IEnumerator MovePicture() {
        GOC.correctAudio.Play();
        
        yield return new WaitForSeconds(1.0f);
        
        // posunutie obrazu
        GOC.moveAudio.Play();
        Animator animator = GOC.painting.GetComponent<Animator>();
        animator.SetBool("Interact", true);
        
    }

    private void MusicBoxFinish(bool finished) {
        if (!finished) return;
     
        GOC.correctAudio.Play();
        
        GameObject trapDoorKey = GameObject.Find("TrapDoorKey");
            
        // kamera sa otaca za klucom
        MouseLook_Custom.SetTransformToFollow(trapDoorKey, 200f);
        
        // kluc bude 'Collectable'
        trapDoorKey.tag = "Collectable";
        musicBoxAnimator.SetBool("SwitchOn",false);

    }
    
    // interakcie s 'Interactable' predmetmi
    private void ObjectOnClick(string clickedObjectName) {
        
        switch (clickedObjectName) {
               
            /****************** fuse box ******************/
            case "FuseBoxSwitch":
                fuseBoxAnimator.SetBool("SwitchOn",true);
                Invoke(nameof(SwitchOnLights),2.0f);
                break;
            
            /****************** music box ******************/
            case "BoxLid":
                GameObject.Find("TrapDoorKey").tag = "Interactable";
                break;
            case "TrapDoorKey":
                GOC.musicBox.GetComponent<AudioSource>().Play();
                musicBoxAnimator.SetBool("SwitchOn",true);
                break;
            
            /******************** radio *********************/
            case "RadioBack":
                if (GOC.inventory.IsInInventory("Screwdriver")) 
                    GOC.inventory.RemoveFromInventory("Screwdriver");
                break;
            
            /****************** trap door ******************/
            case "Carpet":
                GOC.trapDoor.SetActive(true);
                break;
            
            case "TrapDoor":
                if (GOC.inventory.IsInInventory("TrapDoorKey")) {
                    GOC.inventory.RemoveFromInventory("TrapDoorKey");
                    GOC.trapDoor.transform.Find("Lock").gameObject.SetActive(false);
                    GOC.unlockAudio.Play();
                }
                break;
            
            /****************** wardrobe ******************/
            case "W_door1":
            case "W_door2":
                if (GOC.inventory.IsInInventory("WardrobeKey")) {
                    GOC.inventory.RemoveFromInventory("WardrobeKey");
                    GOC.wardrobe.transform.Find("Lock").gameObject.SetActive(false);
                    GOC.unlockAudio.Play();
                }
                break;
            
            /****************** desk lamp ******************/
            case "DeskLampPurple_OFF":
                if (GOC.inventory.IsInInventory("PurpleLightBulb")) {
                    GOC.deskLampPurple.transform.Find("DeskLampPurple_OFF").gameObject.SetActive(false);
                    GOC.deskLampPurple.transform.Find("DeskLampPurple_ON").gameObject.SetActive(true);
                    GOC.inventory.RemoveFromInventory("PurpleLightBulb");
                }
                break;
        }
    }

    
    // zapnutie svetiel
    private void SwitchOnLights() {
        GOC.lightsOff.SetActive(false);
        GOC.lightsOn.SetActive(true);
        GOC.fuseBox.transform.Find("Point Light").gameObject.SetActive(false); // zmizne point light nad FuseBoxom
        GOC.fuseBox.GetComponent<AudioSource>().Play();    // zvukovy efekt
    }
    

    private IEnumerator GameOver() {
        
        yield return new WaitForSeconds(2.0f);
        
        // vypnut svetla
        GOC.lightsOn.SetActive(false);
        GOC.lightsOff.SetActive(true);
        GOC.fuseBox.GetComponent<AudioSource>().Play();
        
        // otocenie kamery
        yield return new WaitForSeconds(1.0f);
        
        // blikanie svetiel, rozbitie dveri
        GOC.gameOver.GetComponent<Animator>().SetBool("GameOver", true);
        
        yield return new WaitForSeconds(0.5f);
        
        // audio
        GOC.player.GetComponent<AudioSource>().Play();
        
        // zombici
        ZombieFollowPlayer.gameOver = true;
        
        yield return new WaitForSeconds(2.5f);

        // player zomrie
        GOC.player.GetComponent<Animator>().SetBool("Dead",true);
        
        // game over obrazovka, fade out
        Fade fade = GetComponent<Fade>();
        StartCoroutine(fade.FadeOutMultiple(GOC.gameOverScreen, fade.fadeImage));
        StartCoroutine(WaitAndLoadScene("Escape Room", 6f));     // reload hry
    }
    
    // zmena ikony kurzora
    private void SetCursor() {
        if (paused || exitingToMenu) 
            uiManager.SetCursorConfined();
        else 
            uiManager.SetCursorLocked(hit);
    }

    // check, ci je spravny kod knih
    private bool CorrectBookCode() {
        if (GOC.books.Length != 6)
            return false;
        
        for (int i = 0; i<GOC.books.Length; i++) {
            Animator animator = GOC.books[i].GetComponent<Animator>();
            if (bookCode[i] != animator.GetBool("Interact"))    // porovnanie polohy knih so spravnym kodom
                return false;
        }
        return true;
    }
    
    
    /************************************ PUBLIC *************************************/
    
    
    public static void PauseGame () {
        Time.timeScale = 0;
        paused = true;
    }

    public static void ResumeGame () {
        Time.timeScale = 1;
        paused = false;
    }
    
    public static IEnumerator WaitAndLoadScene(string sceneName, float time) {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneName);
    }
}
