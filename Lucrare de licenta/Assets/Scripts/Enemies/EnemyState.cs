using UnityEngine;

public abstract class EnemyState
{
    protected EnemyFSM enemy;

    public EnemyState(EnemyFSM enemy)
    {
        this.enemy = enemy;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

}
