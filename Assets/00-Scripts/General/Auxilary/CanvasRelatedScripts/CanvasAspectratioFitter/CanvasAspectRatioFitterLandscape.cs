using System.Linq;
using UnityEngine;

namespace BallsToCup.General.Popups
{
    public class CanvasAspectRatioFitterLandscape : CanvasAspectRatioFitter
    {
        #region Fields

      [SerializeField] AspectRatioData _criterionAspectRatio;

        #endregion

        #region Methods

        protected override void SetCanvasScalarMatch()
        {
            var orientationInfo =
                _criterionAspectRatio.orientationInfos.FirstOrDefault(i => i.orientation == Orientation.Landscape);
            if (orientationInfo == default)
                return;
            var currentRatio = CalcCurrentAspectRatio();
            var matchValue = currentRatio > orientationInfo.AspectRatio ? 1.0f : 0.0f;
            if (_canvasScaler != default)
            {
                _canvasScaler.matchWidthOrHeight = matchValue;
                return;
            }

            if (!TryGetComponent(out _canvasScaler))
                return;
            _canvasScaler.matchWidthOrHeight = matchValue;
        }

        #endregion
    }
}