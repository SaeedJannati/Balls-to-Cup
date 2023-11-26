using UnityEngine;

namespace  BallsToCup.General.Popups
{
    public class CanvasCameraSetter : MonoBehaviour
    {
        #region Monobehaviour callbacks

        private void OnEnable()
        {
            TrySetCanvasCamera();
        }

        #endregion

        #region Methods
        void TrySetCanvasCamera()
        {
            if (!TryGetComponent(out Canvas canvas))
            {
                return;
            }
            canvas.worldCamera=Camera.main;
        }
        #endregion
}

}
