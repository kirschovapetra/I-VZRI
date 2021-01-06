using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private static Dictionary<string, GameObject> inventory = new Dictionary<string, GameObject>();
    
    public static void AddToInventory(GameObject obj) {
        inventory.Add(obj.name, obj);
        obj.SetActive(false);
        print(obj.name +"pridane; Inventory = "+inventory);
    }
    
    public static void RemoveFromInventory(string objName) {
        inventory.Remove(objName);
        print(objName + "odobrane; Inventory = "+inventory);
    }

    public static Boolean IsInInventory(string objName) {
        return inventory.ContainsKey(objName);
    }
}
