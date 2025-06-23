using UnityEngine;

namespace UtilityAI
{
    public class BossAnimationEvents : MonoBehaviour
    {
        private Brain brain;

        void Awake()
        {
            brain = GetComponent<Brain>();
        }

        public void DealMeleeDamage()
        {
          //  brain.DealMeleeDamage();
        }

        public void OnAttackAnimationEnd()
        {
            //brain.OnAttackAnimationEnd();
        }
    }
}
