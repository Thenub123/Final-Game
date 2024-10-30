using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutsceneMove : MonoBehaviour
{

    public bool moveEnabled = false;

    public Movement movement;

    public GameObject player;

    public bool done = false;

    void Update() {
        if (moveEnabled) {
            movement.canMove = false;
            movement.horizontalPressed = 1;
            if (this.transform.position.x > player.transform.position.x) {
                movement.horizontalPressed = 1;
            }
            else if (this.transform.position.x < player.transform.position.x) {
                movement.horizontalPressed = -1;
            }
            
        } else {
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 7) {
            moveEnabled = false;
            done = true;
            movement.horizontalPressed = 0;
        }
    }

}
