using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] foodPrefabs;
    public float minX = 0;
    public float maxX = 0;
    public float minZ = 0;
    public float maxZ = 0;
    private float interval = 5.0f;
    private float timer;
    // Start is called before the first frame update
    void Start() {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > timer ) {
            timer += interval;
            SpawnFruit();
        }
    }

    
    void SpawnFruit() {
        // nahodne vygenerovane suradnice v range
        float randX = Random.Range(minX, maxX);
        float randZ = Random.Range(minZ, maxZ);
        
        //nahodny index objektu v poli
        int randIndex = Random.Range(0, foodPrefabs.Length);

        Vector3 position = new Vector3(randX, 20.0f, randZ);
        
        if (foodPrefabs.Length > 0)
            Instantiate(foodPrefabs[randIndex], position, transform.rotation);
    }
}
