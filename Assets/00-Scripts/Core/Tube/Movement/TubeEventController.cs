using BallsToCup.General;
using UnityEngine;

namespace BallsToCup.Core
{
    public class TubeEventController:BaseEventController
    {
        public readonly ListFuncEvent<Vector3> onPivotTransformRequest = new();
    }
}