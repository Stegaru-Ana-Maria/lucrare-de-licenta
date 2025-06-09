using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI
{
    public class Context
    {
        public Brain brain;
        public Transform bossTransform;
        public Transform playerTransform;
        public Transform spearTransform;
        public Transform healTreeTransform;
        public BossStats bossStats;
        public Transform target;
        public Sensor sensor;

        readonly Dictionary<string, object> data = new();

        public Context(Brain brain)
        {
            this.brain = brain;
            this.bossTransform = brain.transform;
            //  this.agent = brain.gameObject.GetOrAdd<NavMeshAgent>();
            //  this.sensor = brain.gameObject.GetOrAdd<Sensor>();
        }

        public T GetData<T>(string key) => data.TryGetValue(key, out var value) ? (T)value : default;
        public void SetData(string key, object value) => data[key] = value;
    }
}