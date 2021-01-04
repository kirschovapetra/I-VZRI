using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

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
    public GameObject[] books;
    public Text keypadText;
    
    private Ray ray;
    private  RaycastHit hit;
    Boolean[] bookCode = {true, false, true, true, false, false};
    void Start() {
        // kurzor je v strede obrazovky
        Cursor.lockState = CursorLockMode.Locked;        
        Cursor.visible = true;
        keypadText.text = "";
    }

    void Update() {
        // raycast podla otocenia kamery
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f)) {
            SetCursor();

            if (hit.transform.CompareTag("Interactable") && Input.GetMouseButtonDown(0)) {
                
                // fuse box
                if (hit.transform.name == "Fuse Box Door") {
                    Animator animator = fuseBox.GetComponent<Animator>();
                    animator.SetBool("Open", !animator.GetBool("Open"));
                }
                else if (hit.transform.name == "Fuse Box Switch") {
                    Animator animator = fuseBox.GetComponent<Animator>();
                    animator.SetBool("SwitchOn",true);
                }
                // music box
                else if (hit.transform.name == "Lid") {
                    Animator animator = musicBox.GetComponent<Animator>();
                    animator.SetBool("Open",true);
                    GameObject.Find("MovingPart").tag = "Interactable";
                }
                else if (hit.transform.name == "MovingPart") {
                    Animator animator = musicBox.GetComponent<Animator>();
                    animator.SetBool("SwitchOn",true);
                    GameObject.Find("MovingPart").tag = "Collectable";
                }
                // radio
                else if (hit.transform.name == "RadioBack"){
                    if (HasScrewdriver()) {
                        Animator animator = radioBack.GetComponent<Animator>();
                        animator.SetBool("Open", true);
                        radioBack.GetComponent<Interact>().missing = false;
                    }
                }
                // trap door
                else if (hit.transform.name == "TrapDoor") {
                    if (HasTrapDoorKey()) {
                        trapDoor.transform.Find("Lock").gameObject.SetActive(false);    // lock zmizne
                        Animator animator = trapDoor.GetComponent<Animator>();
                        animator.SetBool("Open", true);
                        trapDoor.GetComponent<Interact>().locked = false;
                    }
                }
                // warrdrobe
                else if (hit.transform.name == "Wardrobe") {
                    if (HasWardrobeKey()) {
                        wardrobe.transform.Find("Lock").gameObject.SetActive(false);    // lock zmizne
                        Animator animator = wardrobe.GetComponent<Animator>();
                        animator.SetBool("Open", true);
                        wardrobe.GetComponent<Interact>().locked = false;
                    }
                }
            }
            
            // books
            if (CorrectBookCode()) {
                Animator animator = painting.GetComponent<Animator>();
                animator.SetBool("Move",true);
            }
            
            // keypad
            if (CorrectKeypadCode()) {
                Animator animator = drawerLocked.GetComponent<Animator>();
                animator.SetBool("Interact", true);
                drawerLocked.GetComponent<Interact>().locked = false;
            }
        }
    }

    private void SetCursor() {
        // zmena kurzora pre Interactable a Collectable predmety           
        if (hit.transform.CompareTag("Interactable") || hit.transform.CompareTag("Collectable"))
            Cursor.SetCursor(cursorSelect, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(cursorPointer, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
    }

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

    private Boolean CorrectKeypadCode() {
        if (keypadText.text.Equals("4703")) {
            //TODO dobry sound
            return true;
        }

        if (keypadText.text.Length >= 4) {
            keypadText.text = "";
            //TODO zly sound
        }
        return false;
    }

    private Boolean HasScrewdriver() {
        // TODO check v inventari
        return false;
    }
    private Boolean HasTrapDoorKey() {
        // TODO check v inventari
        return false;
    }
    private Boolean HasWardrobeKey() {
        // TODO check v inventari
        return false;
    }
}
