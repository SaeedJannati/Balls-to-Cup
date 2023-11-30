using BallsToCup.General;
using UnityEngine;

namespace BallsToCup.Core.Gameplay
{
    public class GameManagerEventController : BaseEventController
    {
        public ListEvent onBallCreated = new();
        public ListEvent onBallGoBelowYCriterion = new();
        public ListEvent<bool> onBallTriggerCupEdge = new();
        public ListEvent<int> onTotalBallsChange = new();
        public ListEvent<int> onBallsInCupChange = new();
        public ListEvent onGameEnd = new();
        public ListEvent<int> onGameWon = new();
        public ListEvent onGameLose = new();
    }
}
