using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stacks sprites based on screen height
/// </summary>
public class scp_SpriteStacker : scp_Singleton<scp_SpriteStacker>
{
    List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    List<int[]> bufferLayers = new List<int[]>();

    // Update is called once per frame
    void Update()
    {
        RemoveNullSprites();
        SortSprites();
        SetSpriteSortingOrders();
    }

    // Called when a new scene is loaded
    void OnLevelWasLoaded(int level)
    {
        sprites = new List<SpriteRenderer>();
        bufferLayers = new List<int[]>();
    }

    /// <summary>
    /// Add a sprite to to the stacker
    /// </summary>
    /// <param name="sprite">The sprite renderer to stack</param>
    /// <param name="layersAboveSprite">Buffer layers above its sorting order.</param>
    /// <param name="layersBelowSprite">Buffer layers under its sorting order.</param>
    public void AddSprite(SpriteRenderer sprite, int layersAboveSprite, int layersBelowSprite)
    {
        int[] buffer = new int[2];

        buffer[0] = layersBelowSprite;
        buffer[1] = layersAboveSprite;

        sprites.Add(sprite);
        bufferLayers.Add(buffer);
    }

    // Remove any null sprite renderers (their game object may have been destroyed)
    void RemoveNullSprites()
    {
        for (int spriteIndex = 0; spriteIndex < sprites.Count; spriteIndex++)
        {
            if (sprites[spriteIndex] == null)
            {
                sprites.RemoveAt(spriteIndex);
                bufferLayers.RemoveAt(spriteIndex);
            }
        }
    }

    // Sets the sprites sorting orders
    void SetSpriteSortingOrders()
    {
        for (int spriteIndex = 0; spriteIndex < sprites.Count; spriteIndex++)
        {
            // Start at sorting order 0
            if (spriteIndex == 0)
            {
                sprites[spriteIndex].sortingOrder = 0;
            }
            else
            {
                // Set sorting order with buffer layers
                sprites[spriteIndex].sortingOrder = 1 + sprites[spriteIndex - 1].sortingOrder
                    + bufferLayers[spriteIndex - 1][1] + bufferLayers[spriteIndex][0];
            }
        }
    }

    // Sorts the sprites by screen height
    void SortSprites()
    {
        List<SpriteRenderer> newSprites = new List<SpriteRenderer>();
        List<int[]> newBuffer = new List<int[]>();
        List<float> spriteBottoms = new List<float>();

        SpriteRenderer sprite;
        int[] buffer;
        float spriteBottom;


        for (int spriteIndex = 0; spriteIndex < sprites.Count; spriteIndex++)
        {
            // Use the bottom of the sprite to determine height
            sprite = sprites[spriteIndex];
            buffer = bufferLayers[spriteIndex];
            spriteBottom = sprite.transform.position.y - sprite.sprite.textureRect.size.y / sprite.sprite.pixelsPerUnit / 2;

            // Find the appropriate index in the new list
            int insertIndex;
            bool found = false;
            for (insertIndex = 0; insertIndex < newSprites.Count && !found;)
            {
                if (spriteBottom > spriteBottoms[insertIndex])
                {
                    found = true;
                }
                else
                {
                    insertIndex++;
                }
            }

            // Insert into the new list
            newSprites.Insert(insertIndex, sprite);
            newBuffer.Insert(insertIndex, buffer);
            spriteBottoms.Insert(insertIndex, spriteBottom);
        }
        sprites = newSprites;
        bufferLayers = newBuffer;
    }
}
