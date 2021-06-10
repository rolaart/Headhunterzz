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
        
        public static Vector3Int ModulusNegative(this Vector3Int vec, int mod)
        {
            vec.x %= mod;
            vec.y %= mod;
            vec.z %= mod;
            
            if (vec.x < 0)
            {
                vec.x += mod;
            }

            if (vec.y < 0)
            {
                vec.y += mod;
            }

            if (vec.z < 0)
            {
                vec.z += mod;
            }

            return vec;
        }
    }
}