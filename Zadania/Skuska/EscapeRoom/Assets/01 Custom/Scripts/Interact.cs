using System;
using System.Threading;
using UnityEngine;


/* interakcia s predmetmi - bez condition */

public class Interact : MonoBehaviour {
    private Animator animator;
    public Boolean locked;
    public Boolean missing;
    void Start() {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnMouseDown() {
        if (locked) {
            LockedMessage();
            return;
        }
        if (missing) {
            MissingMessage();
            return;
        }
        if (animator!= null) {
            Boolean interact = animator.GetBool("Interact");
            animator.SetBool("Interact", !interact);
        }
    }

    public void LockedMessage() {
        //TODO show msg
        Debug.Log("Zamknute");
    }
    public void MissingMessage() {
        //TODO show msg
        Debug.Log("Nieco tu chyba");
    }
}
