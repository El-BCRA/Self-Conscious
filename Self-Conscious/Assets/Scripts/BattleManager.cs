using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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

    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private BattleState battleState;
        [SerializeField] private BattlePosition playerBPAttackFront;
        [SerializeField] private BattlePosition playerBPAttackBack;
        [SerializeField] private BattlePosition playerBPDefense;
        [SerializeField] private BattlePosition playerBPSupport;
        [SerializeField] private BattlePosition activeBP;

        [SerializeField] private List<Transform> enemyBattlePositions = new List<Transform>();

        private bool attacking = false;

        [SerializeField] private List<Unit> playerParty = new List<Unit>();
        [SerializeField] private List<Unit> enemyParty = new List<Unit>();

        public static BattleManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }

            // DontDestroyOnLoad(this);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            battleState = BattleState.START;

            StartCoroutine(InitializeBattle());
        }

        #region BATTLE STATE
        public BattleState GetBattleState()
        {
            return battleState;
        }

        public void ChangeBattleState (BattleState newState)
        {
            battleState = newState;

            switch(newState)
            {
                case BattleState.PLAYERTURN:
                    {
                        activeBP = playerBPDefense;
                        activeBP.SetActive();
                        StartCoroutine(PlayerTurn());
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            // Any other state transition functionality that may need to happen later
        }
        #endregion

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

        public void OnAttackButton()
        {
            if (battleState != BattleState.PLAYERTURN || attacking)
            {
                return;
            }

            StartCoroutine(PlayerAttack());
        }

        #region COROUTINES
        IEnumerator InitializeBattle()
        {
            // Spawn in player and enemy units to their appropriate battle positions
            // TODO: Rather than existing in the scene already, should spawn new Units
            playerBPDefense.SetUnit(playerParty[0]);
            playerBPAttackFront.SetUnit(playerParty[1]);
            playerBPAttackBack.SetUnit(playerParty[2]);
            playerBPSupport.SetUnit(playerParty[3]);

            // Battle screen startup delay
            yield return new WaitForSeconds(2f);

            // TODO: Determine turn order (if this is a speed-based system), switch to enemy
            // turn or player turn based on that
            ChangeBattleState(BattleState.PLAYERTURN);
        }

        // Allow player to cycle through menu options until an action has been confirmed
        IEnumerator PlayerTurn()
        {
            Debug.Log("Player turn has started");
            yield return new WaitForSeconds(2f);
        }

        // Allow player to select an ability and target an enemy
        IEnumerator PlayerAttack()
        {
            attacking = true;
            Debug.Log("" + activeBP.GetUnit().unitName + " is attacking.");

            Unit target = enemyParty[Random.Range(0, 3)];

            target.currentHP -= 2;

            // Target an enemy
            // Damage the targeted enemy
            yield return new WaitForSeconds(2f);

            Debug.Log("" + target.name + " took 2 damage.");

            NextBattlePosition();
            attacking = false;

            if (activeBP == playerBPDefense)
            {
                StartCoroutine(EnemyTurn());
            }
        }

        IEnumerator EnemyTurn()
        {
            yield return new WaitForSeconds(2f);
        }
        #endregion
    }
}
