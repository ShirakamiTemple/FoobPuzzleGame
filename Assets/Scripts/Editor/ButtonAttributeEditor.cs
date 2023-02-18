//***
// Author: Nate
// Description: ButtonAttributeEditor.cs is a custom editor that draws a button in the inspector from the 
// custom Button attribute
//***

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FoxHerding.Attributes;
using UnityEditor;
using UnityEngine;

namespace FoxHerding.Editor
{
    public class ButtonAttributeEditor : UnityEditor.Editor
    {
        [CustomEditor(typeof(MonoBehaviour), true)]
        public class ButtonDrawer : UnityEditor.Editor
        {
            [UnityEditor.Callbacks.DidReloadScripts]
            private static void OnScriptsReloaded()
            {
                // Refresh all editor windows when scripts are reloaded, otherwise changes might not be reflected in 
                // the inspector until the user adjusts values on the inspector that forces a repaint.
                foreach (EditorWindow editorWindow in Resources.FindObjectsOfTypeAll<EditorWindow>())
                {
                    editorWindow.Repaint();
                }
            }
        
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                MonoBehaviour monoBehaviour = target as MonoBehaviour;
                if (monoBehaviour == null) return;
                IEnumerable<MethodInfo> methods = monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(info => Attribute.IsDefined(info, typeof(ButtonAttribute)));
                foreach (MethodInfo method in methods)
                {
                    // Get the ButtonAttribute instance for the method
                    ButtonAttribute buttonAttribute = (ButtonAttribute)method.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
                    // Use the label from the attribute if available, otherwise use the method name
                    string buttonText = string.IsNullOrEmpty(buttonAttribute.Label) ? method.Name : buttonAttribute.Label;
                    if (GUILayout.Button(buttonText))
                    {
                        method.Invoke(monoBehaviour, null);
                    }
                }
            }
        }
    }
}