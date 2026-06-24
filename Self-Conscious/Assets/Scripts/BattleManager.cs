using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.EventSystems;
using TMPro;

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
        [Header("Audio")]
        [SerializeField] private AudioSource longScribble;

        [Header("Current Battle State")]
        [SerializeField] private BattleState battleState;

        [Header("Party Battle Positions")]
        [SerializeField] private BattlePosition activeBP;
        [SerializeField] private BattlePosition playerBPAttackFront;
        [SerializeField] private BattlePosition playerBPAttackBack;
        [SerializeField] private BattlePosition playerBPDefense;
        [SerializeField] private BattlePosition playerBPSupport;
        [SerializeField] private List<BattlePosition> battlePositions;

        [Header("UI")]
        [SerializeField] private CanvasGroup battleSelections;
        [SerializeField] private GameObject defaultBSHighlight;
        [SerializeField] private CanvasGroup attackSelections;
        [SerializeField] private GameObject defaultASHighlight;
        [SerializeField] private CanvasGroup targetingAllAlliesSelection;
        [SerializeField] private UITargetingButton allAlliesTB;
        [SerializeField] private GameObject highlightTAAS;
        [SerializeField] private CanvasGroup targetingAllEnemiesSelection;
        [SerializeField] private UITargetingButton allEnemiesTB;
        [SerializeField] private GameObject highlightTAES;
        [SerializeField] private CanvasGroup targetingAllUnitsSelection;
        [SerializeField] private UITargetingButton allUnitsTB;
        [SerializeField] private GameObject highlightTAUS;
        [SerializeField] private List<CanvasGroup> playerUnitSelections;
        [SerializeField] private List<CanvasGroup> enemyUnitSelections;
        [SerializeField] private List<UIRepositionButton> repositionButtons;
        [SerializeField] private List<UIAbilityButton> abilityButtons;
        [Tooltip("Any CanvasGroups that should only appear in certain contexts. Should include all " +
            "CanvasGroups which appear under this header.")]
        [SerializeField] private List<CanvasGroup> contextualUI;
        private BattleUIFallback fallbackLayer;
        private GameObject lastSelected;
        private InputAction cancelAction;

        [Header("Units")]
        [SerializeField] private List<PlayerControlledUnit> totalParty = new List<PlayerControlledUnit>();
        [SerializeField] private List<PlayerControlledUnit> playerParty = new List<PlayerControlledUnit>();
        [SerializeField] private List<EnemyUnit> enemyParty = new List<EnemyUnit>();

        [Header("Player Action Caches")]
        [SerializeField] private AbilityData cachedAbility;
        [SerializeField] private List<Unit> cachedTargets;
        [SerializeField] private AbilityClass cachedSwap;

        [Header("Sequence Canvases")]
        [SerializeField] private GameObject beginSequenceCanvas1;
        [SerializeField] private GameObject beginSequenceCanvas2;
        [SerializeField] private GameObject beginSequenceCanvas3;
        [SerializeField] private GameObject continueTextCanvas;
        [SerializeField] private GameObject endSequenceCanvas;

        [Header("Flags")]
        private bool TutorialCompleted = false;
        private bool selectionUIVisible = false;

        public static BattleManager Instance;
        #endregion

        private void Awake()
        {
            endSequenceCanvas.SetActive(false);
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
            beginSequenceCanvas1.SetActive(false);
            beginSequenceCanvas2.SetActive(false);
            beginSequenceCanvas3.SetActive(false);
            endSequenceCanvas.SetActive(false);
            continueTextCanvas.SetActive(true);

            cancelAction = InputSystem.actions.FindAction("Cancel");

            battleState = BattleState.START;

            StartCoroutine(InitializeBattle());
            if (tutorialTracker != 0)
            {
                ChangeBattleState(BattleState.PLAYERTURN);      
            } else
            {
                TutorialStart();
            }
        }

        void Update()
        {
            if(cancelAction.WasPressedThisFrame())
            {
                UIMenuFallback();
            }
        }

        #region BATTLE STATE FUNCTIONS
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

        #region GETTERS & SETTERS
        public void CacheAbility(AbilityData data)
        {
            cachedAbility = data;
        }

        public void ClearAbilityCache()
        {
            cachedAbility = null;
        }

        public void CacheTargets(List<Unit> targets)
        {
            cachedTargets.Clear();
            foreach (Unit u in targets)
            {
                cachedTargets.Add(u);
            }
        }

        public void CacheSwap(AbilityClass abilityClass)
        {
            cachedSwap = abilityClass;
        }
        #endregion

        #region HELPER FUNCTIONS
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
            StartCoroutine(PlayerReposition());
        }

        public void OnRepositionConfirm()
        {
            StartCoroutine(RepositionActivate());
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
                        activeBP.GetUnit().ShowName();
                        break;
                    }
                case (BattleUIFallback.FIGHTTARGET):
                    {
                        DeactivateTargetingUI();
                        ClearAbilityCache();
                        ActivateCanvasGroup(attackSelections, lastSelected);
                        fallbackLayer = BattleUIFallback.FIGHT;
                        lastSelected = null;
                        activeBP.GetUnit().ShowName();
                        break;
                    }
                case (BattleUIFallback.REPOSITION):
                    {
                        foreach (UIRepositionButton rs in repositionButtons)
                        {
                            DeactivateCanvasGroup(rs.GetCanvasGroup());
                        }
                        if (lastSelected is null)
                        {
                            ActivateCanvasGroup(battleSelections, defaultBSHighlight);
                        }
                        else
                        {
                            ActivateCanvasGroup(battleSelections, lastSelected);
                        }
                        fallbackLayer = BattleUIFallback.MAIN;
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

        public void AddToContextualUI(CanvasGroup cg)
        {
            contextualUI.Add(cg);
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

        #region UNIT FUNCITONS
        public void EnemyDefeat(EnemyUnit enemy)
        {
            // Remove the ability to target this enemy
            enemyUnitSelections.Remove(enemy.GetTargetingSelection());

            // Defeated enemies need to be manually removed from the 
            // List<Unit> targets for all targeting buttons
            allEnemiesTB.RemoveFromTargets(enemy);
            allUnitsTB.RemoveFromTargets(enemy);

            // Remove the enemy from the battle's list of actively tracked enemies
            enemyParty.Remove(enemy);

            // End the battle if no enemies remain
            if (enemyParty.Count <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(EndBattle());
            } else
            {

            }
        }
        #endregion

        #region TUTORIAL SEQUENCE
        [Header("Tutorial Continue Text")]
        [SerializeField] private TMP_Text continueText;
        [SerializeField] private float flashTimeMultiplier = 1f;
        [SerializeField] private float continueTextDelay = 1f;
        [SerializeField] private int tutorialTracker;
        private IDisposable m_Eventlistener;
        
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
            m_Eventlistener.Dispose();
            longScribble.Play();
            StopAllCoroutines();
            switch(tutorialTracker)
            {
                case 0:
                    {
                        beginSequenceCanvas1.SetActive(false);
                        beginSequenceCanvas2.SetActive(true);
                        StartCoroutine(ContinueText());
                        break;
                    }
                case 1:
                    {
                        beginSequenceCanvas2.SetActive(false);
                        beginSequenceCanvas3.SetActive(true);
                        StartCoroutine(ContinueText());
                        break;
                    }
                case 2:
                    {
                        beginSequenceCanvas3.SetActive(false);
                        continueTextCanvas.SetActive(false);
                        ChangeBattleState(BattleState.PLAYERTURN);
                        break;
                    }
            }
            tutorialTracker++;
        }

        public void TutorialStart()
        {
            beginSequenceCanvas1.SetActive(true);
            StartCoroutine(ContinueText());
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

            m_Eventlistener = InputSystem.onAnyButtonPress.Call(control => { OnButtonPressed(); });

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
        #endregion

        #region BATTLE FLOW
        IEnumerator InitializeBattle()
        {
            // Spawn in player and enemy units to their appropriate battle positions

            // TODO: Rather than existing in the scene already, should spawn new Units
            playerBPDefense.SetUnit(playerParty[0]);
            playerBPAttackFront.SetUnit(playerParty[1]);
            playerBPSupport.SetUnit(playerParty[2]);
            playerBPAttackBack.SetUnit(playerParty[3]);

            yield return new WaitForSeconds(0.25f);

            foreach (Unit unit in playerParty)
            {
                allAlliesTB.AddToTargets(unit);
                allUnitsTB.AddToTargets(unit);
                playerUnitSelections.Add(unit.GetTargetingSelection());
            }

            foreach (Unit unit in enemyParty)
            {
                allEnemiesTB.AddToTargets(unit);
                allUnitsTB.AddToTargets(unit);
                enemyUnitSelections.Add(unit.GetTargetingSelection());
            }

            yield return new WaitForSeconds(0.25f);

            foreach (UIRepositionButton rs in repositionButtons)
            {
                rs.UpdateBattleStationUI();
            }

            // TODO: Determine turn order (if this is a speed-based system), switch to enemy
            // turn or player turn based on that
            activeBP = playerBPDefense;
            activeBP.SetActive();
            yield return new WaitForSeconds(.25f);

            foreach (CanvasGroup cg in contextualUI)
            {
                DeactivateCanvasGroup(cg);
            }
        }

        // Turn on player interactables
        IEnumerator PlayerTurn()
        {
            ActivateCanvasGroup(battleSelections, defaultBSHighlight);
            activeBP.SetActive();
            selectionUIVisible = true;
            fallbackLayer = BattleUIFallback.MAIN;
            yield return null;
        }

        IEnumerator EndBattle()
        {
            endSequenceCanvas.SetActive(true);
            yield return null;
        }
        #endregion

        #region ABILITY FLOW
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
            StartCoroutine(PlayerAbilityActivate(cachedAbility, activeBP.GetUnit(), cachedTargets));
            yield return null;
        }

        // Noninteractive, apply ability affects to targets, play animations/SFX
        IEnumerator PlayerAbilityActivate(AbilityData ability, Unit source, List<Unit> targets)
        {
            Debug.Log(source.name + " ended their turn by using the ability " + ability.abilityName);

            source.UseAbility(ability);
            foreach(Unit target in targets)
            {
                target.ApplyAbility(ability, source);
                target.HideName();
            }
            foreach (BattlePosition bp in battlePositions)
            {
                bp.UpdateUI();
            }
            foreach(UIRepositionButton rs in repositionButtons)
            {
                rs.UpdateBattleStationUI();
            }
            yield return new WaitForSeconds(activeBP.GetUnit().GetHitAnimationTime());

            StartCoroutine(PlayerEndTurn());
            yield return null;
        }
        #endregion

        #region REPOSITION FLOW
        IEnumerator PlayerReposition()
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
            DeactivateCanvasGroup(battleSelections);
            foreach (UIRepositionButton rs in repositionButtons)
            {
                if (rs.GetAbilityClass() != activeBP.GetUnit().GetUnitClass())
                {
                    ActivateCanvasGroup(rs.GetCanvasGroup(), rs.gameObject);
                }
            }
            fallbackLayer = BattleUIFallback.REPOSITION;
            yield return null;
        }

        IEnumerator RepositionActivate()
        {
            // Deactivate all UI
            selectionUIVisible = false;
            fallbackLayer = BattleUIFallback.MAIN;
            foreach (UIRepositionButton rs in repositionButtons)
            {
                DeactivateCanvasGroup(rs.GetCanvasGroup());
                rs.UpdateBattleStationUI();
            }

            // Set current unit sprite as idle
            activeBP.GetUnit().SetIdle();
            activeBP.GetUnit().HideName();

            // Perform swap between two active party members
            int terminate = 0;
            foreach(BattlePosition bp in battlePositions)
            {
                if (bp.GetUnit().GetUnitClass() == cachedSwap)
                {
                    activeBP.SwapUnit(bp);
                    terminate = -1;
                    break;
                }
            }

            // Perform swap between active and reserve party member
            if (terminate == 0) 
            {
                PlayerControlledUnit swapIn = new PlayerControlledUnit();
                foreach (PlayerControlledUnit pcu in totalParty)
                {
                    if (pcu.GetUnitClass() == cachedSwap)
                    {
                        swapIn = pcu;
                        break;
                    }
                }
                playerParty.Remove(activeBP.GetUnit());
                activeBP.SetUnit(swapIn);
                playerParty.Add(swapIn);
            }

            // Final UI update check
            foreach (UIRepositionButton rs in repositionButtons)
            {
                rs.UpdateBattleStationUI();
            }
            activeBP.GetUnit().HideName();

            StartCoroutine(PlayerEndTurn());
            yield return null;
        }
        #endregion

        #region ENEMY FLOW
        // Called once the last of the player's units has carried out their turn
        IEnumerator PlayerEndTurn()
        {
            activeBP.GetUnit().HideName();
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
            yield return new WaitForSeconds(1f);
            foreach (EnemyUnit enemy in enemyParty)
            {
                Debug.Log(enemy.name + " is taking their turn.");
                
                // All enemy turn actions would be determined and executed here. 
                // For now, we'll just have them wait for a moment to simulate taking a turn.
                enemy.TickResourceMods();
                enemy.UpdateResourceModUI();

                AbilityData selectedAbility = enemy.GetWeightedRandomAbility();
                List<Unit> targets = new List<Unit>();
                switch (selectedAbility.targetingType)
                {                    
                    case TargetingType.ENEMYSINGLE:
                        {
                            targets.Add(enemy.SelectTarget(playerParty));
                        }
                        break;
                    case TargetingType.ENEMYALL:
                        {
                            targets.AddRange(playerParty);
                        }
                        break;
                    case TargetingType.ALLYSINGLE:
                        {
                            targets.Add(enemyParty[UnityEngine.Random.Range(0, enemyParty.Count)]);
                            break;
                        }
                    case TargetingType.ALLYALL:
                        {
                            targets.AddRange(enemyParty);
                            break;
                        }
                    case TargetingType.ALLUNITS:
                        {
                            targets.AddRange(playerParty);
                            targets.AddRange(enemyParty);
                            break;
                        }
                    case TargetingType.SELF:
                        {
                            targets.Add(enemy);
                            break;
                        }
                    case TargetingType.NONE:
                        {
                            // This shouldn't ever happen, this is a programming logic error
                            Debug.Log("Tried to select a target for an ability with a " +
                                "TargetingType of NONE" + selectedAbility);
                            break;
                        }
                }
                StartCoroutine(EnemyAbilityActivate(selectedAbility, enemy, targets));
                yield return new WaitForSeconds(enemy.GetHitAnimationTime());
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(2f);
            ChangeBattleState(BattleState.PLAYERTURN);
        }

        IEnumerator EnemyAbilityActivate(AbilityData ability, Unit source, List<Unit> targets)
        {
            Debug.Log(source.name + " used the ability " + ability.abilityName);
            
            source.UseAbility(ability);
            foreach (Unit target in targets)
            {
                target.ApplyAbility(ability, source);
                target.HideName();
            }
            foreach (BattlePosition bp in battlePositions)
            {
                bp.UpdateUI();
            }
            foreach (UIRepositionButton rs in repositionButtons)
            {
                rs.UpdateBattleStationUI();
            }
            yield return null;
        }
        #endregion
    }
}
