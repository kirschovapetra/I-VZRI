using UnityEngine;

// zbieranie 'Collectable' objektov do inventaru
public class Collect : MonoBehaviour {
    
    public GlobalObjectsContainer GOC;    // globalne premenne
    
    private void OnMouseDown() {
        // ked je pauza alebo objekt nie je Collectable, nic sa nedeje
        if (GameManager.paused || !CompareTag("Collectable")) return;
        
        // pridanie do inventara
        GOC.inventory.AddToInventory(gameObject);
        GOC.collectAudio.Play();
        
        switch (gameObject.name) {
            case "Screwdriver":
                // bude sa dat otvorit radio
                GOC.radioBack.GetComponent<Interact>().missing = false;
                break;
            case "WardrobeKey":
                // odomkne sa skrina
                GameObject door1 = GOC.wardrobe.transform.Find("W_door1").gameObject;
                door1.GetComponent<Interact>().locked = false;
                GameObject door2 = GOC.wardrobe.transform.Find("W_door2").gameObject;
                door2.GetComponent<Interact>().locked = false;
                break;
            case "TrapDoorKey":
                // odomknu sa padacie dvere
                GOC.trapDoor.GetComponent<Interact>().locked = false;
                break;
            case "PurpleLightBulb":
                // da sa zasvietit lampa na stole
                GOC.deskLampPurple.transform.Find("DeskLampPurple_OFF").GetComponent<Interact>().missing = false;
                // nahradi sa zapnuta lampa vypnutou
                GOC.wallLampPurple.transform.Find("WallLampPurple_OFF").gameObject.SetActive(true);
                GOC.wallLampPurple.transform.Find("WallLampPurple_ON").gameObject.SetActive(false);
                break;
        }
    }
}
