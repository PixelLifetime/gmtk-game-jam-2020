using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerTrigger : MonoBehaviour
{
    [SerializeField]
    private bool _destroyItself;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayer();
            if (_destroyItself)
            {
                Destroy(gameObject);
            }
        }
    }

    protected abstract void OnPlayer();
}
