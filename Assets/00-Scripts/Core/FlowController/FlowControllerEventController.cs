using System.Collections;
using System.Collections.Generic;
using BallsToCup.General;
using UnityEngine;

namespace BallsToCup.Core.Gameplay
{
    public class FlowControllerEventController : BaseEventController
    {
        public readonly ListEvent<bool> onEnableInput = new();
        public readonly ListEvent onGameStart = new();
    }
}
