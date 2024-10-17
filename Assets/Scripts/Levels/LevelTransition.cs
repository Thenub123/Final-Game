using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class LevelTransition : MonoBehaviour
{

    public GameObject camera_obj;
    public GameObject camera_pos;

    public Rigidbody2D rb2D;
    
    public Movement movement;

    public bool level_enabled = false;

    public GameObject checkPointObj;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7) {
            level_enabled = true;
            movement.checkPoint = checkPointObj.transform.position;
            // rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 7) {
            level_enabled = false;
            // rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
    }

    void FixedUpdate() {

        float distance = (float) Math.Sqrt(Math.Pow(camera_pos.transform.position.x - camera_obj.transform.position.x, 2f) + Math.Pow(camera_pos.transform.position.y - camera_obj.transform.position.y, 2f));

        if (level_enabled && camera_obj.transform.position != camera_pos.transform.position && movement.isDead == false) {
            camera_obj.transform.position = new Vector3(camera_obj.transform.position.x + ((camera_pos.transform.position.x - camera_obj.transform.position.x) / 10), camera_obj.transform.position.y + ((camera_pos.transform.position.y - camera_obj.transform.position.y) / 10), -10);
            movement.isFrozen = true;
        }

        if (distance < 0.01f) {
            level_enabled = false;
            movement.isFrozen = false;
        }
    }
}
