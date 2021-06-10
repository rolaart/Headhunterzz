using System;
using UnityEngine;
using Utils.Attributes;
using World;

namespace Utils.Managers
{
    [Serializable]
    public enum MusicType
    {
        MusicAmbiencePlains,
        MusicAmbienceSnowyTundra,
        MusicAmbienceDesert,
    }

    public class MusicManager : Manager<MusicManager>
    {

        [NamedArrayAttribute(typeof(MusicType))]
        public AudioClip[] musics = new AudioClip[Enum.GetNames(typeof(MusicType)).Length];

        private AudioSource musicSource;

        private void Start()
        {
            GameManager.Instance.onIslandVisit.AddListener(OnIslandVisit);
            musicSource = GetComponent<AudioSource>();
        }
        
        public void Play(MusicType type)
        {
            musicSource.PlayOneShot(musics[(int) type]);
        }
        
        public void PlayAmbience(string biomeName){
            switch (biomeName)
            {
                case "Plains":
                    Play(MusicType.MusicAmbiencePlains);
                    break;
                case "Desert":
                    Play(MusicType.MusicAmbienceDesert);
                    break;
                case "Snowy Tundra":
                    Play(MusicType.MusicAmbienceSnowyTundra);
                    break;
            }
        }

        public void OnIslandVisit(IslandChunk islandChunk)
        {
            PlayAmbience(islandChunk.Biome.name);
        }
    }
}