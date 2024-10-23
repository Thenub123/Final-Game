using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOrder {
    public Texture2D Image;
    public string Text;
}

public class dialogue : MonoBehaviour
{

    public GameObject dialogueBox;

    public DialogueOrder[] dialogueOrder;

}
