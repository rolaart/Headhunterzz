using System;
using UnityEngine;
using World.WorldGeneration;
using Utils.Attributes;

namespace Utils
{
    
    [Serializable]
    public enum SoundType
    {
        SoundWeaponAttack
    }

    public class SoundManager : Manager<SoundManager>
    {
        [NamedArrayAttribute(typeof(SoundType))]
        public AudioClip[] sounds = new AudioClip[Enum.GetNames(typeof(SoundType)).Length];
        

       
        private AudioSource soundSource;
        

        private void Start()
        {
            soundSource = GetComponent<AudioSource>();
            
        }

        public void Play(SoundType type)
        {
            soundSource.PlayOneShot(sounds[(int) type]);
        }

       
    }
    
    
}