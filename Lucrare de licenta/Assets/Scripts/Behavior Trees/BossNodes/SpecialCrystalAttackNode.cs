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
    private Boss2AI bossAI;

    public SpecialCrystalAttackNode(Transform boss, Transform player, Animator animator, GameObject crystalPrefab, Transform crystalSpawnPoint, float cooldown, float attackDuration, LayerMask playerLayer)
    {
        this.boss = boss;
        this.player = player;
        this.animator = animator;
        this.crystalPrefab = crystalPrefab;
        this.crystalSpawnPoint = crystalSpawnPoint;
        this.cooldown = cooldown;
        this.attackDuration = attackDuration;
        this.playerLayer = playerLayer;
        bossAI = boss.GetComponent<Boss2AI>();
    }

    public override NodeState Evaluate()
    {
        if (Time.time - lastAttackTime < cooldown)
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
    /*
    private IEnumerator HandleCrystalAttack()
    {
        boss.GetComponent<Boss2AI>().isPerformingSpecialAttack = true;

        GameObject crystal = GameObject.Instantiate(crystalPrefab);
        crystal.transform.position = crystalSpawnPoint.position;

        Animator crystalAnim = crystal.GetComponent<Animator>();
        if (crystalAnim != null)
        {
            crystalAnim.Play("Idle", 0); 
            crystalAnim.ResetTrigger("disappear");
            crystalAnim.SetTrigger("appear");
            Debug.Log("APPEAR");
        }

        yield return new WaitForSeconds(0.5f); 

        if (crystalAnim != null)
        {
            crystalAnim.SetTrigger("disappear");
            Debug.Log("DISAPPEAR");
        }

        yield return new WaitForSeconds(0.5f); 

        GameObject.Destroy(crystal);
        boss.GetComponent<Boss2AI>().isPerformingSpecialAttack = false;
    }
    */
    
    private IEnumerator HandleCrystalAttack()
    {
        bossAI.isPerformingSpecialAttack = true;

        yield return new WaitForSeconds(0.27f);

        GameObject crystal = GameObject.Instantiate(
            crystalPrefab,
            crystalSpawnPoint.position,
            Quaternion.identity
        );


        crystal.SetActive(true);

        yield return new WaitForSeconds(0.2f); 

        Animator crystalAnim = crystal.GetComponent<Animator>();
        if (crystalAnim != null)
        {
            crystalAnim.SetTrigger("appear");
            Debug.Log("APPEAR");
        }

        yield return new WaitForSeconds(0.5f); // durata animatiei de disparitie
       // crystalAnim.SetTrigger("disappear");
        GameObject.Destroy(crystal);
        bossAI.isPerformingSpecialAttack = false;
    }
}
