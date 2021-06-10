using System;
using UnityEngine;
using Utils.Managers;

namespace Utils.Attributes
{
    [Serializable]
    public class NamedArrayAttribute : PropertyAttribute
    {
        public readonly string[] names;

        public NamedArrayAttribute(string[] names)
        {
            this.names = names;
        }

        public NamedArrayAttribute(Type type)
        {
            if(type == typeof(MusicType))
                names = Enum.GetNames(typeof(MusicType));
            else if (type == typeof(SoundType))
                names = Enum.GetNames(typeof(SoundType));
            else
                throw new NotImplementedException();
        }
    }
}