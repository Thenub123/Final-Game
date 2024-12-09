using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class movingObject : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.layer == 7) {
            collision.collider.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.layer == 7) {
            collision.collider.transform.SetParent(null);
        }
    }
}
