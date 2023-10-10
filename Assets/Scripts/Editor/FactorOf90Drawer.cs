//***
// Author: Nate
// Description: FactorOf90Drawer.cs is a custom property drawer that draws the slider for a factor of 90 attribute
// custom Button attribute
//***

using FoxHerding.Attributes;
using UnityEditor;
using UnityEngine;

namespace FoxHerding.Editor
{
    [CustomPropertyDrawer(typeof(FactorOf90Attribute))]
    public class FactorOf90Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get the value of the property
            float value = property.floatValue;

            // Round the value to the nearest factor of 90
            value = Mathf.Round(value / 90f) * 90f;

            // Clamp the value between 0 and 360
            value = Mathf.Clamp(value, 0f, 360f);

            // Update the property with the rounded value
            property.floatValue = value;

            // Draw the slider for the property field
            EditorGUI.Slider(position, property, 0f, 360f, label);
        }
    }
}
