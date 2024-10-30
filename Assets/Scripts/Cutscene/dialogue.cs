using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CutsceneOrder {
    public Sprite PersonImage;
    public string Text;
}

public class dialogue : MonoBehaviour
{

    public GameObject dialogueBox;
    public Image person;

    public TMP_Text text;

    public Animator animator;

    public CutsceneOrder[] dialogueOrder;

    public int currentDialogue = 0;

    void Start() {
            animator.SetBool("Open", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7) {
            animator.SetBool("Open", true);
            person.sprite = dialogueOrder[0].PersonImage;
            text.text = dialogueOrder[0].Text;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 7) {
            animator.SetBool("Open", false);
        }
    }

}
