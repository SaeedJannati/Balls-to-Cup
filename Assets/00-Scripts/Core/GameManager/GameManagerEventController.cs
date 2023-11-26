using BallsToCup.General;
using UnityEngine;

namespace BallsToCup.Core.Gameplay
{
    public class GameManagerEventController : BaseEventController
    {
        public ListEvent onBallCreated = new();
        public ListEvent<bool> onBallGotOutOfTube = new();
        public ListEvent<bool> onBallTriggerdCupEdge = new();
        public ListEvent<int> onTotalBallsChange = new();
        public ListEvent<int> onBallsInCupChange = new();
    }
}
