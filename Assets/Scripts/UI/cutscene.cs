using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

[System.Serializable]
public class DialogueOrder {
    public Sprite PersonImage;
    public string Text;
    public string Name;
    public bool Right;
}

public class cutscene : MonoBehaviour
{

    public GameObject dialogueBox;
    public Image person;

    public Movement movement;

    public TMP_Text text;

    public TMP_Text nameText;

    public Animator animator;

    public DialogueOrder[] dialogueOrder;

    public int currentDialogue = 0;

    public bool skipPressed = false;

    public bool canSkip = true;

    public int dialogueLength;

    public bool cutsceneEnabled = false;

    public float cutsceneSkipTime;
    public float cutsceneCooldownTime;

    public bool dialogueEnabled = true;

    void Start() {
        animator.SetBool("Open", false);
        dialogueLength = dialogueOrder.Length;
        cutsceneSkipTime = cutsceneCooldownTime + cutsceneSkipTime;
        StartCoroutine(cutsceneSkipCooldown(cutsceneSkipTime));
    }

    void Update() {
        if (dialogueEnabled) {
            if (Input.GetKeyDown(KeyCode.Space)) skipPressed = true;
            if (Input.GetKeyUp(KeyCode.Space)) skipPressed = false;

            if (skipPressed && canSkip && cutsceneEnabled) {
                if (currentDialogue < dialogueLength - 1) {
                    animator.SetBool("Open", false);
                    canSkip = false;
                    StartCoroutine(cutsceneCooldown(cutsceneCooldownTime));
                    StartCoroutine(cutsceneSkipCooldown(cutsceneSkipTime));

                } else {
                    animator.SetBool("Open", false);
                    cutsceneEnabled = false;
                    movement.canMove = true;
                    dialogueEnabled = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7) {
            if (dialogueEnabled) {
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
                animator.SetBool("Open", false);
                animator.SetBool("Open", true);
                person.sprite = dialogueOrder[currentDialogue].PersonImage;
                text.text = dialogueOrder[currentDialogue].Text;
                nameText.text = dialogueOrder[currentDialogue].Name;
                movement.canMove = false;
            }
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
    }
}