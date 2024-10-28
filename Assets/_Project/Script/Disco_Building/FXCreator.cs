using Unity.Mathematics;
using UnityEngine;

namespace BuildingSystem
{
    public enum FXType
    {
        Floor,
        Wall,
    }
    public class FXCreator : MonoBehaviour
    {
        [SerializeField] private GameObject _floorFX;
        [SerializeField] private GameObject _wallFX;

        public void CreateFX(FXType fxType, Vector3 position, Vector2 objectSize, Quaternion quaternion)
        {
            switch (fxType)
            {
                case FXType.Floor:
                    Create(_floorFX, position, objectSize, quaternion);
                    break;
                case FXType.Wall:
                    Create(_wallFX, position, objectSize, quaternion);
                    break;
                default:
                    Debug.Log("Not FX Imlemented for : " + fxType.ToString());
                    break;
            }
        }

        private GameObject Create(GameObject gameObject, Vector3 position, Vector2 objectSize, Quaternion quaternion)
        {
            var obj = Instantiate(gameObject, transform);
            // TODO Extension Method For Getinng center of bigger objects.
            obj.transform.localScale = new Vector3(objectSize.x, objectSize.y, objectSize.x);
            obj.transform.rotation = quaternion;
            obj.transform.position = position;

            if (obj.TryGetComponent(out ParticleSystem particleSystem))
            {
                Destroy(obj, particleSystem.main.duration);
            }

            return obj;
        }
    }
}