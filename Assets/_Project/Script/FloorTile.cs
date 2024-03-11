using System;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    [SerializeField] private bool isOccupied;
      
    public bool IsOccupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }

    private void Update()
    {
        var mesh = GetComponent<MeshRenderer>();
        if (isOccupied)
        {
            mesh.material.color = Color.yellow;
        }
        else
        {
            mesh.material.color = Color.white;
        }
    }
}
