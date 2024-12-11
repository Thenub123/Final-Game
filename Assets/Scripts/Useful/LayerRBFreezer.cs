using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerRBFreezer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 13) {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == 13) {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
