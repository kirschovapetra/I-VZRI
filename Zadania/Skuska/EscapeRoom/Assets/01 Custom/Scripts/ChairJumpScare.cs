using System;
using UnityEngine;

// jumpscare na stolicke po prechode cez trigger
public class ChairJumpScare : MonoBehaviour {
    public GameObject chair;
    private Animator flashingLightAnimator;
    private Animator animator;

    void Start() {
        flashingLightAnimator = GetComponent<Animator>();
        animator = chair.GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other) {
        Boolean isMoving = animator.GetBool("Move");
        
        // animacia, zvukove efekty
        if (!isMoving && other.CompareTag("Player")) {
            
            // kamera sa otaca za kreslom
            MouseLook_Custom.SetTransformToFollow(transform.Find("LookAtPoint").gameObject, 200f);
           
            animator.SetBool("Move",true);
            flashingLightAnimator.SetBool("Play",true);
            GetComponent<AudioSource>().Play();
            chair.GetComponent<AudioSource>().Play();
        }
    }
}
