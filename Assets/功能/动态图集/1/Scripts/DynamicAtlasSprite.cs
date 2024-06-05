using UnityEngine;

public class DynamicAtlasSprite
{
    public Sprite SourceSprite { get; private set; }
    public Rect NewSpriteRect { get; private set; }

    public DynamicAtlasSprite(Sprite sourceSprite, Rect newSpriteRect)
    {
        SourceSprite = sourceSprite;
        NewSpriteRect = newSpriteRect;
    }
}
