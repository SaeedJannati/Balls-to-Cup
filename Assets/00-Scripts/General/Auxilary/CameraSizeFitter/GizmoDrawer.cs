using System;
using Unity.VisualScripting;
using UnityEngine;

namespace BallsToCup.General
{
    public class GizmoDrawer:MonoBehaviour
    {
        [SerializeField] public Color _gizmoColour=Color.red;
        [SerializeField] private GizmoPrimitive _primitive=GizmoPrimitive.Sphere;
        [SerializeField] private bool wireFrame=true;
        [SerializeField] private float _size = 1;
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColour;
            DrawGizmo();
        }
#endif


        void DrawGizmo()
        {
            if (_primitive == GizmoPrimitive.Sphere)
            {
                if (wireFrame)
                {
                    Gizmos.DrawWireSphere(transform.position,_size);
                    return;
                }
                Gizmos.DrawSphere(transform.position,_size);
                return;
            }

            if (wireFrame)
            {
                Gizmos.DrawWireCube(transform.position,Vector3.one*_size);
                return;
            }
            Gizmos.DrawCube(transform.position,Vector3.one*_size);
        }

        [Serializable]
        public enum GizmoPrimitive
        {
            Sphere,
            Cube
        }

       
    }
}