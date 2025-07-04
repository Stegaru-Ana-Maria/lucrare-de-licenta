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
        if (hit) return;

        SoundEffectManager.Play("Arrow");

        if (collision.tag == "FlyingEnemy")
        {
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("impact");
            collision.GetComponent<FlyingEnemyHealth>().TakeDamage(1);
            Debug.Log("Arrow hit the enemy!");
        }
        else if (collision.tag == "Enemy")
        {
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("impact");
            collision.GetComponent<EnemyHealth>().TakeDamage(1);
            Debug.Log("Arrow hit the enemy!");
        }
        else if (collision.CompareTag("Ground"))
        {
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("impact");
        }
        else if (collision.CompareTag("Shield"))
        {
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("impact");
            Debug.Log("Arrow hit the electric shield!");
            return;
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("impact");
            collision.gameObject.GetComponent<BossHealth>().TakeDamage(2.5f);
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
