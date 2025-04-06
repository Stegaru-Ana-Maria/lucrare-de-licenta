using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpecialCrystalAttackNode : BTNode
{
    private Transform boss;
    private Transform player;
    private Animator animator;
    private GameObject crystalPrefab;
    private Transform crystalSpawnPoint;
    private float cooldown;
    private float attackDuration;
    private LayerMask playerLayer;
    private float lastAttackTime;
    private float specialAttackRange;
    private Boss2AI bossAI;

    public SpecialCrystalAttackNode(Transform boss, Transform player, Animator animator, GameObject crystalPrefab, Transform crystalSpawnPoint, float cooldown, float attackDuration, float specialAttackRange, LayerMask playerLayer)
    {
        this.boss = boss;
        this.player = player;
        this.animator = animator;
        this.crystalPrefab = crystalPrefab;
        this.crystalSpawnPoint = crystalSpawnPoint;
        this.cooldown = cooldown;
        this.attackDuration = attackDuration;
        this.specialAttackRange = specialAttackRange;
        this.playerLayer = playerLayer;
        bossAI = boss.GetComponent<Boss2AI>();
    }

    public override NodeState Evaluate()
    {
        if (Time.time - lastAttackTime < cooldown)
            return NodeState.FAILURE;

        float distanceToPlayer = Vector2.Distance(boss.position, player.position);
        if (distanceToPlayer > specialAttackRange)
            return NodeState.FAILURE;

        animator.SetBool("isRunning", false);
        animator.SetTrigger("specialAttack");

        if (!bossAI.isPerformingSpecialAttack)
        {
            bossAI.StartCoroutine(HandleCrystalAttack());
            lastAttackTime = Time.time;
        }

        return NodeState.SUCCESS;
    }    
    private IEnumerator HandleCrystalAttack()
    {
        bossAI.isPerformingSpecialAttack = true;

        yield return new WaitForSeconds(0.27f);

        GameObject crystal = GameObject.Instantiate(
            crystalPrefab,
            crystalSpawnPoint.position,
            Quaternion.identity
        );

        CrystalHitbox hitbox = crystal.GetComponent<CrystalHitbox>();
        if (hitbox != null)
        {
            float bossDirection = Mathf.Sign(boss.localScale.x);
            hitbox.SetDirection(bossDirection);
        }

        Animator crystalAnim = crystal.GetComponent<Animator>();
        if (crystalAnim != null)
        {
            crystalAnim.SetTrigger("appear");
        }

        yield return new WaitForSeconds(0.13f);

        if (crystalAnim != null)
        {
            crystalAnim.SetTrigger("disappear");
        }

        yield return new WaitForSeconds(0.3f); 

        GameObject.Destroy(crystal);
        bossAI.isPerformingSpecialAttack = false;
    }
}
