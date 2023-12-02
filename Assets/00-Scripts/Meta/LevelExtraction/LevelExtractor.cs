using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BallsToCup.General;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace BallsToCup.Meta.Levels
{
    public class LevelExtractor : MonoBehaviour,IEventListener
    {
        #region Fields
        [Inject] private AddressableLoader _addressableLoader;
        [Inject] private LevelExtractorSvgParser.Factory _svgParserFactory;
        [Inject] private LevelExtractorEventController _eventController;
        [Inject] private LevelExtractorTubeCreator.Factory _tubeCreatorFactory;
        [SerializeField,Expandable] private LevelExtractorModel _model;
        [SerializeField]    private TextAsset _level;
        private LevelExtractorSvgParser _svgParser;
        private LevelExtractorTubeCreator _tubeCreator;
        #endregion

        #region Unity actions

        private void Start()
        {
            RegisterToEvents();
        }

        private void OnDestroy()
        {
            UnregisterFromEvents();
        }

        #endregion
        #region Methods

        [Inject]
        private void Initialise()
        {
            _svgParser = _svgParserFactory.Create(transform);
            _tubeCreator = _tubeCreatorFactory.Create();
        }

        [Sirenix.OdinInspector.Button]
        void CreateLevel()
        {
            CreateLevelTube(_level);
        }

        public async Task<TubeComposite> CreateLevelTube(TextAsset level)
        {
            var path = _svgParser.ParseLevelAndExtractPointsOnSpline(level);
          return  await _tubeCreator.CreateTube(path);
        }
        public void RegisterToEvents()
        {
          
        }

        public void UnregisterFromEvents()
        {
            
        }
        #endregion

   
    }
}