using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "New Player Customization Data")]
public class PlayerCustomizationDataSo : ScriptableObject
{
    [Tooltip("0 = Male, 1 = Female")]
    public List<Mesh> playerGenders;
    public List<GameObject> playerHairPrefabs;
    public List<GameObject> playerBeardPrefabs;
    public List<GameObject> playerAttachtmentPrefabs;
    public List<GameObject> playerEaringPrefabs;
}
