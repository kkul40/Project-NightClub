using Data;
using Disco_ScriptableObject;
using UnityEngine;

namespace System.Building.Controller.Tools
{
    public interface ITool
    {
        // bool isCanceled { get; }
        bool isFinished { get; }

        //                     storeItem   pos     rotation  
        void OnStart(ToolHelper TH);
        bool OnValidate(ToolHelper TH);
        void OnUpdate(ToolHelper TH);
        void OnPlace(ToolHelper TH);
        void OnStop(ToolHelper TH);
        bool CheckPlaceInput(ToolHelper TH);
    }

    public class IWallDoorPlacerTool : ITool
    {
        public bool isFinished { get; private set; }
        public void OnStart(ToolHelper TH)
        {
        }

        public bool OnValidate(ToolHelper TH)
        {
            return true;
        }

        public void OnUpdate(ToolHelper TH)
        {
        }

        public void OnPlace(ToolHelper TH)
        {
        }

        public void OnStop(ToolHelper TH)
        {
        }

        public bool CheckPlaceInput(ToolHelper TH)
        {
            return true;
        }
    }
}