using System.Building_System.Controller;
using System.Building_System.Model;
using System.Building_System.Service;
using System.Building_System.View;
using Data;
using Framework.Context;
using Framework.Structures.Simple;
using UnityEngine;

namespace System.Building_System
{
    public class BuildingSystem : SimpleMiniMvcs<Context, BuildingModel, BuildingView, BuildingController, BuildingService>
    {
        [SerializeField] private BuildingView _buildingView;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private DiscoData _discoData;
        [SerializeField] private MaterialColorChanger _materialColorChanger;
        [SerializeField] private FXCreatorSystem fxCreatorSystem;
        [SerializeField] private TileIndicator _tileIndicator;

        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;

                //
                _context = new Context();
                // _context.ModelLocator.AddItem(new DummyModel(), "dummy");
                _model = new BuildingModel();
                _view = _buildingView;
                _service = new BuildingService();
                _controller = new BuildingController(_model, _view, _service, _inputSystem, _discoData, _materialColorChanger, fxCreatorSystem, _tileIndicator);
                //

                _model.Initialize(_context);
                _view.Initialize(_context);
                _service.Initialize(_context);
                _controller.Initialize(_context);
            }
        }

        private void Update()
        {
            RequireIsInitialized();

            if (!_isInitialized) return;
            
            _controller.Update(Time.deltaTime);
        }

        private void OnDisable()
        {
            _model.Dispose();
            _view.Dispose();
            _service.Dispose();
            _controller.Dispose();
        }
    }
}