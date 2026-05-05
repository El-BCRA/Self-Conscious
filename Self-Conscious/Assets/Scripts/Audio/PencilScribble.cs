using UnityEngine;
using System.Collections.Generic;

namespace SelfConscious
{
    public class PencilScribble : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> pencilScribbles;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public AudioClip GetClip()
        {
            return pencilScribbles[Random.Range(0, pencilScribbles.Count)];
        }
    }
}
