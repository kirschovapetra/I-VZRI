/********************** globalne premenne ************************/

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalObjectsContainer : MonoBehaviour {
    [Header("Player container")] 
    public GameObject player;
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
    public GameObject gameOver;
    [Header("Audio")] 
    public AudioSource moveAudio;
    public AudioSource correctAudio;
    public AudioSource lockedAudio;
    public AudioSource unlockAudio;
    public AudioSource collectAudio;
    public GameObject forestAudio_multi;
    [Header("UI")] 
    public TextMeshProUGUI commentText;
    public Image gameOverScreen;
    
    [HideInInspector]
    public Inventory inventory;
    
    private void Start() { inventory = GetComponent<Inventory>(); }
}
