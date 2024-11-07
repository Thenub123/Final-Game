// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.UI;

// public class EventHandler : MonoBehaviour
// {
//         [HideInInspector] public int arrayIdx = 0;
//         [HideInInspector] public string[] EventArray = new string[] {"Move", "Animate"};

//         [Header("Move")]
//         [HideInInspector] public int moveCol;

// }

// [CustomEditor(typeof(EventHandler))]

// public class EventEditor : Editor {
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();

//         EventHandler script = (EventHandler)target;

//         GUIContent arrayLabel = new GUIContent("EventArray");
//         script.arrayIdx = EditorGUILayout.Popup(arrayLabel, script.arrayIdx, script.EventArray);

//         if(script.arrayIdx == 0) {
//             script.moveCol = EditorGUILayout.IntField(label: "int", script.moveCol);
//         }

        

//     }
// }
