using UnityEngine;

namespace BallsToCup.General
{
    public static partial class BtcLogger
    {
        [HideInCallstack]
        public static void Log<T>(T log, int r, int g, int b)
        {
            if (!Debug.unityLogger.logEnabled)
                return;
            Log(log.ToString(), (float)r / 255, (float)g / 255, (float)b / 255);
        }

        [HideInCallstack]
        public static void Log<T>(T log, float r, float g, float b)
        {
            if (!Debug.unityLogger.logEnabled)
                return;
            Log(log.ToString(), new Color(r, g, b));
        }

        [HideInCallstack]
        public static void Log<T>(T log, Vector3 colour)
        {
            if (!Debug.unityLogger.logEnabled)
                return;
            Log(log.ToString(), colour.x, colour.y, colour.z);
        }

        [HideInCallstack]
        public static void Log<T>(T log, Vector3Int colour)
        {
            if (!Debug.unityLogger.logEnabled)
                return;
            Log(log.ToString(), colour.x, colour.y, colour.z);
        }

        [HideInCallstack]
        public static void Log<T>(T log, Color colour)
        {
            if (!Debug.unityLogger.logEnabled)
                return;
            Log(log.ToString(), colour.GetHexadecimal());
        }

        [HideInCallstack]
        public static void Log<T>(T log, string hexadecimalColour)
        {
            if (!Debug.unityLogger.logEnabled)
                return;
            var outPut = $"<color={hexadecimalColour}>{log.ToString()}</color>";

            Log(outPut);
        }

        [HideInCallstack]
        public static void Log<T>(T value)
        {
            if (!Debug.unityLogger.logEnabled)
                return;
            Debug.Log(value.ToString());
        }
    }
}