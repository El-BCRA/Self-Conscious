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

        [SerializeField] private List<Transform> enemyBattlePositions = new List<Transform>();

        [SerializeField] private List<Unit> playerParty = new List<Unit>();
        [SerializeField] private List<Unit> enemyParty = new List<Unit>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            battleState = BattleState.START;

            StartCoroutine(InitializeBattle());
        }

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

        public BattleState GetBattleState()
        {
            return battleState;
        }

        public void ChangeBattleState (BattleState newState)
        {
            battleState = newState;

            // Any other state transition functionality that may need to happen later
        }

        IEnumerator PlayerTurn()
        {
            yield return new WaitForSeconds(2f);
        }

        IEnumerator PlayerAttack()
        {
            // Target an enemy
            // Damage the targeted enemy
            yield return new WaitForSeconds(2f);
        }

        IEnumerator EnemyTurn()
        {
            yield return new WaitForSeconds(2f);
        }
    }
}
