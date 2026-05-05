using UnityEngine;
using System.Collections;

namespace SelfConscious
{
    public class BGAudioPlayer : MonoBehaviour
    {
        public AudioSource myAudio;
        public float clipStartTime;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            myAudio.time = clipStartTime;

            myAudio.Play();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Fade()
        {
            StartCoroutine(FadeOut());
        }

        IEnumerator FadeOut()
        {
            while (myAudio.volume > 0)
            {
                myAudio.volume -= Time.deltaTime;
                yield return null;
            }
        }
    }
}