using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace SelfConscious
{
    public class SpriteJitterAnimation : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] List<Sprite> altImages;
        [SerializeField] private float jitterFrameDelay;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            StartCoroutine(Jitter());
        }

        private void OnDisable()
        {
            StopCoroutine(Jitter());
        }

        IEnumerator Jitter()
        {
            int i = 0;
            while (true)
            {
                image.sprite = altImages[i];
                i++;
                if (i >= altImages.Count)
                {
                    i = 0;
                }
                yield return new WaitForSeconds(jitterFrameDelay);
            }
        }
    }
}