using UnityEngine;
using UnityEngine.UI;using Zenject;

namespace  BallsToCup.General.Popups
{
    public abstract class CanvasAspectRatioFitter  : MonoBehaviour
    {
        #region Fields
        protected CanvasScaler _canvasScaler;

        #endregion

        #region Monobehaviour callbacks

        protected void Awake()
        {
            SetCanvasScalarMatch();
        }

        #endregion

        #region Methods

        protected abstract void SetCanvasScalarMatch();
        protected float CalcCurrentAspectRatio()
        {
            return (float)Screen.width / Screen.height;
        }

        #endregion
    }
}



