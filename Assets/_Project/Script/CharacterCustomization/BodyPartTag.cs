using UnityEngine;
using UnityEngine.Serialization;

public enum BodyPartType
    {
        Torso_Hips = 0,
        Torso_Spine01,
        Torso_Spine02,
        Torso_Shoulders,
        Torso_Head,
        Torso_Spine03,

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
