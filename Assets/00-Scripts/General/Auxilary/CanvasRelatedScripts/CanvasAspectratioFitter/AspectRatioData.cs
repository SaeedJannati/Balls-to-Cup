using System;
using System.Collections.Generic;
using UnityEngine;

namespace BallsToCup.General.Popups
{
    public class AspectRatioData:ScriptableObject
    {
        public List<OrientationInfo> orientationInfos;
        
    }

    [Serializable]
    public enum Orientation
    {
        None,
        Landscape,
        Portrait,
        End
    }

    [Serializable]
    public class OrientationInfo
    {
        #region Fields
        
        public Orientation orientation;
        public float width;
        public float height;
        #endregion
    
        #region Properties
    
        public float AspectRatio
        {
            get
            {
                try
                {
                    return width / height;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return 1;
                }
            }
        }
    
        #endregion
    }
}