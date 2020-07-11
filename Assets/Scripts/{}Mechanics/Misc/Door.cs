using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : PlayerTrigger
{
    private bool _isUnlocked;
    private void Start()
    {
        GameManager.Instance.OnKeyCollected.AddListener(UnlockDoor);
    }
    private void UnlockDoor()
    {
        _isUnlocked = true;
        var sprite = GetComponent<SpriteRenderer>();
        sprite.DOColor(Color.white, 1);
    }
    protected override void OnPlayer()
    {
        if (_isUnlocked)
        {
            GameManager.Instance.EndLevel();
        }
    }
}
