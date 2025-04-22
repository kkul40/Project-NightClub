using TMPro;
using UnityEngine;

namespace TEST
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