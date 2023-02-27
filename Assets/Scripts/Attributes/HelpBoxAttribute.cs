//***
// Author: Nate
// Description: HelBoxAttribute.cs is a custom attribute that will show helpboxes above properties
//***

using System;
using UnityEngine;

namespace FoxHerding.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class HelpBoxAttribute : PropertyAttribute
    {
        public string Message { get; private set; }
        public MessageType Type { get; private set; }

        public HelpBoxAttribute(string message, MessageType type = MessageType.Info)
        {
            Message = message;
            Type = type;
        }

        //unity editor already has the same enum as this but made a copy of it here so this can be decoupled from the
        //unity editor assembly when it the project is built.
        public enum MessageType
        {
            None,
            Info,
            Warning,
            Error,
        }
    }
}