using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    [Header("Main kamera")]
    public Camera cam;                   
    
    [Header("Kurzory")]
    public Texture2D cursorSelect;       
    public Texture2D cursorPointer;
    
    [Header("Interaktívne objekty")] 
    public GameObject fuseBox;
    public GameObject musicBox;
    public GameObject painting;
    public GameObject drawerLocked;
    public GameObject radioBack;
    public GameObject trapDoor;
    public GameObject wardrobe;
    public GameObject deskLampPurple;
    public GameObject[] books;
    public GameObject lightsOff;
    public GameObject lightsOn;
    
    public TextMeshProUGUI commentText;
    
    [HideInInspector]
    public Boolean correctKeypadCode = false;
    
    // raycast
    private Ray ray;
    private RaycastHit hit;
    
    // spravny kod knih
    private Boolean[] bookCode = {true, false, true, true, false, false};
    
    // animatory
    Animator fuseBoxAnimator;
    Animator musicBoxAnimator;
    Animator radioAnimator;
    Animator trapDoorAnimator;
    Animator wardrobeAnimator;

    void Start() {
        // kurzor v strede obrazovky
        Cursor.lockState = CursorLockMode.Locked;        
        Cursor.visible = true;
        
        // animatory
        fuseBoxAnimator = fuseBox.GetComponent<Animator>();
        musicBoxAnimator = musicBox.GetComponent<Animator>();
        radioAnimator = radioBack.GetComponent<Animator>();
        trapDoorAnimator = trapDoor.GetComponent<Animator>();
        wardrobeAnimator = wardrobe.GetComponent<Animator>();
    }

    void Update() {
        // raycast podla otocenia kamery
        ray = cam.ScreenPointToRay(Input.mousePosition);
        
        // kurzor zachytil objekt
        if (Physics.Raycast(ray, out hit, 10.0f)) {
           SetCursor(); // zmena ikony kurzora               

           // predmety, na ktore sa da kliknut
           if (Input.GetMouseButtonDown(0)) {
               if (hit.transform.CompareTag("Interactable") ) 
                   Interact(hit.transform.name);
               else if (hit.transform.CompareTag("Collectable")) 
                   Inventory.AddToInventory(hit.transform.gameObject);
           }
           
           // spravny kod knih -> animacia posunutia obrazu
            if (CorrectBookCode()) {
                Animator animator = painting.GetComponent<Animator>();
                animator.SetBool("Move",true);
            }
            
            // spravny kod keypadu -> odomkne sa suflik
            if (correctKeypadCode) {
                Animator animator = drawerLocked.GetComponent<Animator>();
                animator.SetBool("Interact", true);
                drawerLocked.GetComponent<Interact>().locked = false;
            }
        }
    }
    
    // interakcie s klikatelnymi predmetmi
    private void Interact(String objectHitName) {

        switch (objectHitName) {
            /****************** main door ******************/
            case "Main Door":
                commentText.text = "Musí existovať aj iná cesta...";
                break;
            
            /****************** fuse box ******************/
            case "Fuse Box Door":
                fuseBoxAnimator.SetBool("Interact", 
                    !fuseBoxAnimator.GetBool("Interact"));
                break;
            case "Fuse Box Switch":
                fuseBoxAnimator.SetBool("SwitchOn",true);
                Invoke(nameof(SwitchOnLights),2.0f);
                break;
            
            /****************** music box ******************/
            case "Lid":
                musicBoxAnimator.SetBool("Interact",true);
                GameObject.Find("MovingPart").tag = "Interactable";
                break;
            case "MovingPart":
                musicBoxAnimator.SetBool("SwitchOn",true);
                GameObject.Find("Handle").tag = "Collectable";
                break;
            
            /******************** radio *********************/
            case "RadioBack":
                if (Inventory.IsInInventory("Screwdriver")) {
                    radioAnimator.SetBool("Interact", true);
                    radioBack.GetComponent<Interact>().missing = false;
                    Inventory.RemoveFromInventory("Screwdriver");
                }
                else {
                    commentText.text = "Niečo tu chýba.";
                }
                break;
            
            /****************** trap door ******************/
            case "TrapDoor":
                if (Inventory.IsInInventory("TrapDoorKey")) {
                    trapDoor.transform.Find("Lock").gameObject.SetActive(false);    // lock zmizne
                    trapDoorAnimator.SetBool("Interact", true);
                    trapDoor.GetComponent<Interact>().locked = false;
                    Inventory.RemoveFromInventory("TrapDoorKey");
                } else {
                    commentText.text = "Zamknuté.";
                }
                break;
            
            /****************** wardrobe ******************/
            case "Wardrobe":
                if (Inventory.IsInInventory("WardrobeKey")) {
                    wardrobe.transform.Find("Lock").gameObject.SetActive(false);    // lock zmizne
                    wardrobeAnimator.SetBool("Interact", true);
                    wardrobe.GetComponent<Interact>().locked = false;
                    Inventory.RemoveFromInventory("WardrobeKey");
                } 
                else {
                    commentText.text = "Zamknuté.";
                }
                break;
            
            /****************** desk lamp ******************/
            case "DeskLampPurple OFF":
                if (Inventory.IsInInventory("PurpleLightBulb")) {
                    deskLampPurple.transform.Find("DeskLampPurple OFF").gameObject.SetActive(false);
                    deskLampPurple.transform.Find("DeskLampPurple ON").gameObject.SetActive(true);
                    Inventory.RemoveFromInventory("PurpleLightBulb");
                }
                else {
                    commentText.text = "Niečo tu chýba.";
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
        if (books.Length != 6)
            return false;
        
        for (int i = 0; i<books.Length; i++) {
            Animator animator = books[i].GetComponent<Animator>();
            if (bookCode[i] != animator.GetBool("Interact"))
                return false;
        }
        return true;
    }
    
    // zapnutie svetiel
    private void SwitchOnLights() {
        lightsOff.SetActive(false);
        lightsOn.SetActive(true);
        fuseBox.transform.Find("Point Light").gameObject.SetActive(false);
    }
    
}
