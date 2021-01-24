using UnityEngine;
using UnityEngine.AI;

// zombie prenasleduje playera po NavMesh
public class ZombieFollowPlayer : MonoBehaviour {
    [Header("Player - LookAt Point")]
    public Transform target;  
    public static bool gameOver;
    
    private NavMeshAgent agent;
    private AudioSource audioSource;
    private Animator animator;
    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        
        animator.SetBool("Run", false);
        gameOver = false;
    }
    
    void Update() {
        if (!gameOver) return;

        // zvuk, animacia
        if (!audioSource.isPlaying){
            audioSource.Play();
            animator.SetBool("Run", true);
        }
        
        // pohyb zombie za playerom
        agent.SetDestination(target.transform.position);
        gameObject.transform.LookAt(target);
    }
    
}
