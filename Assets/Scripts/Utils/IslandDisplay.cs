using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Utils
{
    public class IslandDisplay : MonoBehaviour
    {
        [FormerlySerializedAs("animation")] [SerializeField] private Animation fadeAnimation;

        public UnityEvent onFadeInAndOutAnimationClip;

        #region Animation Clip Callbacks
        
        
        public void OnFadeInAndOutComplete()
        {
            onFadeInAndOutAnimationClip.Invoke();
        }
        
        #endregion
        

        public void FadeIn()
        {
            fadeAnimation.Play();
        }
    }
}