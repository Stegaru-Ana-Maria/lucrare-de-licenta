using UnityEngine;

namespace UnityUtils
{
    public static class TransformExtensions
    {
        public static bool InRangeOf(this Transform self, Transform target, float maxDistance, float maxAngle)
        {
            if (target == null) return false;

            Vector3 directionToTarget = target.position - self.position;
            float distance = directionToTarget.magnitude;
            if (distance > maxDistance) return false;

            float angle = Vector3.Angle(self.forward, directionToTarget);
            if (angle > maxAngle * 0.5f) return false;

            return true;
        }
    }
}
