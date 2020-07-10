using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public float chargedSpeed;
    public int damage;
    public float timeToLive;
    public bool isCharged;
    public LayerMask enemiesLayer;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyProjectile", timeToLive);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position +=
            transform.right * transform.lossyScale.x * (isCharged ? chargedSpeed : speed) * Time.deltaTime;
        var enemies = Physics2D.RaycastAll(transform.position, 
            transform.right * transform.lossyScale.x,
            0.5f,
            enemiesLayer);

        foreach (var enemy in enemies)
        {
           var damageable = enemy.transform.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.Damage(damage, transform);
                if (!isCharged) DestroyProjectile();
                break;
            }
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
