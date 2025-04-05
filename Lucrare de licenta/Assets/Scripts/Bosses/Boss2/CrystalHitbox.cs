using UnityEngine;

public class CrystalHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 3f;
    [SerializeField] private float knockbackForce = 7f;
    [SerializeField] private LayerMask playerLayer;

    private bool hasDamaged = false;
    private float direction = 1f; 
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        hasDamaged = false;
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        hasDamaged = false;
        if (boxCollider != null)
            boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDamaged) return;

        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);

                Vector2 knockDir = (collision.transform.position - transform.position).normalized;
                playerHealth.ApplyKnockback(knockDir * knockbackForce);

                hasDamaged = true;
            }
        }
    }
}