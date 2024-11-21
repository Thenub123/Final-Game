using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using Unity.VisualScripting;

[System.Serializable]
public class DialogueOrder {

    [Header("Dialogue Options")]
    public Sprite PersonImage;
    public string Text;
    public string Name;
    public bool Right;

    public float cutsceneSkipTime = 0.5f;
    public float cutsceneCooldownTime = 0.5f;

    [Header("Event Options")]

    public bool eventEnabled;

    public EventHandler EV;
}

public class cutscene : MonoBehaviour
{

    [Header("Dialogue")]
    public DialogueOrder[] dialogueOrder;

    [Header("References")]
    public GameObject dialogueBox;
    public Image person;

    public Movement movement;

    public TMP_Text text;

    public TMP_Text nameText;

    public Animator animator;

    private int currentDialogue = 0;

    private bool skipPressed = false;

    private bool canSkip = true;

    private int dialogueLength;

    private bool cutsceneEnabled = false;

    private bool dialogueEnabled = true;

    private void Start() {
        animator.SetBool("Open", false);
        dialogueLength = dialogueOrder.Length;
        dialogueOrder[currentDialogue].cutsceneSkipTime = dialogueOrder[currentDialogue].cutsceneCooldownTime + dialogueOrder[currentDialogue].cutsceneSkipTime;
        StartCoroutine(cutsceneSkipCooldown(dialogueOrder[currentDialogue].cutsceneSkipTime));
    }

    private void Update() {
        DialogueController();
    }

    private void DialogueController() {
        if (dialogueEnabled && cutsceneEnabled) {
            if (Input.GetKeyDown(KeyCode.Space)) skipPressed = true;
            if (Input.GetKeyUp(KeyCode.Space)) skipPressed = false;
            if (skipPressed && canSkip) {
                if (currentDialogue < dialogueLength - 1) {
                    animator.SetBool("Open", false);
                    canSkip = false;
                    StartCoroutine(cutsceneCooldown(dialogueOrder[currentDialogue].cutsceneCooldownTime));
                    StartCoroutine(cutsceneSkipCooldown(dialogueOrder[currentDialogue].cutsceneSkipTime));
                } else {
                    
                    animator.SetBool("Open", false);
                    cutsceneEnabled = false;
                    movement.canMove = true;
                    dialogueEnabled = false;
                }
            }

            eventController();
        } else {
            if(dialogueOrder[currentDialogue].EV != null) dialogueOrder[currentDialogue].EV.cutsceneEnabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7) {
            if (dialogueEnabled) {
                movement.horizontalPressed = 0;
                if (dialogueOrder[currentDialogue].Right == true) {
                    dialogueBox.transform.GetChild(1).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(287, -384);
                    dialogueBox.transform.GetChild(1).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 180, 0);
                    dialogueBox.transform.GetChild(2).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-91, -381);
                    dialogueBox.transform.GetChild(3).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-91, -306);
                } else {
                    dialogueBox.transform.GetChild(1).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-287, -384);
                    dialogueBox.transform.GetChild(1).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                    dialogueBox.transform.GetChild(2).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(91, -381);
                    dialogueBox.transform.GetChild(3).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(91, -306);
                }
                cutsceneEnabled = true;
                if (skipPressed) {
                    skipPressed = false;
                }
                canSkip = true;
                animator.SetBool("Open", true);
                person.sprite = dialogueOrder[currentDialogue].PersonImage;
                text.text = dialogueOrder[currentDialogue].Text;
                nameText.text = dialogueOrder[currentDialogue].Name;


                movement.canMove = false;

                DialogueController();
                eventController();
            }
        }
    }

    private void eventController() {
        if(dialogueOrder[currentDialogue].eventEnabled) {
            dialogueOrder[currentDialogue].EV.cutsceneEnabled = true;
        }
    }

    public IEnumerator cutsceneCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("Open", true);
        currentDialogue += 1;
        person.sprite = dialogueOrder[currentDialogue].PersonImage;
        text.text = dialogueOrder[currentDialogue].Text;
        nameText.text = dialogueOrder[currentDialogue].Name;
        if (dialogueOrder[currentDialogue].Right == true) {
            dialogueBox.transform.GetChild(1).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(287, -384);
            dialogueBox.transform.GetChild(1).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 180, 0);
            dialogueBox.transform.GetChild(2).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-91, -381);
            dialogueBox.transform.GetChild(3).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-91, -306);
        } else {
            dialogueBox.transform.GetChild(1).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-287, -384);
            dialogueBox.transform.GetChild(1).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            dialogueBox.transform.GetChild(2).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(91, -381);
            dialogueBox.transform.GetChild(3).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(91, -306);
        }
        movement.canMove = false;
    }

    public IEnumerator cutsceneSkipCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canSkip = true;
        if(currentDialogue > 0) {
            if(dialogueOrder[currentDialogue - 1].EV != null) dialogueOrder[currentDialogue - 1].EV.cutsceneEnabled = false;
        } 
    }
}