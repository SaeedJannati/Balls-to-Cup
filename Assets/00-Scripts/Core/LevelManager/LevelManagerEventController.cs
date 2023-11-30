using BallsToCup.General;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BallsToCup.Core.Gameplay
{
    public class LevelManagerEventController:BaseEventController
    {
        public readonly ListFuncEvent<(float maxSensitivity, float moveThreshold)>
            onCurrentLevelRotateControllInfoRequest = new();
        public readonly ListFuncEvent<BallsToCupLevel>
            onCurrentLevelRequest = new();

        public ListEvent onCreateTubeRequest = new();
        public ListEvent onTubeCreated = new();
        public ListEvent onGenerateBallsRequest = new();
        public ListEvent onBallsGenerationComplete = new();
        public ListEvent onLevelGenerationComplete = new();
        public ListEvent onCameraReady = new();

    }
}