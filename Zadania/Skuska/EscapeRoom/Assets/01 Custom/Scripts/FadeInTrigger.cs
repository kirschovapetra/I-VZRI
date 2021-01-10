using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInTrigger : MonoBehaviour {
    public FadeScreen fadeScreen;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            fadeScreen.FadeIn();
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }
    }
}
