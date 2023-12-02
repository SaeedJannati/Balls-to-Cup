using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallsToCup.Meta.Levels
{
    public class TubeComposite : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Transform _tubeAttachTransform;

        #endregion

        #region Methods

        public void AttachTube(GameObject tube,float radius)
        {
            _tubeAttachTransform.localScale *= radius;
            var tubeTransform = tube.transform;
            tubeTransform.SetParent(_tubeAttachTransform);
            tubeTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            tubeTransform.localScale=Vector3.one;
        }


        #endregion
    }
}