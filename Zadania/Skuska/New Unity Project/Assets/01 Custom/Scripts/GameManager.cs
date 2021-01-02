using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class GameManager : MonoBehaviour {
    public Camera cam;
    public Texture2D cursorSelect;
    public Texture2D cursorPointer;
    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray,out hit, 100.0f)) {
           
            if (hit.transform.CompareTag("Interactable")) {
                Cursor.SetCursor(cursorSelect, Vector2.zero,CursorMode.Auto);
            }
            else {
                Cursor.SetCursor(cursorPointer, Vector2.zero,CursorMode.Auto);
            }
        }
    }
}
