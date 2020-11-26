using UnityEngine;

// zobrazenie a skytie ohraniceni na pohybujucich paneloch
public class BorderTrigger : MonoBehaviour {
    public GameObject borders;
    public PlayerController playerController;

    void Start() { borders.SetActive(false); }

    private void Update() {
        // ked hrac skace, ohranicenie zmizne
        if (playerController.jumping) {
            borders.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision other) {
        // ked sa hrac dotkne panela, ohranicenie sa zobrazi
        if (other.gameObject.CompareTag("Player"))
            borders.SetActive(true);
    }
    
}
