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
//         [HideInInspector] public cutsceneMove moveCol;

// }

// [CustomEditor(typeof(EventHandler))]

// public class EventEditor : Editor {

//     SerializedProperty moveCol;

//     private void OnEnable() {
//         moveCol = serializedObject.FindProperty("moveCol");
//     }
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();

//         EventHandler script = (EventHandler)target;

//         GUIContent arrayLabel = new GUIContent("EventArray");
//         script.arrayIdx = EditorGUILayout.Popup(arrayLabel, script.arrayIdx, script.EventArray);

//         if(script.arrayIdx == 0) {
//             EditorGUILayout.PropertyField(moveCol);
//         }

        

//     }
// }
