using TMPro;
using UnityEngine;

// globalne premenne
public class GlobalObjectsContainer : MonoBehaviour {
    [Header("Interaktívne objekty")] 
    public GameObject fuseBox;
    public GameObject musicBox;
    public GameObject painting;
    public GameObject drawerLocked;
    public GameObject radioBack;
    public GameObject trapDoor;
    public GameObject wardrobe;
    public GameObject deskLampPurple;
    public GameObject wallLampPurple;
    public GameObject[] books;
    public GameObject lightsOff;
    public GameObject lightsOn;
    [Header("Audio")] 
    public AudioSource correctAudio;
    public AudioSource lockedAudio;
    public AudioSource unlockAudio;
    public AudioSource collectAudio;
    public GameObject forestAudio_multi;
    [Header("Text na spodnej časti obrazovky")] 
    public TextMeshProUGUI commentText;
    [HideInInspector]
    public Inventory inventory;

    private void Start() { inventory = GetComponent<Inventory>(); }
}
