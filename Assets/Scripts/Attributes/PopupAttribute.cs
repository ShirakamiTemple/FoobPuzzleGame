//***
// Author: Nate
// Description: PopupAttribute.cs is an attribute that allows for strings, floats, and ints to be popups in the inspector
//***

using System;
using UnityEngine;

namespace FoxHerding.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PopupAttribute : PropertyAttribute
    {
        public readonly object[] Options;

        public PopupAttribute(params object[] options)
        {
            Options = options;
        }
    }
}