//***
// Author: Nate
// Description: PopupAttributeDrawer.cs is a property drawer for the custom popup attribute 
//***

using System;
using FoxHerding.Attributes;
using UnityEditor;
using UnityEngine;

namespace FoxHerding.Editor
{
    [CustomPropertyDrawer(typeof(PopupAttribute))]
    public class PopupAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                {
                    int selectedIndex = -1;
                    int[] optionValues = new int[property.enumNames.Length];
                    for (int i = 0; i < optionValues.Length; i++)
                    {
                        optionValues[i] = i;
                        if (property.enumValueIndex == i)
                        {
                            selectedIndex = i;
                        }
                    }
                    EditorGUI.BeginChangeCheck();
                    selectedIndex = EditorGUI.IntPopup(position, label.text, selectedIndex, property.enumNames, optionValues);
                    if (EditorGUI.EndChangeCheck())
                    {
                        property.enumValueIndex = selectedIndex;
                    }

                    break;
                }
                case SerializedPropertyType.Float:
                {
                    if (attribute is PopupAttribute popupAttribute)
                    {
                        object[] options = popupAttribute.Options;
                        string[] optionLabels = new string[options.Length];
                        for (int i = 0; i < options.Length; i++)
                        {
                            optionLabels[i] = options[i].ToString();
                        }
                        float value = property.floatValue;
                        int index = Mathf.Max(Array.IndexOf(options, value), 0);
                        EditorGUI.BeginChangeCheck();
                        index = EditorGUI.Popup(position, label.text, index, optionLabels);
                        if (EditorGUI.EndChangeCheck())
                        {
                            property.floatValue = (float)options[index];
                        }
                    }
                    break;
                }
                case SerializedPropertyType.String:
                {
                    if (attribute is PopupAttribute popupAttribute)
                    {
                        object[] options = popupAttribute.Options;
                        string[] optionLabels = new string[options.Length];
                        for (int i = 0; i < options.Length; i++)
                        {
                            optionLabels[i] = options[i].ToString();
                        }
                        string value = property.stringValue;
                        int index = Mathf.Max(Array.IndexOf(options, value), 0);
                        EditorGUI.BeginChangeCheck();
                        index = EditorGUI.Popup(position, label.text, index, optionLabels);
                        if (EditorGUI.EndChangeCheck())
                        {
                            property.stringValue = (string)options[index];
                        }
                    }
                    break;
                }
                default:
                    EditorGUI.LabelField(position, label.text, "PopupAttribute doesn't support the " + property.propertyType + " property type.");
                    break;
            }
        }
    }
}