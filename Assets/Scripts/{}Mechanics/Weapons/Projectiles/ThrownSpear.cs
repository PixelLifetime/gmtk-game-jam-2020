using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownSpear : MonoBehaviour
{
    public float speed;
    public LayerMask groundLayer;
    public LayerMask enemiesLayer;
    public Spear spear;
    public int damage;
    private Rigidbody2D _rigidbody2D;
    private bool _pickable;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = transform.right * transform.lossyScale.x * speed + transform.up * 2f;
        _rigidbody2D.angularVelocity = speed * -transform.lossyScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.velocity = Vector2.zero;
            _pickable = true;
            _rigidbody2D.freezeRotation = true;
        } 
        else if (_pickable)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                spear.PickUp();
                Destroy(gameObject);
            }
        } else if (((1 << collision.gameObject.layer) & enemiesLayer) != 0)
        {
            var damageable = collision.gameObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.Damage(damage, transform);
            }
        }
    }
}
