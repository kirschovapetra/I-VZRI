using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Descriptions : MonoBehaviour {

    public GameObject description;
    public void ToggleDescription() {
        
        if (description.activeSelf) {
            description.SetActive(false);
        } else {
            description.SetActive(true);
            
        }
    }

    
}
