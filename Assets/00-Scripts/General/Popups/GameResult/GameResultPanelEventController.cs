using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallsToCup.General.Popups
{
    public class GameResultPanelEventController : BaseEventController
    {
        public readonly ListEvent onHomeClick = new();
        public readonly ListEvent onRetryClick = new();
        public readonly ListEvent onNextLevelClick = new();
    }
}

