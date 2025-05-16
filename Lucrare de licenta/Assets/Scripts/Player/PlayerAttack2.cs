using UnityEngine;

public class PlayerAttack2 : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private GameObject[] arrows;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    public bool attack2Unlocked = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        attack2Unlocked = GameSession.attack2Unlocked;
    }

    private void Update()
    {
        if (attack2Unlocked && Input.GetKeyDown(KeyCode.K) && cooldownTimer > attackCooldown && playerMovement.canAttack())
        {
            FirePoisonArrow();
            Debug.Log("PlayerAttack2");
        }
            
        cooldownTimer += Time.deltaTime;
    }

    private void FirePoisonArrow()
    {
        anim.SetTrigger("attack2");
        cooldownTimer = 0;

        arrows[FindPoisonArrow()].transform.position = FirePoint.position;
        arrows[FindPoisonArrow()].GetComponent<PoisonArrow>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private int FindPoisonArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}