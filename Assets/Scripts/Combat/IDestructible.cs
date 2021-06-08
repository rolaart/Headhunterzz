using UnityEngine;

namespace Combat
{
    public interface IDestructible
    {
        void OnDestruction(GameObject destroyer);
    }
}