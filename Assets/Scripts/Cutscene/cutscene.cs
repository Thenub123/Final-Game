using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using Unity.VisualScripting;

public class cutscene : MonoBehaviour
{

    [Header("Events")]
    public EventHandler[] events;

    [Header("References")]

    public Movement movement;

    public int currentEvent = 0;

    public int eventLength;

    public bool cutsceneEnabled = false;

    private void Start() {
        eventLength = events.Length;
    }

    private void Update() {
        if(cutsceneEnabled){
            if(events[currentEvent].done && eventLength - 1 != currentEvent){
                currentEvent += 1; 
                eventController();
            } 
            if (eventLength< currentEvent) cutsceneEnabled = false;
        } 
        // else movement.canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7) {
            movement.horizontalPressed = 0;
            cutsceneEnabled = true;
            movement.canMove = false;

            eventController();
        }
    }

    private void eventController() {
        events[currentEvent].cutsceneEnabled = true;
    }
}