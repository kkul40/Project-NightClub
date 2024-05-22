using System.Collections.Generic;
using UnityEngine;

// TODO Daha Sonra uidan kalitim alsin
namespace UI
{
    public class PropMenu : MonoBehaviour
    {
        [SerializeField] public List<GameObject> uiInventories;

        private void Start()
        {
            for (var i = 0; i < uiInventories.Count; i++) uiInventories[i].SetActive(false);
        }

        public void OpenUpInventory(GameObject inventory)
        {
            for (var i = 0; i < uiInventories.Count; i++) uiInventories[i].SetActive(false);

            inventory.SetActive(true);
        }
    }
}