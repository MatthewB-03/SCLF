using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 
/// </summary>
public class scp_SpriteDisabler : scp_Singleton<scp_SpriteDisabler>
{
    public Camera activeCam;
    public SpriteRenderer[] sprites;

    public Vector2 bufferDistance;

    // Start is called before the first frame update
    void Start()
    {
        sprites = FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    // OnLevelWasLoaded is called when a new scene has been loaded
    private void OnLevelWasLoaded(int level)
    {
        sprites = FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        activeCam = FindFirstObjectByType<Camera>();

        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
            {
                if (IsOutOfRange(GetClosestCorner(sprite)))
                    sprite.enabled = false;
                else
                    sprite.enabled = true;
            }
        }
    }

    // Returns true if sprite is out of camera range
    bool IsOutOfRange(Vector2 corner)
    {
        if (corner.x > activeCam.transform.position.x + activeCam.scaledPixelWidth / 2 + bufferDistance.x)
            return true; // Out of range right

        else if (corner.x < activeCam.transform.position.x - activeCam.scaledPixelWidth / 2 - bufferDistance.x)
            return true; // Out of range left

        else if (corner.y > activeCam.transform.position.y + activeCam.scaledPixelHeight / 2 + bufferDistance.y)
            return true; // Out of range up

        else if (corner.y < activeCam.transform.position.y - activeCam.scaledPixelHeight / 2 - bufferDistance.y)
            return true; // Out of range down

        else
            return false; // In range
    }

    // Returns the closest corner to the camera
    Vector2 GetClosestCorner(SpriteRenderer sprite)
    {
        Vector3 UR = sprite.transform.position+sprite.bounds.extents;
        Vector3 UL = sprite.transform.position + new Vector3(-sprite.bounds.extents.x, sprite.bounds.extents.y);
        Vector3 LR = sprite.transform.position + new Vector3(sprite.bounds.extents.x, -sprite.bounds.extents.y);
        Vector3 LL = sprite.transform.position - sprite.bounds.extents;

        Vector3[] corners = new Vector3[3] { UL, LR, LL };
        Vector2 closest = UR;
        float closestDist = (activeCam.transform.position - UR).magnitude;

        foreach (Vector3 corner in corners)
        {
            float distance = (activeCam.transform.position - corner).magnitude;
            if (distance < closestDist)
            {
                closest = corner;
                closestDist = distance;
            }
        }

        return closest;
    }

    /*
    Vector2 GetClosestCorner(Vector3 cell, Tilemap tilemap)
    {
        Tile tile;
        Vector3 UR = tile.gameObject.transform.position + tile.sprite.bounds.extents;
        Vector3 UL = tile.gameObject.transform.position + new Vector3(-tile.sprite.bounds.extents.x, tile.sprite.bounds.extents.y);
        Vector3 LR = tile.gameObject.transform.position + new Vector3(tile.sprite.bounds.extents.x, -tile.sprite.bounds.extents.y);
        Vector3 LL = tile.gameObject.transform.position - tile.sprite.bounds.extents;

        Vector3[] corners = new Vector3[3] { UL, LR, LL };
        Vector2 closest = UR;
        float closestDist = (activeCam.transform.position - UR).magnitude;

        foreach (Vector3 corner in corners)
        {
            float distance = (activeCam.transform.position - corner).magnitude;
            if (distance < closestDist)
            {
                closest = corner;
                closestDist = distance;
            }
        }

        return closest;
    }
    */
}
