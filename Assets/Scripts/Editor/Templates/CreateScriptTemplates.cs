//***
// Author: Nate
// Description: CreateScriptTemplate.cs creates an asset menu to create scripts from templates
//***

using UnityEditor;

namespace FoxHerding.Editor.Templates
{
    public static class CreateScriptTemplates
    {
        [MenuItem("Assets/Create/Code/MonoBehavior", priority = 40)]
        public static void CreateMonoBehaviorMenuItem()
        {
            const string templatePath = "Assets/Scripts/Editor/Templates/MonoBehaviour.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewMono.cs");
        }
        
        [MenuItem("Assets/Create/Code/ScriptableObject", priority = 41)]
        public static void CreateScriptableObjectMenuItem()
        {
            const string templatePath = "Assets/Scripts/Editor/Templates/ScriptableObject.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewSO.cs");
        }
        
        [MenuItem("Assets/Create/Code/CustomAttribute", priority = 42)]
        public static void CreateAttributeMenuItem()
        {
            const string templatePath = "Assets/Scripts/Editor/Templates/Attribute.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewAttribute.cs");
        }
        
        [MenuItem("Assets/Create/Code/PropertyDrawer", priority = 43)]
        public static void CreatePropertyDrawerMenuItem()
        {
            const string templatePath = "Assets/Scripts/Editor/Templates/PropertyDrawer.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewDrawer.cs");
        }
        
        [MenuItem("Assets/Create/Code/CustomEditor", priority = 44)]
        public static void CreateEditorMenuItem()
        {
            const string templatePath = "Assets/Scripts/Editor/Templates/Editor.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewEditor.cs");
        }
    }
}