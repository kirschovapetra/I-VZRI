using System;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SceneManagement;

// manager hry
public class GameManager : MonoBehaviour {
    [Header("Main kamera")]
    public Camera cam;

    [HideInInspector]
    public Boolean correctKeypadCode;
    public static Boolean paused;
    
    // raycast
    private Ray ray;
    private RaycastHit hit;
    
    // spravny kod knih
    private readonly Boolean[] bookCode = {true, false, true, true, false, false};
    
    // animatory
    private Animator fuseBoxAnimator;
    private Animator musicBoxAnimator;
    
    // globalne premenne
    private GlobalObjectsContainer GOC;    
    private UIManager uiManager;    
    
    private Boolean alreadyPlayed;
    
    public static Boolean exitingToMenu = false;
    
    void Start() {
        exitingToMenu = false;
        
        // kurzor locknuty v strede obrazovky
        Cursor.lockState = CursorLockMode.Locked;        
        Cursor.visible = true;

        GOC = GetComponent<GlobalObjectsContainer>();
        uiManager = GetComponent<UIManager>();
        
        // animatory
        fuseBoxAnimator = GOC.fuseBox.GetComponent<Animator>();
        musicBoxAnimator = GOC.musicBox.GetComponent<Animator>();
    }

    void Update() {
        
        // raycast podla otocenia kamery
        ray = cam.ScreenPointToRay(Input.mousePosition);
        
        // kurzor nezachytil objekt -> nic sa nestane
        if (!Physics.Raycast(ray, out hit, 10.0f)) return;
        
        // nastavenie kurzora
        SetCursor();               
        
        // ked je pauza, nic sa nestane
        if (paused) return;
        
       // predmety, na ktore sa da kliknut - 'Interactable'
       if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("Interactable")) 
           ObjectOnClick(hit.transform.name);

       // spravny kod knih -> animacia posunutia obrazu
       if (CorrectBookCode()) {

           if (!alreadyPlayed) {
               // kamera sa otaca na obraz
               MouseLook_Custom.SetTransformToFollow(GOC.painting, 200f);
               GOC.correctAudio.Play();
               alreadyPlayed = true;
           }
           Animator animator = GOC.painting.GetComponent<Animator>();
           animator.SetBool("Interact", true);
       }

       // spravny kod keypadu -> odomkne sa suflik
        if (correctKeypadCode) {
            // kamera sa otaca na suflik
            MouseLook_Custom.SetTransformToFollow(GOC.drawerLocked, 200f);

            GOC.drawerLocked.GetComponent<Interact>().locked = false;
            Animator animator = GOC.drawerLocked.GetComponent<Animator>();
            animator.SetBool("Interact", true);
            correctKeypadCode = false;
        }

        // hracia skrinka dohrala -> vysunie sa kluc
        Boolean musicBoxFinished = musicBoxAnimator.GetBool("SwitchOn") && 
                                   !GOC.musicBox.GetComponent<AudioSource>().isPlaying;
        
        if (musicBoxFinished) {
            GameObject trapDoorKey = GameObject.Find("TrapDoorKey");
            
            // kamera sa otaca za klucom
            MouseLook_Custom.SetTransformToFollow(trapDoorKey, 200f);
            
            GOC.correctAudio.Play();
            trapDoorKey.tag = "Collectable";
            musicBoxAnimator.SetBool("SwitchOn",false);
        }
        
    }
    
    // interakcie s klikatelnymi predmetmi
    private void ObjectOnClick(String clickedObjectName) {
        
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

    // zmena ikony kurzora
    private void SetCursor() {
        if (paused || exitingToMenu) 
            uiManager.SetCursorConfined();
        else 
            uiManager.SetCursorLocked(hit);
    }

    // check, ci je spravny kod knih
    private Boolean CorrectBookCode() {
        if (GOC.books.Length != 6)
            return false;
        
        for (int i = 0; i<GOC.books.Length; i++) {
            Animator animator = GOC.books[i].GetComponent<Animator>();
            if (bookCode[i] != animator.GetBool("Interact"))    // porovnanie polohy knih so spravnym kodom
                return false;
        }
        return true;
    }
    
    // zapnutie svetiel
    private void SwitchOnLights() {
        GOC.lightsOff.SetActive(false);
        GOC.lightsOn.SetActive(true);
        GOC.fuseBox.transform.Find("Point Light").gameObject.SetActive(false); // zmizne point light nat FuseBoxom
        GOC.fuseBox.GetComponent<AudioSource>().Play();    // zvukovy efekt
    }

    public static void PauseGame () {
        Time.timeScale = 0;
        // AudioListener.pause = true;
        paused = true;
    }

    public static void ResumeGame () {
        Time.timeScale = 1;
        // AudioListener.pause = false;
        paused = false;
    }
    
    public static IEnumerator WaitAndLoadScene(string sceneName, float time) {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneName);
    }
    
}
