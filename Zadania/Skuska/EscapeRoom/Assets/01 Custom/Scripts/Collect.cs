using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour {
    public InteractiveObjectsContainer IOC;
    private Transform transf;
    private void OnMouseDown() {
        if (!CompareTag("Collectable")) return;
        
        Inventory.AddToInventory(gameObject);
        IOC.collectAudio.Play();
        
        switch (gameObject.name) {
            case "Screwdriver":
                IOC.radioBack.GetComponent<Interact>().missing = false;
                break;
            case "WardrobeKey":
                GameObject door1 = IOC.wardrobe.transform.Find("W_door1").gameObject;
                door1.GetComponent<Interact>().locked = false;
                GameObject door2 = IOC.wardrobe.transform.Find("W_door2").gameObject;
                door2.GetComponent<Interact>().locked = false;
                break;
            case "TrapDoorKey":
                IOC.trapDoor.GetComponent<Interact>().locked = false;
                break;
            case "PurpleLightBulb":
                IOC.deskLampPurple.transform.Find("DeskLampPurple_OFF").GetComponent<Interact>().missing = false;
                IOC.wallLampPurple.transform.Find("WallLampPurple_OFF").gameObject.SetActive(true);
                IOC.wallLampPurple.transform.Find("WallLampPurple_ON").gameObject.SetActive(false);
                break;
        }
    }
}
