using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CMove {

    [Header("Move Player")]
    public bool isMove = false;
    public cutsceneMove moveCol;
}

[System.Serializable]
public class CAnim {

    [Header("Animation")]
    public bool isAnim = false;
    public Animator animator;
    public string animValueType;
    public string animValueName;
    public string animValue;

    public bool hasReturn;
    public string returnValueName;
}
[System.Serializable]
public class CActive {

    [Header("Set Active")]

    public bool isActive;
    public GameObject obj;
    public bool setActive;
}

[System.Serializable]
public class CZoom {

    [Header("Camera Zoom")]

    public bool isZoom;
    public float startZoom;
    public float endZoom;
    public Vector2 startPos;
    public Vector2 endPos;
    public float time;
    public RectTransform cam;
}

public class EventHandler : MonoBehaviour
{

    public bool cutsceneEnabled = false;

    [Header("Move Player")]

    public CMove moveFold;



    [Header("Animate")]

    public CAnim animFold;

    [Header("Active")]

    public CActive activeFold;

    [Header("Zoom")]
    public CZoom zoomFold;
    private Coroutine zoomAction;

    [Header("Other Reference")]
    public bool otherEVEnabled;
    public EventHandler otherEV;


    private void Update() {
        if (cutsceneEnabled){

            // Move

            if(moveFold.isMove) {
                moveFold.moveCol.moveEnabled = true;
                if(moveFold.moveCol.done) {
                    moveFold.moveCol.moveEnabled = false;
                }
            } else if (moveFold.moveCol != null) {
                moveFold.moveCol.moveEnabled = false;
            }

            //Animate

            if(animFold.isAnim) {
                if(animFold.animValueType == "int") {
                    animFold.animator.SetInteger(animFold.animValueName, int.Parse(animFold.animValue));
                } else if (animFold.animValueType == "bool") {
                    if (animFold.animValue == "true") animFold.animator.SetBool(animFold.animValueName, true);
                    else animFold.animator.SetBool(animFold.animValueName, false);
                } else {
                    animFold.animator.SetFloat(animFold.animValueName, float.Parse(animFold.animValue));
                }
            }

            //Active

            if(activeFold.isActive) {
                activeFold.obj.SetActive(activeFold.setActive);
            }

            //Zoom

            if(zoomFold.isZoom) {
                zoomAction = StartCoroutine(ZoomAction(zoomFold.startZoom, zoomFold.endZoom, zoomFold.startPos, zoomFold.endPos, zoomFold.time, zoomFold.cam));
                zoomFold.isZoom = false;
            }

            //Referencing other script

            if(otherEVEnabled && otherEV != null) {
                otherEV.cutsceneEnabled = true;
            }
        } else if(otherEV != null) {
            otherEV.cutsceneEnabled = false;
        }
    }

    private IEnumerator ZoomAction(float startZoom, float endZoom, Vector2 startPos, Vector2 endPos, float time, RectTransform cam) {
        Vector2 posLerp;
        float sizeAmount = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < time) {
            elapsedTime += Time.deltaTime;

            posLerp = new Vector2(Mathf.Lerp(startPos.x, endPos.x, (elapsedTime / time)), Mathf.Lerp(startPos.y, endPos.y, (elapsedTime / time)));
            sizeAmount = Mathf.Lerp(startZoom, endZoom, (elapsedTime / time));

            cam.localScale = new Vector2(sizeAmount, sizeAmount);
            cam.localPosition = posLerp;

            yield return null;
        }
    }

}

