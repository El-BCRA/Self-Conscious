using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace SelfConscious
{
    public class UIMenuButton : UIButton
    {
        [SerializeField] private string selectionDetailsText;
        [SerializeField] private TMP_Text choiceDescriptionText;
        [SerializeField] private Button button;
        [SerializeField] private bool isStart;
        [SerializeField] private bool isQuit;

        void Start()
        {
            base.Start();
            if (isStart)
            {
                button.onClick.AddListener(OnStartClicked);
            }

            if (isQuit)
            {
                button.onClick.AddListener(OnQuitClicked);
            }
        }

        public void OnStartClicked()
        {
            GameManager.Instance.StartGame();
        }

        public void OnQuitClicked()
        {
            GameManager.Instance.QuitGame();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            StartCoroutine(TextJitter());
            StartCoroutine(SelectionPulse());
            if (choiceDescriptionText != null)
            {
                choiceDescriptionText.text = selectionDetailsText;
            }
        }
    }
}
