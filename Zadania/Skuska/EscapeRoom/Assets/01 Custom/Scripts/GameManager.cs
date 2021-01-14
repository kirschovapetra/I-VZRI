using System;
using NUnit.Framework.Constraints;
using UnityEngine;

// manager hry
public class GameManager : MonoBehaviour {
    [Header("Main kamera")]
    public Camera cam;                   
    
    [Header("Kurzory")]
    public Texture2D cursorSelect;       
    public Texture2D cursorPointer;
    
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
    
    private Boolean alreadyPlayed;
    
    public static Boolean exitingToMenu = false;
    
    void Start() {
        exitingToMenu = false;
        // kurzor locknuty v strede obrazovky
        Cursor.lockState = CursorLockMode.Locked;        
        Cursor.visible = true;

        GOC = GetComponent<GlobalObjectsContainer>();
        
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
               GOC.correctAudio.Play();
               alreadyPlayed = true;
           }
           Animator animator = GOC.painting.GetComponent<Animator>();
           animator.SetBool("Interact", true);
       }

       // spravny kod keypadu -> odomkne sa suflik
        if (correctKeypadCode) {
            GOC.drawerLocked.GetComponent<Interact>().locked = false;
            Animator animator = GOC.drawerLocked.GetComponent<Animator>();
            animator.SetBool("Interact", true);
            correctKeypadCode = false;
        }

        // hracia skrinka dohrala -> vysunie sa kluc
        Boolean musicBoxFinished = musicBoxAnimator.GetBool("SwitchOn") && 
                                   !GOC.musicBox.GetComponent<AudioSource>().isPlaying;
        
        if (musicBoxFinished) {
            GOC.correctAudio.Play();
            GameObject.Find("TrapDoorKey").tag = "Collectable";
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
        
        if (paused || exitingToMenu) {
            // kurzorom sa da hybat, ikonka = pointer
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.SetCursor(cursorPointer, Vector2.zero, CursorMode.Auto);
        }
        else { 
            // kurzor je locknuty v strede obrazovky
            Cursor.lockState = CursorLockMode.Locked;
            // zmena ikonky kurzora        
            if (hit.transform.CompareTag("Interactable") || hit.transform.CompareTag("Collectable"))
                Cursor.SetCursor(cursorSelect, Vector2.zero, CursorMode.Auto);
            else
                Cursor.SetCursor(cursorPointer, Vector2.zero, CursorMode.Auto);
        }
        Cursor.visible = true;
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
    
}
