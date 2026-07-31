using System.Collections.Generic;
using UnityEngine;
using BloodLine.Modules.Combat.Data;

namespace BloodLine.Modules.Combat.Simulation
{
    /// <summary>
    /// Runtime registry of all available moves.
    /// In a full game, this would parse ScriptableObjects into pure C# structs at Boot 
    /// for cache-friendly execution. For now, it holds direct references.
    /// </summary>
    public class MoveBank
    {
        private Dictionary<string, MoveDefinition> _moves = new Dictionary<string, MoveDefinition>();

        public void RegisterMove(MoveDefinition move)
        {
            if (move != null && !string.IsNullOrEmpty(move.MoveID))
            {
                _moves[move.MoveID] = move;
            }
        }

        public MoveDefinition GetMove(string moveID)
        {
            if (_moves.TryGetValue(moveID, out MoveDefinition move))
            {
                return move;
            }
            return null;
        }

        public bool TryGetMove(string moveID, out MoveDefinition move)
        {
            return _moves.TryGetValue(moveID, out move);
        }
    }
}
