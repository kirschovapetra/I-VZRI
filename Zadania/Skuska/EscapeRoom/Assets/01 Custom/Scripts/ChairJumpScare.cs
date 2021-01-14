using System;
using UnityEngine;

// jumpscare na stolicke po prechode cez trigger
public class ChairJumpScare : MonoBehaviour {
    public GameObject chair;
    private Animator animator;
    void Start() { animator = chair.GetComponent<Animator>(); }


    private void OnTriggerEnter(Collider other) {
        Boolean isMoving = animator.GetBool("Move");
        
        // animacia, zvukove efekty
        if (!isMoving && other.CompareTag("Player")) {
            animator.SetBool("Move",true);
            GetComponent<AudioSource>().Play();
            chair.GetComponent<AudioSource>().Play();
        }
    }
}
