using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI
{
    public class Context
    {
        public Brain brain;
        public Transform bossTransform;
        public Transform playerTransform;
        public BossHealth health;
        public Spear spear;
        public Transform spearTransform;
        public Vector3 spearPosition;
        public Transform healSpot;
        public BossStats bossStats;
        public Transform target;
        public Sensor sensor;
        public Animator animator;
        public Rigidbody2D bossRigidbody;
        public BossSpearThrow bossSpearThrow;
        public bool hasSpear = true;
        public float speed = 5f;
        public float jumpForce = 10f;
        public float stopDistance = 1.2f;
        public float obstacleCheckDistance = 2.0f;
        public float groundCheckDistance = 1.5f;
        public LayerMask obstacleMask;
     //   public LayerMask spearLayer;

        public Dictionary<string, object> data;

        public Context(Brain brain)
        {
            this.brain = brain;
            this.data = brain.data;
            this.bossTransform = brain.transform;
            this.animator = brain.GetComponent<Animator>();
            this.obstacleMask = brain.obstacleMask;
            this.health = brain.GetComponent<BossHealth>();

        }

        public T GetData<T>(string key) => data.TryGetValue(key, out var value) ? (T)value : default;
        public void SetData(string key, object value) => data[key] = value;
    }
}