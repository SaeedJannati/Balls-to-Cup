using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BallsToCup.General;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;

namespace BallsToCup.Meta.Levels
{
    public class LevelExtractor : MonoBehaviour
    {
        #region Fields

        [SerializeField] private int _levelIndex;
        [SerializeField] private List<TextAsset> _levels;
        [SerializeField] private GameObject _pointOnPath;
        [SerializeField] private Material _material;
        [SerializeField] private TubeComposite _tubeCompositePrefab;
        #endregion

        #region Methods

        [Button]
        void PrintRawLevelData()
        {
            BezierContour bezierContour = new();
            bool found = false;
            var scene = SVGParser.ImportSVG(new StringReader(_levels[_levelIndex].text));


            JsonSerializerSettings settings = new();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            BtcLogger.Log($"{JsonConvert.SerializeObject(scene, settings)}");
        }

        [Button]
        void ReadSVG()
        {
            BezierContour bezierContour = new();
            bool found = false;
            var scene = SVGParser.ImportSVG(new StringReader(_levels[_levelIndex].text));
            // bool isReversed = scene.SceneViewport.bottom > scene.SceneViewport.top;
            var couturs = scene.Scene.Root.Shapes?.Where(i => i.Contours.Length > 0).Select(j => j.Contours).ToList();
            if (couturs != default)
                foreach (var contur in couturs)
                {
                    var contourr = contur.ToList();
                    if (!contourr.Exists(i => i.Segments.Length > 0))
                        continue;
                    bezierContour = contourr.FirstOrDefault(i => i.Segments.Length > 0);
                    found = true;
                }

            if (!found)
            {
                foreach (var child in scene.Scene.Root.Children)
                {
                    if (!child.Shapes.Exists(i => i.Contours.Length > 0))
                        continue;
                    var shape = child.Shapes.FirstOrDefault(i => i.Contours.Length > 0);
                    if (shape == default)
                        continue;
                    var contorss = shape.Contours.ToList();
                    if (!contorss.Exists(i => i.Segments.Length > 0))
                        continue;
                    bezierContour = contorss.FirstOrDefault(i => i.Segments.Length > 0);
                    found = true;
                }
            }

            if (!found)
            {
                BtcLogger.Log("No path found");
                return;
            }

            // var contours = shape.Contours;
            // JsonSerializerSettings settings = new();
            // settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            // BtcLogger.Log($"{JsonConvert.SerializeObject(contours, settings)}");
            DrawPath(bezierContour);
        }

        void DrawPath(BezierContour contours)
        {
            transform.ClearChildren();
            BezierPathSegment segment;
            BezierPathSegment nextSegment;
            List<Vector3> pointsOnPath = new();
            for (int i = 0, e = contours.Segments.Length - 1; i < e; i++)
            {
                segment = contours.Segments[i];
                nextSegment = contours.Segments[i + 1];
                pointsOnPath =
                    pointsOnPath.Concat(GetWayPointsOnBezierCurve(segment.P0, segment.P1, segment.P2, nextSegment.P0))
                        .ToList();
                DrawBezierCurve(segment.P0, segment.P1, segment.P2, nextSegment.P0, 10, i);
            }

            if (pointsOnPath[^1].y < pointsOnPath[0].y)
                pointsOnPath.Reverse();
            int radialSegmentsCount = 8;
            var geometry = CreateTubeGeometryWithThickness(pointsOnPath.ToArray(), 8.0f,radialSegmentsCount ,2f);

            var triangles = CreateTopTriangles(geometry.triangles, geometry.vertices, radialSegmentsCount);
  
        var tube=    CreateTube(triangles, geometry.vertices, geometry.uvs,radialSegmentsCount);
        GameObject.Instantiate(_tubeCompositePrefab).AttachTube(tube);
        }

        public int[] CreateTopTriangles  (int[] origTriangles,Vector3[] vertices,int segments)
        {
            var triangles = new int[origTriangles.Length + 3*segments*2*2];
            for (int i = 0,e=origTriangles.Length;i < e; i++)
            {
                triangles[i] = origTriangles[i];
            }

            for (int i = 0; i < segments ; i++)
            {
                int vertIndex = vertices.Length / 2+i;
                triangles[^(i*6+1)] = vertIndex + 1;
                triangles[^(i*6+2)] = vertIndex;
                triangles[^(i*6+3)] = i; 
                
                triangles[^(i*6+4)] = i;
                triangles[^(i*6+5)] = i+1;
                triangles[^(i*6+6)] = vertIndex+1;   
            }

            for (int i = 0; i < segments ; i++)
            {
                int innerTube = vertices.Length / 2 - segments-1 + i;
                int vertIndex = vertices.Length -segments-1+i;
                
                triangles[^(i*6+1+6*segments)] = innerTube; 
                triangles[^(i*6+2+6*segments)] = vertIndex;
                triangles[^(i*6+3+6*segments)] = vertIndex + 1;
                triangles[^(i*6+4+6*segments)] = vertIndex+1; 
                triangles[^(i*6+5+6*segments)] = innerTube+1;  
                triangles[^(i*6+6+6*segments)] = innerTube;
            }
            return triangles;

        }

        List<Vector3> GetWayPointsOnBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int increments = 10)
        {
            List<Vector3> points = new();
            float deltaT = 1.0f / increments;
            for (int i = 0; i < increments; i++)
            {
                points.Add(GetPointOnBezierCurve(p0, p1, p2, p3, i * deltaT));
            }

            points.Add(GetPointOnBezierCurve(p0, p1, p2, p3, 1.0f));
            return points;
        }

        void DrawBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int increments, int segmentIndex)
        {
            var points = GetWayPointsOnBezierCurve(p0, p1, p2, p3, increments);
            for (int i = 0, e = points.Count; i < e; i++)
            {
                var obj = Instantiate(_pointOnPath, transform);
                obj.name = $"PointOnPath|segment:{segmentIndex},index:{i}";
                obj.transform.position = points[i];
            }
        }

        Vector2 GetPointOnBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            var oneMinus = 1.0f - t;
            return Mathf.Pow(oneMinus, 3) * p0
                   + 3 * Mathf.Pow(oneMinus, 2) * t * p1 + 3 * oneMinus * t * t * p2 + Mathf.Pow(t, 3) * p3;
        }

        public (int[] triangles, Vector3[] vertices, Vector2[] uvs) CreateTubeGeometryWithThickness(
            Vector3[] originalSplinePointArray, float tubeRadius,
            int radialSegmentsCount,float thickness)
        {
            var innerTubeGeometry = CalculateTubeGeometry(originalSplinePointArray, tubeRadius, radialSegmentsCount,true);
            var outerTubeGeometry=CalculateTubeGeometry(originalSplinePointArray, tubeRadius+thickness, radialSegmentsCount);
            var triangles = CombinationOfIntTriangles(innerTubeGeometry.triangles, outerTubeGeometry.triangles,
                innerTubeGeometry.vertices.Length);
            var vertices = innerTubeGeometry.vertices.Concat(outerTubeGeometry.vertices).ToArray();
            var uvs = innerTubeGeometry.uvs.Concat(outerTubeGeometry.uvs).ToArray();
            return (triangles, vertices, uvs);
        }

        public (int[] triangles,Vector3[] vertices,Vector2[] uvs) CalculateTubeGeometry(Vector3[] originalSplinePointArray, float tubeRadius,
            int radialSegmentsCount,bool isInnerTube=false)
        {
            var PI = Mathf.PI;
            var lengthOfSplinePointArray = originalSplinePointArray.Length;
           var splinePointArray = originalSplinePointArray;
           var splineDirectionalVectors = new Vector3[lengthOfSplinePointArray];
           var splineInterpolatedDirectionalVectors = new Vector3[lengthOfSplinePointArray];
           var splineVectorU = new Vector3[lengthOfSplinePointArray];
           var splineVectorV = new Vector3[lengthOfSplinePointArray];
            float[] distanceBetweenTwoPointsArray = new float[lengthOfSplinePointArray];

            splineDirectionalVectors[0] = (splinePointArray[1] - splinePointArray[0]).normalized;

            for (int k = 1; k < lengthOfSplinePointArray - 1; k++)
            {
                splineDirectionalVectors[k] = (splinePointArray[k + 1] - splinePointArray[k]).normalized;
            }

            splineDirectionalVectors[lengthOfSplinePointArray - 1] =
                (splinePointArray[lengthOfSplinePointArray - 1] -
                 splinePointArray[lengthOfSplinePointArray - 2]).normalized;
            distanceBetweenTwoPointsArray[0] = 0.0f;

            var wholeLegthOfSpline = 0.0f;
            for (int k = 1; k < lengthOfSplinePointArray; k++)
            {
                distanceBetweenTwoPointsArray[k] =
                    (splinePointArray[k] - splinePointArray[k - 1]).magnitude;
                wholeLegthOfSpline += distanceBetweenTwoPointsArray[k];
            }

            splineInterpolatedDirectionalVectors[0] = splineDirectionalVectors[0];
            for (int k = 1; k < lengthOfSplinePointArray - 1; k++)
            {
                splineInterpolatedDirectionalVectors[k] =
                    (splineDirectionalVectors[k] + splineDirectionalVectors[k - 1]).normalized;
            }

            splineInterpolatedDirectionalVectors[lengthOfSplinePointArray - 1] =
                splineDirectionalVectors[lengthOfSplinePointArray - 1];

            for (int k = 0; k < lengthOfSplinePointArray; k++)
            {
                Vector3 upstandingVector = new Vector3(0.0f, 1.0f, 0.0f);
                if (k > 0)
                {
                    upstandingVector = splineVectorU[k - 1];
                }
                else
                {
                    if (splineInterpolatedDirectionalVectors[0].normalized == upstandingVector.normalized)
                    {
                        //set it to something so cross product wont turn out to be vector3.zero
                        upstandingVector = new Vector3(1, 0, 0.0f);
                    }
                }

                splineVectorV[k] = Vector3.Cross(upstandingVector, splineInterpolatedDirectionalVectors[k])
                    .normalized;
                splineVectorU[k] = Vector3.Cross(splineInterpolatedDirectionalVectors[k], splineVectorV[k])
                    .normalized;
            }

            var verticesCurvedSurfaceArea =
                new Vector3[(radialSegmentsCount + 1) * lengthOfSplinePointArray];
            var uvCurvedSurfaceArea = new Vector2[verticesCurvedSurfaceArea.Length];
            var trianglesCurvedSurfaceArea =
                new int[radialSegmentsCount * (lengthOfSplinePointArray - 1) * 2 * 3];
            var currentAbsoluteLengthOfSplineInUVPosition = 0.0f;
            for (int m = 0; m < lengthOfSplinePointArray; m++)
            {
                currentAbsoluteLengthOfSplineInUVPosition += distanceBetweenTwoPointsArray[m];
                for (int p = 0; p < radialSegmentsCount + 1; p++)
                {
                    verticesCurvedSurfaceArea[m * (radialSegmentsCount + 1) + p] = splinePointArray[m]
                        + tubeRadius * Mathf.Cos(p * 2f * (PI / radialSegmentsCount)) * splineVectorU[m]
                        + tubeRadius * Mathf.Sin(p * 2f * (PI/ radialSegmentsCount)) * splineVectorV[m];
                    uvCurvedSurfaceArea[m * (radialSegmentsCount + 1) + p] = new Vector2(
                        .5f * p * (1f / (radialSegmentsCount + 1)),
                        currentAbsoluteLengthOfSplineInUVPosition);
                    if (!isInnerTube)
                    {
                        if (p < radialSegmentsCount && m < lengthOfSplinePointArray - 1)
                        {
                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6)] =
                                m * (radialSegmentsCount + 1) + p;
                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6) + 1] =
                                (m + 1) * (radialSegmentsCount + 1) + p;
                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6) + 2] =
                                (m) * (radialSegmentsCount + 1) + p + 1;

                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6) + 3] =
                                (m) * (radialSegmentsCount + 1) + p + 1;
                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6) + 4] =
                                (m + 1) * (radialSegmentsCount + 1) + p;
                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6) + 5] =
                                (m + 1) * (radialSegmentsCount + 1) + p + 1;
                        }
                    }
                    else
                    {
                        if (p < radialSegmentsCount && m < lengthOfSplinePointArray - 1)
                        {
                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6)] =
                              (m) * (radialSegmentsCount + 1) + p + 1;
                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6) + 1] =
                                (m + 1) * (radialSegmentsCount + 1) + p;
                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6) + 2] =
                                m * (radialSegmentsCount + 1) + p;

                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6) + 3] =
                                (m + 1) * (radialSegmentsCount + 1) + p + 1;
                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6) + 4] =
                                (m + 1) * (radialSegmentsCount + 1) + p;
                            trianglesCurvedSurfaceArea[m * (radialSegmentsCount * 6) + (p * 6) + 5] =
                                (m) * (radialSegmentsCount + 1) + p + 1;
                        }
                    }
                }
            }
            var trianglesAll = trianglesCurvedSurfaceArea;
            var verticesAll = verticesCurvedSurfaceArea;
            var uvAll = uvCurvedSurfaceArea;

            return (trianglesAll, verticesAll, uvAll);
        }

        GameObject CreateTube(int[] triangles,Vector3[] vertices,Vector2[] uvs,int radialSegmentsCount)
        {
            Mesh currentMesh;
            var currentTube = new GameObject("Tube");
            currentTube.AddComponent<MeshRenderer>();
            currentMesh = currentTube.AddComponent<MeshFilter>().mesh;
            currentMesh.Clear();

            currentMesh.vertices = vertices;
            currentMesh.triangles = triangles;
            currentMesh.uv = uvs;

           
            currentTube.GetComponent<MeshRenderer>().material = _material;
            currentMesh.RecalculateNormals();
            currentMesh.RecalculateBounds();
            currentTube.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            var tubeParent = new GameObject("TubeParent");
            var tubeParentPos = Vector3.zero;
            for (int i = 0; i < radialSegmentsCount; i++)
            {
                tubeParentPos += currentMesh.vertices[^(i+1)]/radialSegmentsCount;
            }
            tubeParent.transform.position = tubeParentPos;
            currentTube.transform.SetParent(tubeParent.transform);
            currentTube.AddComponent<MeshCollider>();
            return tubeParent;
        }

        private int[] CombinationOfIntTriangles(int[] first, int[] second, int offset)
        {
            int[] finalArray = new int[first.Length + second.Length];
            for (int i = 0; i < first.Length; i++)
            {
                finalArray[i] = first[i];
            }

            for (int i = 0; i < second.Length; i++)
            {
                finalArray[i + first.Length] = second[i] + offset;
            }

            return finalArray;
        }

        #endregion
    }
}





