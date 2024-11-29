using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
namespace JioGames.UI.Buttons
{
    [CustomEditor(typeof(Button))]
    public class JioButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button($"Replace With JioButton"))
            {
                ReplaceButton();
            }
        }

        private void ReplaceButton()
        {
            Button button = (Button)target;
            GameObject buttonGameobject = button.gameObject;
            DestroyImmediate(button);
            JioButton jioButton = buttonGameobject.AddComponent<JioButton>();
        }
    }
}

#endif