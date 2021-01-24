using UnityEngine;

// check na spustenie novej hry
public class NewGameCheck : MonoBehaviour {
    
    public static bool newGameStarted;
    private static NewGameCheck instance;
    void Awake() {
        // uz objekt v scene existuje
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        newGameStarted = true;
        DontDestroyOnLoad(gameObject);

    }
}
