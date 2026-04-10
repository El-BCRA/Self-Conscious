using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

namespace SelfConscious
{
    public enum BattleState
    {
        START,
        PLAYERTURN,
        ENEMYTURN,
        WIN,
        LOSE
    }

    public enum BattleUIFallback
    {
        MAIN,
        FIGHT,
        FIGHTTARGET,
        ITEMS,
        ITEMSTARGET,
        REPOSITION,
        FLEE,
    }

    public class BattleManager : MonoBehaviour
    {
        #region VARIABLES
        [Header("Current Battle State")]
        [SerializeField] private BattleState battleState;

        [Header("Party Battle Positions")]
        [SerializeField] private BattlePosition playerBPAttackFront;
        [SerializeField] private BattlePosition playerBPAttackBack;
        [SerializeField] private BattlePosition playerBPDefense;
        [SerializeField] private BattlePosition playerBPSupport;
        [SerializeField] private BattlePosition activeBP;

        [Header("Enemy Battle Positions")]
        [SerializeField] private List<Transform> enemyBattlePositions = new List<Transform>();

        [Header("UI")]
        [Tooltip("Any CanvasGroups that should only appear in certain contexts. Should include all " +
            "CanvasGroups which appear under this header.")]
        [SerializeField] private List<CanvasGroup> contextualUI;
        [SerializeField] private CanvasGroup battleSelections;
        [SerializeField] private GameObject defaultBSHighlight;
        [SerializeField] private CanvasGroup attackSelections;
        [SerializeField] private GameObject defaultASHighlight;
        [SerializeField] private List<UIAbilityButton> abilityButtons;
        [SerializeField] private CanvasGroup targetingAllAlliesSelection;
        [SerializeField] private GameObject highlightTAAS;
        [SerializeField] private CanvasGroup targetingAllEnemiesSelection;
        [SerializeField] private GameObject highlightTAES;
        [SerializeField] private CanvasGroup targetingAllUnitsSelection;
        [SerializeField] private GameObject highlightTAUS;
        [SerializeField] private List<CanvasGroup> playerUnitSelections;
        [SerializeField] private List<CanvasGroup> enemyUnitSelections;
        private BattleUIFallback fallbackLayer;
        private GameObject lastSelected;
        private InputAction cancelAction;

        [Header("Ability Select & Targeting")]
        [SerializeField] private AbilityData cachedAbility;
        [SerializeField] private List<Unit> cachedTargets;

        [Header("Units")]
        [SerializeField] private List<PlayerControlledUnit> playerParty = new List<PlayerControlledUnit>();
        [SerializeField] private List<Unit> enemyParty = new List<Unit>();

        [Header("Flags")]
        private bool selectionUIVisible = false;

        public static BattleManager Instance;
        #endregion

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this);
            }

            // DontDestroyOnLoad(this);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            cancelAction = InputSystem.actions.FindAction("Cancel");

            battleState = BattleState.START;

            foreach (CanvasGroup cg in contextualUI)
            {
                DeactivateCanvasGroup(cg);
            }

            StartCoroutine(InitializeBattle());
        }

        void Update()
        {
            if(cancelAction.WasPressedThisFrame())
            {
                UIMenuFallback();
            }
        }

        #region BATTLE STATE
        public BattleState GetBattleState()
        {
            return battleState;
        }

        public void ChangeBattleState (BattleState newState)
        {
            battleState = newState;

            switch (battleState)
            {
                case BattleState.PLAYERTURN:
                    {
                        StartCoroutine(PlayerTurn());
                        break;
                    }
                case BattleState.ENEMYTURN:
                    {
                        StartCoroutine(EnemyTurn());
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            // Any other state transition functionality that may need to happen later
        }

        // Cycle through the player turn in a predefined order
        public void NextBattlePosition()
        {
            activeBP.SetInactive();
            switch (activeBP.GetBPKind())
            {
                case BattlePositionKind.DEFENSE:
                    {
                        activeBP = playerBPAttackFront;
                        activeBP.SetActive();
                        break;
                    }
                case BattlePositionKind.ATTACKFRONT:
                    {
                        activeBP = playerBPSupport;
                        activeBP.SetActive();
                        break;
                    }
                case BattlePositionKind.SUPPORT:
                    {
                        activeBP = playerBPAttackBack;
                        activeBP.SetActive();
                        break;
                    }
                case BattlePositionKind.ATTACKBACK:
                    {
                        activeBP = playerBPDefense;
                        break;
                    }
            }
        }
        #endregion

        #region UI EVENTS
        public void OnAttackButton()
        {
            if (battleState != BattleState.PLAYERTURN || !selectionUIVisible)
            {
                return;
            }
            StartCoroutine(PlayerAttack());
        }

        public void OnItemsButton()
        {
            if (battleState != BattleState.PLAYERTURN || !selectionUIVisible)
            {
                return;
            }
            // StartCoroutine(PlayerAttack());
        }

        public void OnRepositionButton()
        {
            if (battleState != BattleState.PLAYERTURN || !selectionUIVisible)
            {
                return;
            }
            // StartCoroutine(PlayerAttack());
        }

        public void OnFleeButton()
        {
            if (battleState != BattleState.PLAYERTURN || !selectionUIVisible)
            {
                return;
            }
            // StartCoroutine(PlayerAttack());
        }

        public void OnAbilitySelect()
        {
            StartCoroutine(PlayerTargetSelect());
        }

        public void OnTargetConfirm()
        {
            lastSelected = null;
            StartCoroutine(PlayerTargetConfirm());
        }
        #endregion

        #region UI FUNCTIONS
        public void UIMenuFallback()
        {
            switch (fallbackLayer)
            {
                case (BattleUIFallback.MAIN):
                    {
                        // No layer to roll back
                        break;
                    }
                case (BattleUIFallback.FIGHT):
                    {
                        DeactivateCanvasGroup(attackSelections);
                        if (lastSelected is null)
                        {
                            ActivateCanvasGroup(battleSelections, defaultBSHighlight);
                        } else
                        {
                            ActivateCanvasGroup(battleSelections, lastSelected);
                        }
                        fallbackLayer = BattleUIFallback.MAIN;
                        lastSelected = null;
                        break;
                    }
                case (BattleUIFallback.FIGHTTARGET):
                    {
                        DeactivateTargetingUI();
                        ClearAbilityCache();
                        ActivateCanvasGroup(attackSelections, lastSelected);
                        fallbackLayer = BattleUIFallback.FIGHT;
                        lastSelected = null;
                        break;
                    }
            }
        }

        public void DeactivateTargetingUI()
        {
            switch (cachedAbility.targetingType)
            {
                case TargetingType.ENEMYSINGLE:
                    {
                        foreach (CanvasGroup cg in enemyUnitSelections)
                        {
                            DeactivateCanvasGroup(cg);
                        }
                        break;
                    }
                case TargetingType.ENEMYALL:
                    {
                        DeactivateCanvasGroup(targetingAllEnemiesSelection);
                        break;
                    }
                case TargetingType.ALLYSINGLE:
                    {
                        foreach (CanvasGroup cg in playerUnitSelections)
                        {
                            DeactivateCanvasGroup(cg);
                        }
                        break;
                    }
                case TargetingType.ALLYALL:
                    {
                        DeactivateCanvasGroup(targetingAllAlliesSelection);
                        break;
                    }
                case TargetingType.ALLUNITS:
                    {
                        DeactivateCanvasGroup(targetingAllUnitsSelection);
                        break;
                    }
                case TargetingType.SELF:
                    {
                        DeactivateCanvasGroup(activeBP.GetUnit().GetTargetingSelection());
                        break;
                    }
                case TargetingType.NONE:
                    {
                        // This shouldn't ever happen, this is a programming logic error
                        Debug.Log("Tried to deactivate a targeting selection on an ability with a " +
                            "TargetingType of NONE" + cachedAbility);
                        break;
                    }
            }
        }
        
        public void AddToAbilitiesUIList(UIAbilityButton abilityUI)
        {
            abilityButtons.Add(abilityUI);
        }

        public void RefreshAbilitiesUI()
        {
            for (int i = 0; i < abilityButtons.Count; i++)
            {
                switch (activeBP.GetBPKind())
                {
                    case (BattlePositionKind.ATTACKFRONT):
                        {
                            abilityButtons[i].SetAbility(activeBP.GetUnit().GetAttackAbilities()[i]);
                            break;
                        }
                    case (BattlePositionKind.ATTACKBACK):
                        {
                            abilityButtons[i].SetAbility(activeBP.GetUnit().GetAttackAbilities()[i]);
                            break;
                        }
                    case (BattlePositionKind.DEFENSE):
                        {
                            abilityButtons[i].SetAbility(activeBP.GetUnit().GetDefenseAbilities()[i]);
                            break;
                        }
                    case (BattlePositionKind.SUPPORT):
                        {
                            abilityButtons[i].SetAbility(activeBP.GetUnit().GetSupportAbilities()[i]);
                            break;
                        }
                }
                abilityButtons[i].ReplaceUIText();
                abilityButtons[i].ResetSelectionHighlight();
            }
        }
        #endregion

        #region HELPERS
        private void DeactivateCanvasGroup(CanvasGroup cg)
        {
            cg.interactable = false;
            cg.alpha = 0.0f;
        }

        private void ActivateCanvasGroup(CanvasGroup cg, GameObject button)
        {
            cg.interactable = true;
            cg.alpha = 1.0f;
            EventSystem.current.SetSelectedGameObject(button);
        }
        #endregion

        #region GETTERS & SETTERS
        public void CacheAbility(AbilityData data)
        {
            cachedAbility = data;
        }

        public void ClearAbilityCache()
        {
            cachedAbility = null;
        }
        #endregion

        #region COROUTINES
        IEnumerator InitializeBattle()
        {
            // Spawn in player and enemy units to their appropriate battle positions

            // TODO: Rather than existing in the scene already, should spawn new Units
            playerBPDefense.SetUnit(playerParty[0]);
            playerBPAttackFront.SetUnit(playerParty[1]);
            playerBPAttackBack.SetUnit(playerParty[2]);
            playerBPSupport.SetUnit(playerParty[3]);

            // TODO: Populate the targetingSelections CanvasGroup with all the targeting highlights
            // for the enemies currently on the battlefield

            // TODO: Determine turn order (if this is a speed-based system), switch to enemy
            // turn or player turn based on that
            activeBP = playerBPDefense;
            activeBP.SetActive();
            Debug.Log("Active selection highlight should be on");
            ChangeBattleState(BattleState.PLAYERTURN);

            yield return null;
        }

        // Turn on player interactables
        IEnumerator PlayerTurn()
        {
            ActivateCanvasGroup(battleSelections, defaultBSHighlight);
            selectionUIVisible = true;
            fallbackLayer = BattleUIFallback.MAIN;
            yield return null;
        }

        // Bring up ability selection screen
        IEnumerator PlayerAttack()
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
            RefreshAbilitiesUI();
            DeactivateCanvasGroup(battleSelections);
            ActivateCanvasGroup(attackSelections, defaultASHighlight);
            fallbackLayer = BattleUIFallback.FIGHT;
            yield return null;
        }

        // Select target for the currently selected ability
        IEnumerator PlayerTargetSelect()
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
            DeactivateCanvasGroup(attackSelections);
            switch (cachedAbility.targetingType)
            {
                case TargetingType.ENEMYSINGLE:
                    {
                        foreach (CanvasGroup cg in enemyUnitSelections)
                        {
                            ActivateCanvasGroup(cg, enemyParty[0].GetSelectionHighlight());
                        }
                        break;
                    }
                case TargetingType.ENEMYALL:
                    {
                        ActivateCanvasGroup(targetingAllEnemiesSelection, highlightTAES);
                        break;
                    }
                case TargetingType.ALLYSINGLE:
                    {
                        foreach (CanvasGroup cg in playerUnitSelections)
                        {
                            ActivateCanvasGroup(cg, activeBP.GetUnit().GetSelectionHighlight());
                        }
                        break;
                    }
                case TargetingType.ALLYALL:
                    {
                        ActivateCanvasGroup(targetingAllAlliesSelection, highlightTAAS);
                        break;
                    }
                case TargetingType.ALLUNITS:
                    {
                        ActivateCanvasGroup(targetingAllUnitsSelection, highlightTAUS);
                        break;
                    }
                case TargetingType.SELF:
                    {
                        ActivateCanvasGroup(activeBP.GetUnit().GetTargetingSelection(), 
                            activeBP.GetUnit().GetSelectionHighlight());
                        break;
                    }
                case TargetingType.NONE:
                    {
                        // This shouldn't ever happen, this is a programming logic error
                        Debug.Log("Tried to activate a targeting selection on an ability with a " +
                            "TargetingType of NONE" + cachedAbility);
                        break;
                    }
            }
            fallbackLayer = BattleUIFallback.FIGHTTARGET;
            yield return null;
        }

        // Ability and target confirmed, carry out damage
        IEnumerator PlayerTargetConfirm()
        {
            DeactivateTargetingUI();
            selectionUIVisible = false;
            StartCoroutine(AbilityActivate(cachedAbility, activeBP.GetUnit(), cachedTargets));
            yield return null;
        }

        IEnumerator AbilityActivate(AbilityData ability, Unit source, List<Unit> targets)
        {
            StartCoroutine(PlayerEndTurn());
            yield return null;
        }

        // Called once the last of the player's units has carried out their turn
        IEnumerator PlayerEndTurn()
        {
            NextBattlePosition();
            if (activeBP == playerBPDefense)
            {
                ChangeBattleState(BattleState.ENEMYTURN);
            }
            else
            {
                StartCoroutine(PlayerTurn());
            }
            yield return null;
        }

        IEnumerator EnemyTurn()
        {
            Debug.Log("The enemies are taking their turns.");
            yield return new WaitForSeconds(2f);
            ChangeBattleState(BattleState.PLAYERTURN);
        }
        #endregion
    }
}
