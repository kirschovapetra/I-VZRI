/******************* kontrola spustenia novej hry ********************/

using UnityEngine;

public class NewGameCheck : MonoBehaviour {
    
    public static bool newGameStarted;
    private static NewGameCheck instance;
    void Awake() {
        if (instance != null && instance != this) {   // uz objekt v scene existuje -> destroy
            Destroy(gameObject);
        } else {                                     // objekt zostane existovat aj po prepnuti sceny
            instance = this;
            newGameStarted = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}
