#if UNITY_EDITOR
//***
// Author: Nate
// Description: QuickAccess adds a quick access menu for scene switching to the toolbar
//***

using UnityEditor;
using UnityEditor.SceneManagement;

public static class QuickAccess
{
    [MenuItem("QuickAccess/Scenes/Global", false, 1)]
    public static void OpenGlobalScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Global.unity");
    }
    [MenuItem("QuickAccess/Scenes/Splash", false, 2)]
    public static void OpenSplashScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/SplashScreen.unity");
    }
    [MenuItem("QuickAccess/Scenes/Main Menu", false, 3)]
    public static void OpenMainMenuScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
    }
    [MenuItem("QuickAccess/Scenes/Map Screen", false, 4)]
    public static void OpenMapScreenScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MapScreen.unity");
    }
}
#endif