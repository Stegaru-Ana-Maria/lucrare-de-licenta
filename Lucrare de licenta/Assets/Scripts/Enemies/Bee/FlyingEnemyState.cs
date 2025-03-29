using UnityEngine;

public abstract class FlyingEnemyState
{
    protected FlyingEnemyFSM enemy;

    public FlyingEnemyState(FlyingEnemyFSM enemy)
    {
        this.enemy = enemy;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

}
