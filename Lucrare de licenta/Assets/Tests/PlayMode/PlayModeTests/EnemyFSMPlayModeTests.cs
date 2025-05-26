using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyFSMPlayModeTests
{
    /*
    [UnityTest]
    public IEnumerator EnemyStartsInPatrolState()
    {
        var enemyPrefab = Resources.Load<GameObject>("Enemy");
        var enemyGO = GameObject.Instantiate(enemyPrefab);
        var fsm = enemyGO.GetComponent<EnemyFSM>();

        yield return null;

        Assert.AreEqual("PatrolState", fsm.GetCurrentStateName());
    }

    [UnityTest]
    public IEnumerator EnemyEntersChaseStateWhenForced()
    {
        var enemyPrefab = Resources.Load<GameObject>("Enemy");
        var enemyGO = GameObject.Instantiate(enemyPrefab);
        var fsm = enemyGO.GetComponent<EnemyFSM>();

        yield return null;

        fsm.ForceChaseState();
        yield return null;

        Assert.AreEqual("ChaseState", fsm.GetCurrentStateName());
    }

    [UnityTest]
    public IEnumerator EnemyEntersAttackStateWhenForced()
    {
        var enemyPrefab = Resources.Load<GameObject>("Enemy");
        var enemyGO = GameObject.Instantiate(enemyPrefab);
        var fsm = enemyGO.GetComponent<EnemyFSM>();

        yield return null;

        fsm.ForceAttackState();
        yield return null;

        Assert.AreEqual("AttackState", fsm.GetCurrentStateName());
    }
    */
}