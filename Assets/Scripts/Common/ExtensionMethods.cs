using Characters;
using UnityEngine;

namespace Common
{
    public static class ExtensionMethods
    {

        public static bool IsFacingTarget(this GameObject gameObject, GameObject target)
        {
            var vectorToTarget = target.transform.position - gameObject.transform.position;
            int direction = Character.DirectionToIndex(vectorToTarget);

            return direction == gameObject.GetComponent<Character>().lastDirection;
        }

        public static void SetToFaceTarget(this GameObject gameObject, GameObject target)
        {
            var vectorToTarget = target.transform.position - gameObject.transform.position;
            gameObject.GetComponent<Character>().SetDirection(vectorToTarget);
        }
    }
}