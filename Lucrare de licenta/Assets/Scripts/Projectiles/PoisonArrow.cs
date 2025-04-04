using UnityEngine;

public class PoisonArrow : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Arrow hit: " + collision.name);
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("impact");

        if (collision.tag == "FlyingEnemy")
        {
            collision.GetComponent<FlyingEnemyHealth>().TakeDamage(1);
            Debug.Log("Arrow hit the enemy!");
        }

        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(1);
            Debug.Log("Arrow hit the enemy!");
        }

        if (collision.CompareTag("Ground"))
        {
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("impact");
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossHealth>().TakeDamage(3f);
            Debug.Log("Arrow hit the boss!");
        }

    }
    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
