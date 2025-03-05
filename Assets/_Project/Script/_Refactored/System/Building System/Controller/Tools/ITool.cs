namespace System.Building_System.Controller.Tools
{
    public interface ITool
    {
        // bool isCanceled { get; }
        bool isFinished { get; }

        void OnStart(ToolHelper TH);
        bool OnValidate(ToolHelper TH);
        void OnUpdate(ToolHelper TH);
        void OnPlace(ToolHelper TH);
        void OnStop(ToolHelper TH);
        bool CheckPlaceInput(ToolHelper TH);
    }
}