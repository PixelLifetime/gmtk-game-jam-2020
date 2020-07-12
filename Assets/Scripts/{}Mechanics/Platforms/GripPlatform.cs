using DG.Tweening;
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
        DOTween.ToAlpha(() => tilemap.color, x => tilemap.color = x, 0, 1);

    }

    protected override void UndoEffect()
    {
        collider2D.enabled = true;
        DOTween.ToAlpha(() => tilemap.color, x => tilemap.color = x, 1, 1);
    }
}
