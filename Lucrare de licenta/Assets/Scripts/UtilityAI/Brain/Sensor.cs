using System;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Sensor : MonoBehaviour
    {
        public float detectionRadius = 10f;
        public List<string> targetTags = new();

        readonly List<Transform> detectedObjects = new(10);
        CircleCollider2D circleCollider;

        void Start()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.isTrigger = true;
            circleCollider.radius = detectionRadius;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
            foreach (var c in colliders)
            {
                ProcessTrigger(c, transform => detectedObjects.Add(transform));
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            ProcessTrigger(other, transform => detectedObjects.Add(transform));
        }

        void OnTriggerExit2D(Collider2D other)
        {
            ProcessTrigger(other, transform => detectedObjects.Remove(transform));
        }

        void ProcessTrigger(Collider2D other, Action<Transform> action)
        {
            if (other.CompareTag("Untagged")) return;

            foreach (string t in targetTags)
            {
                if (other.CompareTag(t))
                {
                    action(other.transform);
                    break;
                }
            }
        }

        public Transform GetClosestTarget(string tag)
        {
            if (detectedObjects.Count == 0) return null;

            Transform closestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (Transform potentialTarget in detectedObjects)
            {
                float dSqrToTarget = (potentialTarget.position - currentPosition).sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestTarget = potentialTarget;
                }
            }
            return closestTarget;
        }
    }
}