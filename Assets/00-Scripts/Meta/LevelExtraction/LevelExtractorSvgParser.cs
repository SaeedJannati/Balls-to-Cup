using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BallsToCup.General;
using Newtonsoft.Json;
using Unity.VectorGraphics;
using UnityEngine;
using Zenject;

namespace BallsToCup.Meta.Levels
{
    public class LevelExtractorSvgParser
    {
        #region Fields

        private readonly Transform _transform;
        [Inject] private LevelExtractorModel _model;

        #endregion

        #region Constructors

        public LevelExtractorSvgParser(Transform transform, LevelExtractorModel model)
        {
            _transform = transform;
        }

        #endregion

        #region Methods

        public List<Vector3> ParseLevelAndExtractPointsOnSpline(TextAsset level)
        {
            var bezierContour = ReadSVGFile(level);
            return ExtractPathPoints(bezierContour);
        }

        void PrintRawLevelData(TextAsset level)
        {
            bool found = false;
            var scene = SVGParser.ImportSVG(new StringReader(level.text));


            JsonSerializerSettings settings = new();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            BtcLogger.Log($"{JsonConvert.SerializeObject(scene, settings)}");
        }

        BezierContour ReadSVGFile(TextAsset level)
        {
            BezierContour bezierContour = new();
            bool found = false;
            var scene = SVGParser.ImportSVG(new StringReader(level.text));
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
                throw new Exception("No path found");
            }

            return bezierContour;
        }

        List<Vector3> ExtractPathPoints(BezierContour contours)
        {
            _transform.ClearChildren();
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
            return pointsOnPath;
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
                var obj = GameObject.Instantiate(_model.pointOnPath, _transform);
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

        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<Transform, LevelExtractorSvgParser>
        {
        }

        #endregion
    }
}