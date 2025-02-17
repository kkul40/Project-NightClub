public interface ITool
{
    // bool isCanceld { get; }
    bool isFinished { get; }

    void OnStart(ToolHelper TH);
    bool OnValidate(ToolHelper TH);
    void OnUpdate(ToolHelper TH);
    void OnPlace(ToolHelper TH);
    void OnStop(ToolHelper TH);
}