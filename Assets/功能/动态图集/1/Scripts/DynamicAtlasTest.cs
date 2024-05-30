using UnityEngine;
using UnityEngine.UI;

public class DynamicAtlasTest : MonoBehaviour
{
    public Sprite[] testSprites;
    public Image[] testImages;
    private RectTransform canvasRectTransform;

    private void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene. Please add a canvas to test the dynamic atlas.");
            return;
        }

        canvasRectTransform = canvas.GetComponent<RectTransform>();

        AddImageWithDynamicAtlas();
    }

    private void AddImageWithDynamicAtlas()
    {
        if (testImages.Length == 0 || testSprites.Length == 0)
        {
            Debug.LogError("Failed to add sprite to the dynamic atlas.");
            return;
        }
        for (int i = 0; i < testSprites.Length; i++)
        {
            DynamicAtlasManager.Instance.AddSpriteToAtlas(testSprites[i], out Texture2D newAtlasTexture, out Rect newSpriteRect, out Vector2 newSpritePivot);
            if (newAtlasTexture != null)
            {
                if (testImages[i])
                {
                    testImages[i].sprite = Sprite.Create(newAtlasTexture, newSpriteRect, newSpritePivot, testSprites[i].pixelsPerUnit);
                }
            }
            else
            {
                Debug.LogError("Failed to add sprite to the dynamic atlas.");
            }
        }
    }
}