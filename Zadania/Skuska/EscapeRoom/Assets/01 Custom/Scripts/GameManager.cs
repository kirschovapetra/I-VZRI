using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Main kamera")]
    public Camera cam;                   
    
    [Header("Kurzory")]
    public Texture2D cursorSelect;       
    public Texture2D cursorPointer;
    
    [HideInInspector]
    public Boolean correctKeypadCode = false;
    
    // raycast
    private Ray ray;
    private RaycastHit hit;
    
    // spravny kod knih
    private Boolean[] bookCode = {true, false, true, true, false, false};
    
    // animatory
    private Animator fuseBoxAnimator;
    private Animator musicBoxAnimator;

    private InteractiveObjectsContainer IOC;

    private Boolean playedAudio = false;
    void Start() {
        // kurzor v strede obrazovky
        Cursor.lockState = CursorLockMode.Locked;        
        Cursor.visible = true;

        IOC = GetComponent<InteractiveObjectsContainer>();
        
        // animatory
        fuseBoxAnimator = IOC.fuseBox.GetComponent<Animator>();
        musicBoxAnimator = IOC.musicBox.GetComponent<Animator>();
    }

    void Update() {
        // raycast podla otocenia kamery
        ray = cam.ScreenPointToRay(Input.mousePosition);
        
        // kurzor zachytil objekt
        if (Physics.Raycast(ray, out hit, 10.0f)) {
           SetCursor(); // zmena ikony kurzora               

           // predmety, na ktore sa da kliknut
           if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("Interactable")) 
               ObjectOnClick(hit.transform.name);

           // spravny kod knih -> animacia posunutia obrazu
           if (CorrectBookCode()) {
               if (!playedAudio) {
                   IOC.correctAudio.Play();
                   playedAudio = true;
               }
               Animator animator = IOC.painting.GetComponent<Animator>();
               animator.SetBool("Interact", true);
           }

           // spravny kod keypadu -> odomkne sa suflik
            if (correctKeypadCode) {
                IOC.drawerLocked.GetComponent<Interact>().locked = false;
                Animator animator = IOC.drawerLocked.GetComponent<Animator>();
                animator.SetBool("Interact", true);
                correctKeypadCode = false;
            }

            Boolean musicBoxFinished = musicBoxAnimator.GetBool("SwitchOn") && 
                                       !IOC.musicBox.GetComponent<AudioSource>().isPlaying;
            
            if (musicBoxFinished) {
                IOC.correctAudio.Play();
                GameObject.Find("TrapDoorKey").tag = "Collectable";
                musicBoxAnimator.SetBool("SwitchOn",false);
            }
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
                IOC.musicBox.GetComponent<AudioSource>().Play();
                musicBoxAnimator.SetBool("SwitchOn",true);
                break;
            
            /******************** radio *********************/
            case "RadioBack":
                if (Inventory.IsInInventory("Screwdriver")) 
                    Inventory.RemoveFromInventory("Screwdriver");
                break;
            
            /****************** trap door ******************/
            case "Carpet":
                IOC.trapDoor.SetActive(true);
                break;
            
            case "TrapDoor":
                if (Inventory.IsInInventory("TrapDoorKey")) {
                    Inventory.RemoveFromInventory("TrapDoorKey");
                    IOC.trapDoor.transform.Find("Lock").gameObject.SetActive(false);
                    IOC.unlockAudio.Play();
                }
                break;
            
            /****************** wardrobe ******************/
            case "W_door1":
            case "W_door2":
                if (Inventory.IsInInventory("WardrobeKey")) {
                    Inventory.RemoveFromInventory("WardrobeKey");
                    IOC.wardrobe.transform.Find("Lock").gameObject.SetActive(false);
                    IOC.unlockAudio.Play();
                }
                break;
            
            /****************** desk lamp ******************/
            case "DeskLampPurple_OFF":
                if (Inventory.IsInInventory("PurpleLightBulb")) {
                    IOC.deskLampPurple.transform.Find("DeskLampPurple_OFF").gameObject.SetActive(false);
                    IOC.deskLampPurple.transform.Find("DeskLampPurple_ON").gameObject.SetActive(true);
                    Inventory.RemoveFromInventory("PurpleLightBulb");
                }
                break;
        }
    }

    // zmena ikony kurzora
    private void SetCursor() {
        // zmena kurzora pre Interactable a Collectable predmety           
        if (hit.transform.CompareTag("Interactable") || hit.transform.CompareTag("Collectable"))
            Cursor.SetCursor(cursorSelect, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(cursorPointer, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
    }

    // check, ci je spravny kod knih
    private Boolean CorrectBookCode() {
        if (IOC.books.Length != 6)
            return false;
        
        for (int i = 0; i<IOC.books.Length; i++) {
            Animator animator = IOC.books[i].GetComponent<Animator>();
            if (bookCode[i] != animator.GetBool("Interact"))
                return false;
        }
        return true;
    }
    
    // zapnutie svetiel
    private void SwitchOnLights() {
        IOC.lightsOff.SetActive(false);
        IOC.lightsOn.SetActive(true);
        IOC.fuseBox.transform.Find("Point Light").gameObject.SetActive(false);
        IOC.fuseBox.GetComponent<AudioSource>().Play();
    }

}
