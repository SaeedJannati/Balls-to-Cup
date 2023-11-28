using BallsToCup.General;
using UnityEngine;

namespace BallsToCup.Core
{
    public class TubeEventController:BaseEventController
    {
        public readonly ListFuncEvent<Vector3> onPivotTransformRequest = new();
        public readonly ListFuncEvent<(Vector3 min,Vector3 max)> onTubeBoundsRequest = new();

    }
}