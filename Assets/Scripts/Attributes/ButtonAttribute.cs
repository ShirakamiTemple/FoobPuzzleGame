//***
// Author: Nate
// Description: ButtonAttribute.cs is a custom Button Attribute for methods. It is drawn via a custom editor class
//***

using System;

namespace FoxHerding.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute
    {
        public readonly string Label;

        /// <summary>
        /// Creates a new instance of the ButtonAttribute without a label
        /// </summary>
        public ButtonAttribute()
        {
            Label = null;
        }

        /// <summary>
        /// Creates a new instance of the ButtonAttribute with a label
        /// </summary>
        /// <param name="label">The label to display on the button</param>
        public ButtonAttribute(string label)
        {
            Label = label;
        }
    }
}