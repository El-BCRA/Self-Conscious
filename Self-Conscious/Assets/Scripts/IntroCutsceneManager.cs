using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace SelfConscious
{
    public class IntroCutsceneManager : MonoBehaviour
    {
        [Header("CG Stills")]
        public GameObject CG1;
        public GameObject CG2;
        public GameObject CG3;
        private int cgCounter = 0;

        [Header("Continue Text")]
        public TMP_Text continueText;
        public float flashTimeMultiplier = 1f;
        public float continueTextDelay = 3f;

        [Header("Audio")]
        public AudioSource doorOpening;
        public AudioSource longScribble;

        private IDisposable m_Eventlistener;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            cgCounter = 0;
            m_Eventlistener = InputSystem.onAnyButtonPress.Call(control => { OnButtonPressed(); });
            CG1.SetActive(true);
            CG2.SetActive(false);
            CG3.SetActive(false);
            StartCoroutine(ContinueText());
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDisable()
        {
            m_Eventlistener.Dispose();
        }

        private void OnDestroy()
        {
            m_Eventlistener.Dispose();
        }

        public void OnButtonPressed()
        {
            longScribble.Play();
            StopAllCoroutines();
            switch(cgCounter)
            {
                case 0:
                    {
                        // Debug.Log("Pressed first");
                        CG1.SetActive(false);
                        CG2.SetActive(true);
                        doorOpening.Play();
                        StartCoroutine(ContinueText());
                        break;
                    }
                case 1:
                    {
                        // Debug.Log("Pressed second");
                        CG2.SetActive(false);
                        CG3.SetActive(true);
                        StartCoroutine(ContinueText());
                        break;
                    }
                case 2:
                    {
                        // Debug.Log("Pressed third");
                        GameManager.Instance.LoadScene("BattleScene1", 1.0f);
                        break;
                    }
                default:
                    {
                        // Debug.Log("Pressed past");
                        break;
                    }
            }
            cgCounter++;
        }

        IEnumerator ContinueText()
        {
            continueText.color = new Color(0, 0, 0, 0);
            yield return new WaitForSeconds(continueTextDelay);

            float timer = 0f;
            float newAlpha = 0;
            bool paused = false;
            while (newAlpha < .9f)
            {
                newAlpha = Mathf.Sin(timer * flashTimeMultiplier);
                continueText.color = new Color(0, 0, 0, newAlpha);
                timer += Time.deltaTime;
                yield return null;
            }

            while (true)
            {
                newAlpha = Mathf.Abs(Mathf.Sin(timer * flashTimeMultiplier));
                continueText.color = new Color(0, 0, 0, newAlpha);
                if (!paused && newAlpha >= .95f)
                {
                    paused = true;
                    yield return new WaitForSeconds(continueTextDelay);
                } else if (paused && newAlpha < .2f)
                {
                    paused = false;
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
