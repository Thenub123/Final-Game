using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueOrder {
    public Sprite PersonImage;
    public string Text;
}

public class cutscene : MonoBehaviour
{

    public GameObject dialogueBox;
    public Image person;

    public Movement movement;

    public TMP_Text text;

    public Animator animator;

    public DialogueOrder[] dialogueOrder;

    public int currentDialogue = 0;

    public bool skipPressed = false;

    public bool canSkip = true;

    public int dialogueLength;

    public bool cutsceneEnabled = false;

    void Start() {
        animator.SetBool("Open", false);
        dialogueLength = dialogueOrder.Length;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) skipPressed = true;
        if (Input.GetKeyUp(KeyCode.Space)) skipPressed = false;

        if (!skipPressed) {
            canSkip = true;
        }

        if (skipPressed && canSkip && cutsceneEnabled) {
            if (currentDialogue < dialogueLength - 1) {
                canSkip = false;
                animator.SetTrigger("Refresh");
                currentDialogue += 1;
                person.sprite = dialogueOrder[currentDialogue].PersonImage;
                text.text = dialogueOrder[currentDialogue].Text;
                movement.canMove = false;
                animator.ResetTrigger("Refresh");
            } else {
                animator.SetBool("Open", false);
                cutsceneEnabled = false;
                movement.canMove = true;
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7) {
            cutsceneEnabled = true;
            canSkip = false;
            animator.SetBool("Open", false);
            animator.SetBool("Open", true);
            person.sprite = dialogueOrder[currentDialogue].PersonImage;
            text.text = dialogueOrder[currentDialogue].Text;
            movement.canMove = false;
        }
    }
}