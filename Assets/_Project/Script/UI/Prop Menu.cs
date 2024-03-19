using System;
using System.Collections.Generic;
using UnityEngine;

// TODO Daha Sonra uidan kalitim alsin
public class PropMenu : MonoBehaviour
{
    [SerializeField] public List<GameObject> uiInventories;

    private void Start()
    {
        for (int i = 0; i < uiInventories.Count; i++)
        {
            uiInventories[i].SetActive(false);
        }
    }

    public void OpenUpInventory(GameObject inventory)
    {
        for (int i = 0; i < uiInventories.Count; i++)
        {
            uiInventories[i].SetActive(false);
        }

        inventory.SetActive(true);
    }
}
