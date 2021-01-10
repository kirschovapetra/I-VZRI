using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairJumpScare : MonoBehaviour {
    public GameObject chair;
    private Animator animator;
    void Start() {
        animator = chair.GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other) {
        Boolean isMoving = animator.GetBool("Move");
        if (!isMoving && other.CompareTag("Player")) {
            animator.SetBool("Move",true);
            GetComponent<AudioSource>().Play();
            chair.GetComponent<AudioSource>().Play();
        }
    }
}
