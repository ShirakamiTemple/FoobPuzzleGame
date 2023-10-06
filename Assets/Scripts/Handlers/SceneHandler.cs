//***
// Author: Nate
// Description: SceneHandler is a Handler for managing scenes
//***

using System;
using System.Collections;
using FoxHerding.Generics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace FoxHerding.Handlers
{
    public enum SwipeDirection { Right, Left, Up, Down }
    
    public class SceneHandler : Handler<SceneHandler>
    {
        [SerializeField, Tooltip("This is the first scene that will load")] 
        private string firstSceneToLoad;
        [SerializeField, Tooltip("The duration of the transition in seconds")]
        private float transitionDuration = 0.5f;
        private readonly AnimationCurve _transitionCurve = new(
            new Keyframe(0, 0, 0, 2),
            new Keyframe(1, 1, 0.5f, 0)
        );
        private bool _isTransitioning;
        private Canvas _transitionCanvas;
        private float _canvasWidth;
        private float _canvasHeight;
        [SerializeField]
        private RectTransform transitionImageTransform;
        public delegate void SceneLoadedHandler(string sceneName);
        public event SceneLoadedHandler SceneLoaded;

        protected override void Awake()
        {
            base.Awake();
            if (string.IsNullOrEmpty(firstSceneToLoad)) return;

            _transitionCanvas = GetComponent<Canvas>();
            Rect rect = _transitionCanvas.GetComponent<RectTransform>().rect;
            _canvasWidth = rect.width;
            _canvasHeight = rect.height;
            
            SwitchSceneWithoutTransition(firstSceneToLoad);
        }

        public void SwitchScene(string scene)
        {
            if (!_isTransitioning)
            {
                StartCoroutine(TransitionToScene(scene));
            }
        }

        public void SwitchSceneWithoutTransition(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void ReloadCurrentScene()
        {
            if (!_isTransitioning)
            {
                StartCoroutine(TransitionToScene(SceneManager.GetActiveScene().name));
            }
        }
        
        public void ReloadCurrentSceneWithoutTransition()
        {
            if (_isTransitioning) return;
            
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadSceneAsync(sceneName).completed += _ =>
            {
                SceneLoaded?.Invoke(sceneName);
            };
        }
        
        private static SwipeDirection GetRandomSwipeDirection()
        {
            int randomIndex = Random.Range(0, 4);
            return (SwipeDirection)randomIndex;
        }

        private static SwipeDirection GetOppositeSwipeDirection(SwipeDirection direction)
        {
            return direction switch
            {
                SwipeDirection.Right => SwipeDirection.Left,
                SwipeDirection.Left => SwipeDirection.Right,
                SwipeDirection.Up => SwipeDirection.Down,
                SwipeDirection.Down => SwipeDirection.Up,
                _ => throw new ArgumentException("Invalid SwipeDirection value")
            };
        }

        private IEnumerator TransitionToScene(string scene)
        {
            _isTransitioning = true;
            // Start swipe transition
            SwipeDirection randomDirection = GetRandomSwipeDirection();
            AudioHandler.Instance.PlaySound(AudioHandler.Instance.SfxClips[0]);
            yield return StartCoroutine(BeginSwipeTransition(0, randomDirection));
            // Load new scene
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene);
            while (!loadOperation.isDone)
            {
                yield return null;
            }
            const float duration = 0.5f;
            float timer = 0;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            // Finish swipe transition
            AudioHandler.Instance.PlaySound(AudioHandler.Instance.SfxClips[0]);
            yield return StartCoroutine(EndSwipeTransition(GetOppositeSwipeDirection(randomDirection)));
            _isTransitioning = false;
            
            SceneLoaded?.Invoke(scene);
        }

        private IEnumerator BeginSwipeTransition(float newEndingPos, SwipeDirection randomDirection)
        {
            // Choose a random direction for the swipe to come from
            // Set the starting and ending positions based on the chosen direction
            Vector2 startingPos;
            Vector2 endingPos;
            switch (randomDirection)
            {
                case SwipeDirection.Right: // Right
                    startingPos = new Vector2(_canvasWidth, 0);
                    endingPos = new Vector2(newEndingPos, 0);
                    break;
                case SwipeDirection.Left: // Left
                    startingPos = new Vector2(-_canvasWidth, 0);
                    endingPos = new Vector2(newEndingPos, 0);
                    break;
                case SwipeDirection.Up: // Up
                    startingPos = new Vector2(0, _canvasHeight);
                    endingPos = new Vector2(0, newEndingPos);
                    break;
                case SwipeDirection.Down: // Down
                    startingPos = new Vector2(0, -_canvasHeight);
                    endingPos = new Vector2(0, newEndingPos);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(randomDirection), randomDirection, null);
            }
            transitionImageTransform.anchoredPosition = startingPos;
            //literally just choosing -30 or 30 degrees for rotation amount
            float rotationAmount = 0; //Random.Range(0, 2) == 0 ? -30f : 30f;
            transitionImageTransform.rotation = Quaternion.Euler(0f, 0f, rotationAmount);
            float timer = 0;
            while (timer < transitionDuration)
            {
                timer += Time.deltaTime;
                float progress = _transitionCurve.Evaluate(timer / transitionDuration);
                transitionImageTransform.anchoredPosition = Vector2.Lerp(startingPos, endingPos, progress);
                transitionImageTransform.rotation = Quaternion.Euler(0f, 0f, rotationAmount * (1 - progress));
                yield return null;
            }
            transitionImageTransform.anchoredPosition = endingPos;
            transitionImageTransform.rotation = Quaternion.identity;
        }

        private IEnumerator EndSwipeTransition(SwipeDirection randomDirection)
        {
            Vector2 startingPos = default;
            Vector2 endingPos;
    
            float screenRatio = (float)Screen.width / Screen.height;
            float imageWidth = transitionImageTransform.rect.width;
            float imageHeight = transitionImageTransform.rect.height;
    
            switch (randomDirection)
            {
                case SwipeDirection.Right: // Right
                    endingPos = new Vector2(Screen.width * screenRatio + imageWidth, 0);
                    break;
                case SwipeDirection.Left: // Left
                    endingPos = new Vector2(-Screen.width * screenRatio - imageWidth, 0);
                    break;
                case SwipeDirection.Up: // Up
                    endingPos = new Vector2(0, Screen.height * screenRatio + imageHeight);
                    break;
                case SwipeDirection.Down: // Down
                    endingPos = new Vector2(0, -Screen.height * screenRatio - imageHeight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(randomDirection), randomDirection, null);
            }
    
            transitionImageTransform.anchoredPosition = startingPos;
            float timer = 0;
    
            while (timer < transitionDuration)
            {
                timer += Time.deltaTime;
                float progress = _transitionCurve.Evaluate(timer / transitionDuration);
                transitionImageTransform.anchoredPosition = Vector2.Lerp(startingPos, endingPos, progress);
                yield return null;
            }
    
            transitionImageTransform.anchoredPosition = endingPos;
        }
    }
}