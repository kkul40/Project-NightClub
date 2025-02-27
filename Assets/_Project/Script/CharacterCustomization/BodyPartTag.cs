using UnityEngine;
using UnityEngine.Serialization;

namespace CharacterCustomization
{
    public enum BodyPartType
    {
        Torso_Hips = 0,
        Torso_Lower_Chest,
        Torso_Chest,
        Torso_Shoulders,
        Torso_Head,
        Torso_Neck,

        Arms_Lower = 20,
        Arms_Upper,
        Arms_Hand,

        Legs_Upper = 30,
        Legs_Knees,
        Legs_Lower,
        Legs_Feet,
    }

    public class BodyPartTag : MonoBehaviour
    {
        [FormerlySerializedAs("type")] public BodyPartType bodyTag;
    }
}