using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BallsToCup.Core
{
    public class SwipeInput : MonoBehaviour,IDraggable
    {
        #region Fields

        private Camera _camera;
        public Action<Vector2> onDrag { get; set; }

        public Action<Vector2> onDragDelta { get; set; }
        #endregion

        #region Methods
        public void OnDrag(BaseEventData eventData)
        {
            if(Input.touches.Length==0)
                return;
            onDrag?.Invoke(Input.touches[0].position);
            onDragDelta?.Invoke(Input.touches[0].deltaPosition);
        }
   
        #endregion
    }
}

