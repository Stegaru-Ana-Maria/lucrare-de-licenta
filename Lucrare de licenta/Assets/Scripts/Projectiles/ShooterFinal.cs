using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShooterFinal : MonoBehaviour
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


    private void Update()
    {
        shootTimer -= Time.deltaTime;

        if (hasSpear && shootTimer <= 0f)
        {
            ShootSpear();
            shootTimer = shootRate;
        }
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
                Destroy(spear.gameObject);
                hasSpear = true;
                Debug.Log("Spear collected");
            }
        }
    }
}

