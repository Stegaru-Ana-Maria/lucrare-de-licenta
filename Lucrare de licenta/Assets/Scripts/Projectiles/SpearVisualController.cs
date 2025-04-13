using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpearVisualController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform spearVisual;
    [SerializeField] private Spear spear;


    private Transform target;
    private Vector3 trajectoryStartPosition;

    private void Start()
    {
        trajectoryStartPosition = transform.position;
    }


    private void Update()
    {
        if (spear != null && spear.HasHitSomething()) return;

        UpdateProjectileRotation();

    }

    private void UpdateProjectileRotation()
    {
        Vector3 projectileMoveDir = spear.GetProjectileMoveDir();
        float angle = Mathf.Atan2(projectileMoveDir.y, projectileMoveDir.x) * Mathf.Rad2Deg;
        spearVisual.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void SetTarget(Transform newTarget)
    {
        this.target = newTarget;
    }
}
