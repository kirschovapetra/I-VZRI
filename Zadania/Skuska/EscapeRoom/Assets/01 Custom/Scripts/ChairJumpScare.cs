using System;
using UnityEngine;

// jumpscare na stolicke po prechode cez trigger
public class ChairJumpScare : MonoBehaviour {
    
    public GameObject chair;
    
    private Animator chairAnimator;
    private Animator flashingLightAnimator;

    void Start() {
        flashingLightAnimator = GetComponent<Animator>();
        chairAnimator = chair.GetComponent<Animator>();
    }
    
    private void OnTriggerEnter(Collider other) {
        
        bool isMoving = chairAnimator.GetBool("Move");
        
        // player prejde triggerom
        if (!isMoving && other.CompareTag("Player")) {
            
            // kamera sa otaca za kreslom
            MouseLook_Custom.SetTransformToFollow(transform.Find("LookAtPoint").gameObject, 200f);
           
            // animacia, zvukove efekty
            chairAnimator.SetBool("Move",true);
            flashingLightAnimator.SetBool("Play",true);
            GetComponent<AudioSource>().Play();
            chair.GetComponent<AudioSource>().Play();
        }
    }
}
