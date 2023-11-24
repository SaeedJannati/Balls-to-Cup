using System;
using UnityEngine;

namespace BallsToCup.Core
{
    public interface IDraggable
    {
        public Action<Vector2> onDrag { get; set; }
        public Action<Vector2> onDragDelta{ get; set; }
    }
}