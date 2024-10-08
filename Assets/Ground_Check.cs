using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Check : MonoBehaviour
{
    public Movement movement_script;
    void OnTriggerEnter2D(Collider2D col)
    {
        movement_script.isGrounded = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        movement_script.isGrounded = false;
    }
}