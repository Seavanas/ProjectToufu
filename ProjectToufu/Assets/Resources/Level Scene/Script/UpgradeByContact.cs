using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeByContact : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.GetComponent<PlayerController>().Upgrade();
        }
    }
}
