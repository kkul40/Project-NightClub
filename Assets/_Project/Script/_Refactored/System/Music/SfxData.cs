using UnityEngine;

namespace System.Music
{
    public enum SoundFXType
    {
        Click,
        Success,
        Error,
        UIClick,
        UIBack,
        EmployeeSelection,
        InteractableSelection,
        BuildingSuccess,
        BuildingError,
        MoneyAdd,
        MoneyRemove,
        CameraFocus,
        CameraZoom,
    }
    
    [Serializable]
    public class SfxData
    {
        [Header("Sound FX")] 
        public AudioClip Click;
        public AudioClip Succes;
        public AudioClip Error;
        public AudioClip UIClick;
        public AudioClip UIBack;
        public AudioClip EmployeeSelection;
        public AudioClip InteractableSelection;
        public AudioClip BuildingSuccess;
        public AudioClip BuildingError;
        public AudioClip MoneyAdd;
        public AudioClip MoneyRemove;
        public AudioClip CameraFocus;
        public AudioClip CameraZoom;
        
        public AudioClip GetSfx(SoundFXType fxType)
        {
            switch (fxType)
            {
                case SoundFXType.Click:
                    return Click;
                case SoundFXType.Success:
                    return Succes;
                case SoundFXType.Error:
                    return Error;
                case SoundFXType.UIClick:
                    return UIClick;
                case SoundFXType.UIBack:
                    return UIBack;
                case SoundFXType.EmployeeSelection:
                    return EmployeeSelection;
                case SoundFXType.InteractableSelection:
                    return InteractableSelection;
                case SoundFXType.BuildingSuccess:
                    return BuildingSuccess;
                case SoundFXType.BuildingError:
                    return BuildingError;
                case SoundFXType.MoneyAdd:
                    return MoneyAdd;
                case SoundFXType.MoneyRemove:
                    return MoneyRemove;
                case SoundFXType.CameraFocus:
                    return CameraFocus;
                case SoundFXType.CameraZoom:
                    return CameraZoom;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fxType), fxType, null);
            }
        }
    }
}