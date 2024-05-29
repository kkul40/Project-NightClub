using BuildingSystem.SO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StoreItemSO), true)]
public class StoreItemSOEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var storeItem = (StoreItemSO)target;

        if (storeItem.Icon == null || storeItem == null)
        {
            return null;
        }

        var texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(storeItem.Icon.texture,texture);
        return texture;
    }
}
