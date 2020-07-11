using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GripPlatform : GripEffect
{
    public new Collider2D collider2D;
    public Tilemap tilemap;
    
    protected override void DoEffect()
    {
        collider2D.enabled = false;
        tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, 0f);
    }

    protected override void UndoEffect()
    {
        collider2D.enabled = true;
        tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, 1f);
    }
}
