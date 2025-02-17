using Data;
using Disco_Building;
using DiscoSystem;
using RMC.Mini;
using UnityEngine;

public class BuildingSystem : SimpleMiniMvcs<Context, BuildingModel, BuildingView, BuildingController, BuildingService>
{
    [SerializeField] private BuildingView _buildingView;
    
    [SerializeField] private InputSystem _inputSystem;
    [SerializeField] private DiscoData _discoData;
    [SerializeField] private MaterialColorChanger _materialColorChanger;
    [SerializeField] private FXCreator _fxCreator;

    private void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        if (!IsInitialized)
        {
            _isInitialized = true;
            
            //
            _context = new Context();
            _model = new BuildingModel();
            _view = _buildingView;
            _service = new BuildingService();
            _controller = new BuildingController(_model, _view, _service, _inputSystem, _discoData, _materialColorChanger, _fxCreator);
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
        
        _controller.Update(Time.deltaTime);
    }
}