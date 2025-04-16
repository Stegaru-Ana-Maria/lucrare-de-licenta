using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
    public string actionName = "New Action";
    public float cost = 1f;
    public bool inProgress { get; protected set; }

    public Dictionary<string, bool> preconditions = new Dictionary<string, bool>();
    public Dictionary<string, bool> effects = new Dictionary<string, bool>();

    protected GameObject target;
    public bool requiresInRange = false;
    public bool isInRange = false;

    protected virtual void Awake()
    {
        Debug.Log($"[GOAPAction] {actionName} initialized on {gameObject.name}");
    }

    protected virtual void Start()
    {
        if (string.IsNullOrEmpty(actionName))
            actionName = this.GetType().Name;

        Debug.Log($"[GOAPAction] {actionName} started on {gameObject.name}");
    }


    public void AddPrecondition(string key, bool value) => preconditions[key] = value;
    public void AddEffect(string key, bool value) => effects[key] = value;

    public abstract bool CheckProceduralPrecondition(GameObject agent); // ex: vezi sulita pe jos?
    public abstract bool Perform(GameObject agent);                     // efectul propriu-zis
    public abstract bool IsDone();                                      // s-a terminat actiunea?
    public abstract void ResetAction();                                 // resetare intre planuri
    public GameObject GetTarget() => target;
    public void SetTarget(GameObject t) => target = t;
    public virtual float GetCost() => cost;
    public virtual void Abort()
    {
        inProgress = false;
        Debug.Log($"[GOAPAction] {actionName} was aborted.");
    }
    public virtual bool IsInProgress() => inProgress;
    public virtual void OnActionComplete()
    {
        Debug.Log($"[GOAPAction] {actionName} completed.");
    }
    public virtual void PrintInfo()
    {
        Debug.Log($"Action: {actionName} | Cost: {cost}");
        Debug.Log($"Preconditions: {string.Join(", ", preconditions)}");
        Debug.Log($"Effects: {string.Join(", ", effects)}");
    }
}

