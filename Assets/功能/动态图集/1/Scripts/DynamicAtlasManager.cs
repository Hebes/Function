using System.Collections.Generic;
using UnityEngine;

public class DynamicAtlasManager
{
    public Vector2Int atlasSize = new Vector2Int(1024, 1024);

    private static DynamicAtlasManager _instance;
    public static DynamicAtlasManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DynamicAtlasManager();
            }
            return _instance;
        }
    }

    public List<DynamicAtlas> atlases = new List<DynamicAtlas>();
    public bool useMultipleAtlases = true;

    private DynamicAtlasManager() { }

    private DynamicAtlas CreateNewAtlas()
    {
        DynamicAtlas newAtlas = new DynamicAtlas(atlasSize.x, atlasSize.y);
        atlases.Add(newAtlas);
        return newAtlas;
    }

    public void AddSpriteToAtlas(Sprite sprite, out Texture2D newAtlasTexture, out Rect newSpriteRect, out Vector2 newSpritePivot)
    {
        newAtlasTexture = null;
        newSpriteRect = new Rect();
        newSpritePivot = Vector2.zero;

        DynamicAtlas targetAtlas = null;

        foreach (var atlas in atlases)
        {
            if (atlas.TryAddSprite(sprite, out newAtlasTexture, out newSpriteRect, out newSpritePivot))
            {
                targetAtlas = atlas;
                break;
            }
        }

        if (targetAtlas == null)
        {
            targetAtlas = CreateNewAtlas();
            if (targetAtlas.TryAddSprite(sprite, out newAtlasTexture, out newSpriteRect, out newSpritePivot))
            {
                if (useMultipleAtlases)
                {
                    atlases.Add(targetAtlas);
                }
            }
            else
            {
                Debug.LogError("Failed to add sprite to the atlas. Consider enabling useMultipleAtlases.");
            }
        }
    }
}

public class DynamicAtlas
{
    public Texture2D atlasTexture { get; private set; }
    private int nextX;
    private int nextY;
    private int maxHeight;

    public List<DynamicAtlasSprite> Sprites { get; private set; }

    public DynamicAtlas(int width, int height)
    {
        atlasTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        nextX = 0;
        nextY = 0;
        maxHeight = 0;

        Sprites = new List<DynamicAtlasSprite>();
    }

    public bool TryAddSprite(Sprite sprite, out Texture2D newAtlasTexture, out Rect newSpriteRect, out Vector2 newSpritePivot)
    {
        newAtlasTexture = null;
        newSpriteRect = new Rect();
        newSpritePivot = Vector2.zero;

        Texture2D originalTexture = sprite.texture;
        Rect originalRect = sprite.rect;
        Vector2 originalPivot = sprite.pivot;

        if (nextX + originalRect.width > atlasTexture.width)
        {
            nextX = 0;
            nextY += maxHeight;
            maxHeight = 0;

            if (nextY + originalRect.height > atlasTexture.height)
            {
                return false;
            }
        }

        newSpriteRect = new Rect(nextX, nextY, originalRect.width, originalRect.height);
        newSpritePivot = new Vector2(originalPivot.x / originalRect.width, originalPivot.y / originalRect.height);

        Graphics.CopyTexture(originalTexture, 0, 0, (int)originalRect.x, (int)originalRect.y, (int)originalRect.width,
            (int)originalRect.height, atlasTexture, 0, 0, (int)newSpriteRect.x, (int)newSpriteRect.y);

        nextX += (int)originalRect.width;

        if (originalRect.height > maxHeight)
        {
            maxHeight = (int)originalRect.height;
        }

        newAtlasTexture = atlasTexture;

        // Create a new DynamicAtlasSprite and add it to the Sprites list
        DynamicAtlasSprite newDynamicAtlasSprite = new DynamicAtlasSprite(sprite, newSpriteRect);
        Sprites.Add(newDynamicAtlasSprite);

        return true;
    }
}