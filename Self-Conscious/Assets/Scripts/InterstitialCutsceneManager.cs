using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace SelfConscious
{
    public class InterstitialCutsceneManager : MonoBehaviour
    {
        [Header("CG Stills")]
        public GameObject Line1;
        public GameObject Line2;
        public GameObject Line3;
        public GameObject Line4;
        public GameObject Line5;
        public GameObject Line6;
        private int cgCounter = 0;

        [Header("Continue Text")]
        public TMP_Text continueText;
        public float flashTimeMultiplier = 1f;
        public float continueTextDelay = 3f;

        [Header("Audio")]
        public AudioSource longScribble;
        public AudioSource engineRev;

        private IDisposable m_Eventlistener;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            cgCounter = 0;
            m_Eventlistener = InputSystem.onAnyButtonPress.Call(control => { OnButtonPressed(); });
            Line1.SetActive(true);
            Line2.SetActive(false);
            Line3.SetActive(false);
            Line4.SetActive(false);
            Line5.SetActive(false);
            Line6.SetActive(false);
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
                        Line1.SetActive(false);
                        Line2.SetActive(true);
                        StartCoroutine(ContinueText());
                        break;
                    }
                case 1:
                    {
                        // Debug.Log("Pressed second");
                        Line2.SetActive(false);
                        Line3.SetActive(true);
                        StartCoroutine(ContinueText());
                        break;
                    }
                case 2:
                    {
                        // Debug.Log("Pressed second");
                        Line3.SetActive(false);
                        Line4.SetActive(true);
                        StartCoroutine(ContinueText());
                        break;
                    }
                case 3:
                    {
                        // Debug.Log("Pressed second");
                        Line4.SetActive(false);
                        Line5.SetActive(true);
                        engineRev.Play();
                        StartCoroutine(ContinueText());
                        break;
                    }
                case 4:
                    {
                        // Debug.Log("Pressed second");
                        Line5.SetActive(false);
                        Line6.SetActive(true);
                        StartCoroutine(ContinueText());
                        break;
                    }
                case 5:
                    {
                        // Debug.Log("Pressed third");
                        GameManager.Instance.LoadScene("BattleScene2", 1.0f);
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
