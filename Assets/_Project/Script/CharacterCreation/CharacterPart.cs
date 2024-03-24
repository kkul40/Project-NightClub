using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPart : MonoBehaviour
{
    [SerializeField] private Transform partHolder;
    [SerializeField] private List<GameObject> parts;

    private int currentPartIndex;

    private void Start()
    {
        ChangePart(parts[currentPartIndex]);
    }

    private void ChangePart(GameObject part)
    {
        for (int i = partHolder.childCount - 1; i >= 0; i--)
        {
            Debug.Log("Removed");
            var child = partHolder.GetChild(i).gameObject;
            Destroy(child);
        }

        if (part != null)
        {
            var newPart = Instantiate(part, partHolder);
        }
    }   

    public void OnLeftButton()
    {
        currentPartIndex--;
        if (currentPartIndex < 0)
            currentPartIndex = parts.Count - 1;
        
        ChangePart(parts[currentPartIndex]);
    }

    public void OnRightButton()
    {
        currentPartIndex++;
        if (currentPartIndex > parts.Count - 1)
            currentPartIndex = 0;
        
        ChangePart(parts[currentPartIndex]);
    }
}
