using UnityEngine;

namespace UtilityAI
{
    public class UtilityAIDebug : MonoBehaviour
    {
        public static UtilityAIDebug Instance;

        [Header("Boss-ul AI vizualizat")]
        public Transform agentTransform;

        [Header("Raze de atac")]
        public float meleeRange = 5f;
        public float spearRange = 20f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnDrawGizmos()
        {
            if (agentTransform == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(agentTransform.position, meleeRange);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(agentTransform.position, spearRange);
        }
    }
}
