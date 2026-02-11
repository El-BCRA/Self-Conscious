using UnityEngine;

namespace SelfConscious
{
    public class UIButton : MonoBehaviour
    {
        [SerializeField] private RectTransform position;
        [SerializeField] private RectTransform cursorPosition;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            position = GetComponent<RectTransform>();
        }

        public RectTransform GetCursorPosition()
        {
            return cursorPosition;
        }
    }
}
