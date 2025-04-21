using TMPro;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.TEST
{
    public class VersionText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI versionText;

        private void Awake()
        {
            versionText.text = "Alpha:" + Application.version;
        }
    }
}