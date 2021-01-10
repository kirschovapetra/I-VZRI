using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private static Dictionary<string, GameObject> inventory = new Dictionary<string, GameObject>();
    
    public static void AddToInventory(GameObject obj) {
        inventory.Add(obj.name, obj);
        obj.SetActive(false);
        print(obj.name +" pridane; inventory length = "+inventory.Count);
    }
    
    public static void RemoveFromInventory(string objName) {
        inventory.Remove(objName);
        print(objName + " odobrane; inventory length = " + inventory.Count);
    }

    public static Boolean IsInInventory(string objName) {
        return inventory.ContainsKey(objName);
    }
}
