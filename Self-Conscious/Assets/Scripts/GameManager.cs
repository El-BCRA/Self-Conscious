using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SelfConscious
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Image blackScreen;
        public static GameManager Instance;

        private InputAction resetAction;
        private InputAction quitAction;

        private void Awake()
        {
            Cursor.visible = false;
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else if (Instance != this)
            {
                Destroy(this);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            resetAction = InputSystem.actions.FindAction("Reset");
            quitAction = InputSystem.actions.FindAction("Quit");
        }

        // Update is called once per frame
        void Update()
        {
            if (resetAction.WasPressedThisFrame())
            {
                SceneManager.LoadScene("TitleScreen");
            } if (quitAction.WasPressedThisFrame())
            {
                Application.Quit();
            }
        }


        public void StartGame()
        {
            FadeToBlack("IntroCutscene", 3.5f);
        }

        public void LoadScene(string sceneName, float pauseTime)
        {
            FadeToBlack(sceneName, pauseTime);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        void FadeToBlack(string nextScene, float pauseTime)
        {
            StartCoroutine(FadeOut(nextScene, pauseTime));
        }

        void FadeInFromBlack()
        {
            StartCoroutine(FadeIn());
        }

        IEnumerator FadeOut(string nextScene, float pauseTime)
        {
            while (blackScreen.color.a < 1.0f)
            {
                blackScreen.color = new Color(0, 0, 0, blackScreen.color.a + .02f);
                yield return null;
            }
            yield return (new WaitForSeconds(pauseTime));
            SceneManager.LoadScene(nextScene);
            yield return (new WaitForSeconds(.5f));
            FadeInFromBlack();
        }

        IEnumerator FadeIn()
        {
            while (blackScreen.color.a > 0f)
            {
                blackScreen.color = new Color(0, 0, 0, blackScreen.color.a - .02f);
                yield return null;
            }
        }
    }
}
