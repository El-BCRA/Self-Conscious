using UnityEngine;
using System.Collections.Generic;

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
        [SerializeField] private List<Transform> playerBattlePositions = new List<Transform>();
        [SerializeField] private List<Transform> enemyBattlePositions = new List<Transform>();

        [SerializeField] private List<Unit> playerParty = new List<Unit>();
        [SerializeField] private List<Unit> enemyParty = new List<Unit>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            battleState = BattleState.START;

            InitializeBattle();
        }

        void InitializeBattle()
        {
            // Spawn in player and enemy units to their appropriate battle positions
        }

        public BattleState GetBattleState()
        {
            return battleState;
        }
    }
}
