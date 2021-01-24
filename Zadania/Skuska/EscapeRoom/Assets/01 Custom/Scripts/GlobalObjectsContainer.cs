using TMPro;
using UnityEngine;
using UnityEngine.UI;

// globalne premenne
public class GlobalObjectsContainer : MonoBehaviour {
    [Header("First Person Controller")] 
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
    public AudioSource correctAudio;
    public AudioSource lockedAudio;
    public AudioSource unlockAudio;
    public AudioSource collectAudio;
    public GameObject forestAudio_multi;
    [Header("UI")] 
    public TextMeshProUGUI commentText;
    public Image gameOverScreen;
    public TextMeshProUGUI[] startGameMessages;
    
    [HideInInspector]
    public Inventory inventory;

    // text na zaciatku hry (backup)
    [HideInInspector] 
    public string[] startGameText = { "Počas nočnej prechádzky lesom\nťa kroky zaviedli do opusteného domu.",
                                      "Dom však nie je taký opustený,\nako by sa na prvý pohľad zdalo...",
                                      "Nájdi cestu von skôr, ako sa k tebe dostanú.",
                                      "Máš na to 15 minút.",
                                      "Veľa šťastia..."};
    private void Start() { inventory = GetComponent<Inventory>(); }
}
