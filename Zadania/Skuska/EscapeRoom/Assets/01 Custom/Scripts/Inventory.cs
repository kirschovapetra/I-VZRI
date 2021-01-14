using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Inventar
public class Inventory : MonoBehaviour {
    public GameObject inventoryPanel;            // UI panel inventaru
    public GameObject inventoryButtonPrefab;     // prefab buttonu inventaru na bocnom paneli 
    public Sprite[] sprites;                     // obrazky 'Collectable' objektov

    // inventar <nazov, objekt>
    private Dictionary<string, GameObject> inventory = new Dictionary<string, GameObject>();
    
    // dictionary obrazkov <nazov, obrazok>
    private readonly Dictionary<String, Sprite> spriteDictionary = new Dictionary<string, Sprite>();
    // pozicia Y posledneho buttonu na paneli
    private float lastPosY = 0.0f;         
   
    void Start() {
        // naplnenie dictionary obrazkov
        foreach (var sprite in sprites) 
            spriteDictionary.Add(sprite.name, sprite);
    }

    void Update() {
        // zobrazenie/skrytie inventaru po stlaceni 'I'
        if (Input.GetKeyDown(KeyCode.I)) 
            ToggleInventory(); 
    }

    // pridanie noveho objektu do inventara
    public void AddToInventory(GameObject obj) {
        inventory.Add(obj.name, obj);
        obj.SetActive(false);
        AddToInventory_UI(obj.name);
    }
    
    // odstranenie objektu z inventara
    public void RemoveFromInventory(string objName) {
        inventory.Remove(objName);
        RemoveFromInventory_UI(objName);
    }
    
    public Boolean IsInInventory(string objName) { return inventory.ContainsKey(objName); }
    
    
    // zobrazenie detailu inventaroveho itemu - zvacseny obrazok
    public void ShowInventoryDetail() {
        // kliknuty button
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject; 
        // nazov obrazka buttonu (child element buttonu je selectedButton/Image)
        string imageName = selectedButton.transform.Find("Image").GetComponent<Image>().sprite.name;
        // prislusny UI panel k buttonu
        GameObject itemToShow = inventoryPanel.transform.Find(imageName + "_UI").gameObject;
        
        // zobrazi sa selected, skryju sa vsetky ostatne
        itemToShow.SetActive(!itemToShow.activeSelf);
        HideInventoryDetails(itemToShow.name);
    }

    // skrytie nahladov vsetkych itemov okrem 'selected'
    private void HideInventoryDetails(string selected) {
        // loop cez child elementy inventoryPanela
        foreach (Transform child in inventoryPanel.transform.GetComponentsInChildren<Transform>()) {
            if (child.name.EndsWith("_UI") && !child.name.Equals(selected)) {    // UI panel, ktory nie je selected
                child.gameObject.SetActive(false);    
            }
        }
    }
    
    // zobrazenie / skrytie inventaroveho panelu
    private void ToggleInventory() {
        HideInventoryDetails(""); // skryju sa vsetky nahlady
        
        Boolean inventoryVisible = !inventoryPanel.activeSelf;
        
        // hra je pauznuta, ked sa zobrazuje inventar
        if (inventoryVisible) 
            GameManager.PauseGame();    
        else 
            GameManager.ResumeGame();
        
        inventoryPanel.SetActive(inventoryVisible);
    }

    // pridanie buttonu do inventaru s ikonkou 'Collectable' objektu
    private void AddToInventory_UI(string objectName) {
        // novy button z prefabu
        GameObject newInventoryButton = Instantiate(inventoryButtonPrefab, inventoryPanel.transform);
        
        // nazov buttonu podla toho aky objekt zobrazuje
        newInventoryButton.name = objectName+"_BUTTON";
        
        // parent = InventoryPanel
        newInventoryButton.transform.SetParent(inventoryPanel.transform);
       
        // pod buttonom je child objekt Image
        Transform button_Image = newInventoryButton.transform.Find("Image");
        // nastavenie obrazku z dictionary podla nazvu objektu (obrazky naju rovnake nazvy ako 'Collectable' objekty)
        button_Image.GetComponent<Image>().sprite = spriteDictionary[objectName];
        
        // pridanie OnClick funkcie ShowInventoryDetail na button
        Button button = newInventoryButton.GetComponent<Button>();
        button.onClick.AddListener(ShowInventoryDetail);
        
        // posunutie position Y buttonu -= 50
        RectTransform inventoryButtonTransform = newInventoryButton.GetComponent<RectTransform>();
        inventoryButtonTransform.anchoredPosition = new Vector2(inventoryButtonTransform.anchoredPosition.x, lastPosY);
        lastPosY -= 50;
    }
    
    // odstranenie buttonu po pouziti itemu z inventaru
    private void RemoveFromInventory_UI(string objectName) {
        // najdenie buttonu podla nazvu
        GameObject buttonToDestroy = inventoryPanel.transform.Find(objectName+"_BUTTON").gameObject;   
        // suradnica Y
        float currentPosY = buttonToDestroy.GetComponent<RectTransform>().anchoredPosition.y;
        // odstranenie buttonu
        Destroy(buttonToDestroy);
        // posunutie buttonov hore
        MoveButtons(currentPosY);
        // odstranenie obrazku z distionary
        spriteDictionary.Remove(objectName);
    }

    // getnutie vsetkych buttonov s nizsou suradnicou Y ako maxY
    private GameObject[] getButtonsLessThan(float maxY) {
        List<GameObject> buttons = new List<GameObject>();
        
        // loop cez child objekty inventoryPanela
        foreach (var child in inventoryPanel.transform.GetComponentsInChildren<Transform>()) {
            // najdenie vsetkych buttonov
            if (child.name.EndsWith("_BUTTON")) {
                // suradnica Y buttonu
                float childPosY = child.gameObject.GetComponent<RectTransform>().anchoredPosition.y;
                // ked je button pod maxY -> do listu
                if (childPosY <= maxY)
                    buttons.Add(child.gameObject);
            }
        }
        return buttons.ToArray();
    }
    
    // posunutie vsetkych buttonov hore, ktore su pod odstranenym buttonom
    private void MoveButtons(float currentPosY) {
        GameObject[] buttonsToMove = getButtonsLessThan(currentPosY);    // pole buttonov na posunutie

        foreach (var button in buttonsToMove) {
            // posun suradnice Y += 50
            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            buttonTransform.anchoredPosition += new Vector2(0,50);
            // nastavenie novej pozicie Y posledneho buttonu
            lastPosY = buttonTransform.anchoredPosition.y-50;
        }
    }
}
