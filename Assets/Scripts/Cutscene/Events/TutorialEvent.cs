using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TutorialEvent : MonoBehaviour
{

    public Transform camPos;
    public GameObject cam;
    public Movement movement;

    public Volume volume;
    public ColorAdjustments CA;

    public Animator anim;

    public bool eventEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out CA);
    }

    // Update is called once per frame
    void Update()
    {
        if(eventEnabled) {
            cam.transform.position = new Vector3(camPos.position.x, camPos.position.y, -10);
            cam.transform.rotation = camPos.rotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 7) {
            eventEnabled = true;
            movement.canMove = false;
            movement.horizontalPressed = 0;
        }
    }
}
