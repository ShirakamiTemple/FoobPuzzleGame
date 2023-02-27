//***
// Author: Nate
// Description: HelpBoxAttributeDrawer.cs is a property drawer for the custom helpbox attribute
//***

using FoxHerding.Attributes;
using UnityEditor;
using UnityEngine;

namespace FoxHerding.Editor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxAttributeDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            if (attribute is not HelpBoxAttribute helpBoxAttribute) return base.GetHeight();

            GUIStyle style = GUI.skin.GetStyle("HelpBox");
            float height = style.CalcHeight(new GUIContent(helpBoxAttribute.Message), EditorGUIUtility.currentViewWidth);
            return height + style.margin.top + style.margin.bottom;
        }

        public override void OnGUI(Rect position)
        {
            if (attribute is not HelpBoxAttribute helpBoxAttribute) return;

            GUIStyle style = GUI.skin.GetStyle("HelpBox");
            position.x += style.margin.left;
            position.width -= style.margin.left + style.margin.right;
            position = EditorGUI.IndentedRect(position);
            EditorGUI.HelpBox(position, helpBoxAttribute.Message, (MessageType)helpBoxAttribute.Type);
        }
    }
}