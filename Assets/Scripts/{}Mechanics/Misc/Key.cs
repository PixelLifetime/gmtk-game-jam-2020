using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PlayerTrigger
{
    protected override void OnPlayer()
    {
        GameManager.Instance.SetKeyCollected();
    }
}
