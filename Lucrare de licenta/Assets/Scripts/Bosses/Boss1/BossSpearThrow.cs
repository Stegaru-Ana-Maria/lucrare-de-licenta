using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI;


public class BossSpearThrow : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform target;
    [SerializeField] private float shootRate;
    [SerializeField] private float projectileMaxMoveSpeed;
    [SerializeField] private float projectileMaxHeight;

    [Header("Trajectory Settings")]
    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrectionAnimationCurve;
    [SerializeField] private AnimationCurve projectileSpeedAnimationCurve;

    private float shootTimer;
    private bool hasSpear = true;
    private Animator animator;


    public bool HasSpear => hasSpear;

    private void Start()
    {
        shootTimer = 0f;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public bool TryShoot()
    {
        Debug.Log($"TryShoot() called | hasSpear: {hasSpear}, shootTimer: {shootTimer}");

        if (hasSpear && shootTimer <= 0f)
        {
            Debug.Log("Shooting spear!");
            ShootSpear();
            shootTimer = shootRate;
            return true;
        }

        return false;
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
    }

    private void ShootSpear()
    {
        GameObject spearGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Spear spear = spearGO.GetComponent<Spear>();
        if (spear != null)
        {
            spear.InitializeProjectile(target, projectileMaxMoveSpeed, projectileMaxHeight);
            spear.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionAnimationCurve, projectileSpeedAnimationCurve);
        }

        hasSpear = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasSpear && collision.CompareTag("Spear"))
        {
            Spear spear = collision.GetComponent<Spear>();

            if (spear != null && spear.HasHitSomething())
            {
               // Destroy(spear.gameObject);
                hasSpear = true;
                Debug.Log("Spear collected");
            }
        }
    }
}

