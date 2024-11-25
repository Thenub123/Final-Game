using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PosChanger : MonoBehaviour
{
    public Transform child;
    public Transform parent;

    public bool x;
    public bool y;
    public bool z;

    void Update()
    {
        if(x) {
            child.position = new Vector3(parent.position.x, child.position.y, child.position.z);
        }

        if(y) {
            child.position = new Vector3(child.position.x, parent.position.y, child.position.z);
        }

        if(z) {
            child.position = new Vector3(child.position.x, child.position.y, parent.position.z);
        }
    }
}
