using System;
using UnityEngine;


// Interakcie s 'Interactable' objektmi
public class Interact : MonoBehaviour {
    [Header("Stavy objektov")]
    public Boolean locked;         
    public Boolean missing;        
    public Boolean mainDoor;
    [Header("Globálne premenné")]
    public GlobalObjectsContainer GOC;
    
    private Animator animator;
    private AudioSource audioSrc;
    void Start() {
        audioSrc = GetComponent<AudioSource>();
        
        Animator gameObjectAnimator = GetComponent<Animator>();
        Animator parentAnimator = transform.parent.GetComponent<Animator>();
        // bud je animator na objekte alebo v parentovi
        animator = gameObjectAnimator != null ? gameObjectAnimator : parentAnimator;
    }

    // akcie po kliknuti mysou
    private void OnMouseDown() {
        if (GameManager.paused || !gameObject.CompareTag("Interactable")) return;
        
        // koment a zvukovy efekt podla typu objektu
        if (locked) {
            Comment("Zamknuté.");
            GOC.lockedAudio.Play();
            return;
        } 
        if (missing) {
            Comment("Niečo tu chýba.");
            GOC.lockedAudio.Play();
            return;
        } 
        if (mainDoor) {
            Comment("Musí existovať aj iná cesta...");
            // audioSrc.Play();
            // return;
        }
  
        // prehratie zvuku, ak objekt obsahuje AudioSource
        if (audioSrc != null) audioSrc.Play();
        
        // spustenie animacie, ak objekt/jeho parent obsahuje animator
        if (animator != null) {
            Boolean interact = animator.GetBool("Interact");
            animator.SetBool("Interact", !interact);
        }
    }
    
    // komentare na spodnej casti obrazovky
    private void Comment(string comment) {
        GOC.commentText.text = comment;
        GOC.commentText.transform.parent.gameObject.SetActive(true); // zobrazi sa cely spodny panel (parent commentTextu)
        Invoke(nameof(ClearComment), 1.0f);
    }

    // skrytie komentu
    private void ClearComment() {
        GOC.commentText.text = "";
        GOC.commentText.transform.parent.gameObject.SetActive(false); // skryje sa cely spodny panel (parent commentTextu)
    }


}
