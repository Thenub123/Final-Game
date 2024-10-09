using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{

    public Movement movement;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8) {
            movement.Die();
            // rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
    }
}
