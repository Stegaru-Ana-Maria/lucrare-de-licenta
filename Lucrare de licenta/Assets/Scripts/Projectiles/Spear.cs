using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpearVisualController spearVisual;

    [Header("Trajectory Settings")]
    [SerializeField] private float destroyDistanceThreshold = 1f;

    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 3f;

    private Transform target;
    private float moveSpeed;
    private float maxMoveSpeed;
    private float trajectoryMaxRelativeHeight;

    private AnimationCurve trajectoryAnimationCurve;
    private AnimationCurve axisCorrectionAnimationCurve;
    private AnimationCurve projectileSpeedAnimationCurve;

    private Vector3 trajectoryStartPoint;
    private Vector3 projectileMoveDir;
    private Vector3 trajectoryRange;

    private float nextYTrajectoryPosition;
    private float nextXTrajectoryPosition;
    private float nextPositionYCorrectionAbsolute;
    private float nextPositionXCorrectionAbsolute;

    private bool hasHitSomething = false;


    private void Start()
    {
        trajectoryStartPoint = transform.position;
    }

    private void Update()
    {
        if (hasHitSomething) return;

        UpdateProjectilePosition();


        if (Vector3.Distance(transform.position, target.position) < destroyDistanceThreshold)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateProjectilePosition()
    {
        trajectoryRange = target.position - trajectoryStartPoint;

        // Projectile will be curved on the X axis
        if (Mathf.Abs(trajectoryRange.normalized.x) < Mathf.Abs(trajectoryRange.normalized.y))
        {
            if (trajectoryRange.y < 0)
            {
                moveSpeed = -moveSpeed;
            }

            UpdatePositionWithXCurve();
        }
        else
        {
            // Projectile will be curved on the Y axis
            if (trajectoryRange.x < 0)
            {
                moveSpeed = -moveSpeed;
            }

            UpdatePositionWithYCurve();
        }


    }

    private void UpdatePositionWithXCurve()
    {
        float nextPositionY = transform.position.y + moveSpeed * Time.deltaTime;
        float nextPositionYNormalized = (nextPositionY - trajectoryStartPoint.y) / trajectoryRange.y;

        float nextPositionXNormalized = trajectoryAnimationCurve.Evaluate(nextPositionYNormalized);
        nextXTrajectoryPosition = nextPositionXNormalized * trajectoryMaxRelativeHeight;

        float nextPositionXCorrectionNormalized = axisCorrectionAnimationCurve.Evaluate(nextPositionYNormalized);
        nextPositionXCorrectionAbsolute = nextPositionXCorrectionNormalized * trajectoryRange.x;

        if ((trajectoryRange.x > 0 && trajectoryRange.y > 0) || (trajectoryRange.x < 0 && trajectoryRange.y < 0))
        {
            nextXTrajectoryPosition = -nextXTrajectoryPosition;
        }

        float nextPositionX = trajectoryStartPoint.x + nextXTrajectoryPosition + nextPositionXCorrectionAbsolute;
        Vector3 newPosition = new Vector3(nextPositionX, nextPositionY, 0);

        CalculateNextProjectileSpeed(nextPositionYNormalized);
        projectileMoveDir = newPosition - transform.position;
        transform.position = newPosition;
    }

    private void UpdatePositionWithYCurve()
    {
        float nextPositionX = transform.position.x + moveSpeed * Time.deltaTime;
        float nextPositionXNormalized = (nextPositionX - trajectoryStartPoint.x) / trajectoryRange.x;

        float nextPositionYNormalized = trajectoryAnimationCurve.Evaluate(nextPositionXNormalized);
        nextYTrajectoryPosition = nextPositionYNormalized * trajectoryMaxRelativeHeight;

        float nextPositionYCorrectionNormalized = axisCorrectionAnimationCurve.Evaluate(nextPositionXNormalized);
        nextPositionYCorrectionAbsolute = nextPositionYCorrectionNormalized * trajectoryRange.y;

        float nextPositionY = trajectoryStartPoint.y + nextYTrajectoryPosition + nextPositionYCorrectionAbsolute;
        Vector3 newPosition = new Vector3(nextPositionX, nextPositionY, 0);

        CalculateNextProjectileSpeed(nextPositionXNormalized);
        projectileMoveDir = newPosition - transform.position;
        transform.position = newPosition;
    }

    private void CalculateNextProjectileSpeed(float nextPositionXNormalized)
    {
        float nextMoveSpeedNormalized = projectileSpeedAnimationCurve.Evaluate(nextPositionXNormalized);

        moveSpeed = nextMoveSpeedNormalized * maxMoveSpeed;
    }


    public void InitializeProjectile(Transform target, float maxMoveSpeed, float trajectoryMaxHeight)
    {
        this.target = target;
        this.maxMoveSpeed = maxMoveSpeed;

        float xDistanceToTarget = target.position.x - transform.position.x;
        this.trajectoryMaxRelativeHeight = Mathf.Abs(xDistanceToTarget) * trajectoryMaxHeight;

        spearVisual.SetTarget(target);
    }


    public void InitializeAnimationCurves(AnimationCurve trajectoryAnimationCurve, AnimationCurve axisCorrectionAnimationCurve, AnimationCurve projectileSpeedAnimationCurve)
    {
        this.trajectoryAnimationCurve = trajectoryAnimationCurve;
        this.axisCorrectionAnimationCurve = axisCorrectionAnimationCurve;
        this.projectileSpeedAnimationCurve = projectileSpeedAnimationCurve;
    }

    private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            hasHitSomething = true;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Static;
            }

            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }

            if (spearVisual != null)
            {
                spearVisual.transform.rotation = Quaternion.identity;
            }
        }
    }


    public Vector3 GetProjectileMoveDir()
    {
        return projectileMoveDir;
    }


    public float GetNextYTrajectoryPosition()
    {
        return nextYTrajectoryPosition;
    }


    public float GetNextPositionYCorrectionAbsolute()
    {
        return nextPositionYCorrectionAbsolute;
    }


    public float GetNextXTrajectoryPosition()
    {
        return nextXTrajectoryPosition;
    }


    public float GetNextPositionXCorrectionAbsolute()
    {
        return nextPositionXCorrectionAbsolute;
    }


}
