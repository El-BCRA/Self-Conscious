using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SelfConscious {
    public class UISelectionManager : MonoBehaviour
    {
        [SerializeField] private Image selectionCursor;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            selectionCursor.color = new Color(1.0f, 1.0f, 1.0f, 0f);
        }

        // Update is called once per frame
        void Update()
        {
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected != null && BattleManager.instance.GetBattleState() == BattleState.PLAYERTURN)
            {
                selectionCursor.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                Vector2 targetPosition = currentSelected.GetComponent<UIButton>().GetCursorPosition().position;
                if (targetPosition != null)
                {
                    selectionCursor.rectTransform.position = targetPosition;
                } else
                {
                    selectionCursor.rectTransform.position = currentSelected.GetComponent<RectTransform>().position - new Vector3(50f,0f,0f);
                }
            } else
            {
                selectionCursor.color = new Color(1.0f, 1.0f, 1.0f, 0f);
            }
        }
    }
}
