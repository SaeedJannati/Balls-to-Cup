using NaughtyAttributes;
using UnityEngine;

namespace  BallsToCup.General
{
    public class OrthoResponsiveCamera : MonoBehaviour
{
    #region Fields
    [SerializeField,Expandable] private OrthoCameraSizeModel _model;
    private Camera _camera;
    [SerializeField] private bool hasScalePreference;

    [Sirenix.OdinInspector.ShowIf(nameof(hasScalePreference)), SerializeField]
    private bool isHeightPreferred; 
    #endregion
     #region Monobehaviour callbacks
     private void Awake()
     {
         if (!TryGetComponent(out _camera))
             return;
         if(TryDoPreferredScaling())
             return;
         SetCameraSizeBasedOnScreenRatio();
     }
     #endregion

     #region Methods
     bool TryDoPreferredScaling()
     {
         if(!hasScalePreference)
             return false;
         if (isHeightPreferred)
         {
             ScaleHeightPreferred();
             return true;
         }
         ScaleWidthPreferred();
         return true;
     }

     void ScaleHeightPreferred()
     {
         var referenceRatio = _model.referenceWidth / _model.ReferenceHeight;
         var currentRatio = (float)Screen.width / Screen.height;
         var cameraCoefficient = referenceRatio / currentRatio;
         if (cameraCoefficient < 1)
             cameraCoefficient = 1;
         var destCamSize = _model.referenceCameraSize * cameraCoefficient;
         _camera.orthographicSize = destCamSize;
     }

     void ScaleWidthPreferred()
     {
         var referenceRatio = _model.referenceWidth / _model.ReferenceHeight;
         var currentRatio = (float)Screen.width / Screen.height;
         var cameraCoefficient = referenceRatio / currentRatio;
         if (cameraCoefficient> 1)
             cameraCoefficient = 1;
         var destCamSize = _model.referenceCameraSize * cameraCoefficient;
         _camera.orthographicSize = destCamSize;
     }

     [Sirenix.OdinInspector.Button]
     void SetCameraSizeBasedOnScreenRatio()
     {
         var referenceRatio = _model.referenceWidth / _model.ReferenceHeight;
         var currentRatio = (float)Screen.width / Screen.height;
         var destCamSize = _model.referenceCameraSize * referenceRatio / currentRatio;
         _camera.orthographicSize = destCamSize;
     }
     #endregion

   
}
}

