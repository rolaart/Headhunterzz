using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class IslandDisplay : MonoBehaviour
    {
        [SerializeField] private Animation animation;

        public UnityEvent onFadeInAndOutAnimationClip;

        #region Animation Clip Callbacks
        
        
        public void OnFadeInAndOutComplete()
        {
            onFadeInAndOutAnimationClip.Invoke();
        }
        
        #endregion
        

        public void FadeIn()
        {
            animation.Play();
        }
    }
}