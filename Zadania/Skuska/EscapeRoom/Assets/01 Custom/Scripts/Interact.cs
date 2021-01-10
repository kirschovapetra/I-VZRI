using System;
using System.Threading;
using TMPro;
using UnityEngine;


/* interakcia s predmetmi - bez condition */

public class Interact : MonoBehaviour {
    public Boolean locked;
    public Boolean missing;
    public Boolean mainDoor;
    public InteractiveObjectsContainer IOC;
    private Animator animator;
    private AudioSource audioSrc;
    void Start() {
        audioSrc = GetComponent<AudioSource>();
        
        Animator gameObjectAnimator = GetComponent<Animator>();
        Animator parentAnimator = transform.parent.GetComponent<Animator>();

        animator = gameObjectAnimator != null ? gameObjectAnimator : parentAnimator;
    }

    private void OnMouseDown() {
        if (gameObject.CompareTag("Interactable")) {
        
            if (locked) {
                Comment("Zamknuté.");
                IOC.lockedAudio.Play();
                return;
            } 
            if (missing) {
                Comment("Niečo tu chýba.");
                IOC.lockedAudio.Play();
                return;
            } 
            if (mainDoor) {
                Comment("Musí existovať aj iná cesta...");
                audioSrc.Play();
                return;
            }
      
            if (audioSrc != null) {
                print(audioSrc.clip.name);
                audioSrc.Play();
            }
            if (animator == null) return;
            
            Boolean interact = animator.GetBool("Interact");
            animator.SetBool("Interact", !interact);
        }
    }


    
    private void Comment(string comment) {
        IOC.commentText.text = comment;
        IOC.commentText.transform.parent.gameObject.SetActive(true);
        Invoke(nameof(ClearComment), 1.0f);
    }

    private void ClearComment() {
        IOC.commentText.text = "";
        IOC.commentText.transform.parent.gameObject.SetActive(false);
    }


}
