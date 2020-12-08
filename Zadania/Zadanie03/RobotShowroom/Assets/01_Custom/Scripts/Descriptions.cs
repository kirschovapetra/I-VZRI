using UnityEngine;

// zobrazovanie popisu casti tela robota
public class Descriptions : MonoBehaviour {

    public GameObject description;
    public void ToggleDescription() {
        if (description.activeSelf) 
            description.SetActive(false);
        else 
            description.SetActive(true);
    }
}
