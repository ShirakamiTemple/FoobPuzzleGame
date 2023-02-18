//***
// Author: Nate
// Description: SceneHandler is a Handler for managing scenes
//***

using FoxHerding.Generics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoxHerding.Handlers
{
    public class SceneHandler : Handler<SceneHandler>
    {
        [SerializeField, Tooltip("This is the first scene that will load")] 
        private string firstSceneToLoad;
    
        protected override void Awake()
        {
            base.Awake();
            if (string.IsNullOrEmpty(firstSceneToLoad)) return;
        
            SwitchScene(firstSceneToLoad);
        }

        public void SwitchScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void ReloadCurrentScene()
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
    }
}