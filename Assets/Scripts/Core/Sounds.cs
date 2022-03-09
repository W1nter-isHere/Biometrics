using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    [Serializable]
    public class Sound
    {
        public ESounds soundID;
        public AudioClip[] clip;
        [Range(-3, 3)]
        public float pitch = 1;
        [Range(0, 1)]
        public float volume = 1;
        public bool loop;
        public AudioSource source;

        private int _requestCount;

        public AudioClip GetClip()
        {
            if (_requestCount >= clip.Length)
            {
                _requestCount = 0;
            }
            
            var c = clip[_requestCount];
            _requestCount++;
            return c;
        }
    }

    [Serializable]
    public class SoundCategory
    {
        public AudioMixerGroup group;
        public ESoundCategory categoryID;
    }
    
    public enum ESounds
    {
        [SoundInfo(ESoundCategory.Music)] MainTheme,
        [SoundInfo(ESoundCategory.Music)] Menu,
        [SoundInfo(ESoundCategory.UI)] UISelect,
        [SoundInfo(ESoundCategory.Object)] EnemyDeath,
        [SoundInfo(ESoundCategory.Object)] EnemyShoot,
        [SoundInfo(ESoundCategory.Object)] EnemyAlert,
        [SoundInfo(ESoundCategory.Object)] SwingSword,
        [SoundInfo(ESoundCategory.Object)] Dash,
        
        [SoundInfo(ESoundCategory.Dialogue)] D_Congrats,
        [SoundInfo(ESoundCategory.Dialogue)] D_DeafenWarn,
        [SoundInfo(ESoundCategory.Dialogue)] D_Die,
        [SoundInfo(ESoundCategory.Dialogue)] D_Empathytry,
        [SoundInfo(ESoundCategory.Dialogue)] D_HTAttack,
        [SoundInfo(ESoundCategory.Dialogue)] D_HTDash,
        [SoundInfo(ESoundCategory.Dialogue)] D_HTJump,
        [SoundInfo(ESoundCategory.Dialogue)] D_HTPortal,
        [SoundInfo(ESoundCategory.Dialogue)] D_Intro,
        [SoundInfo(ESoundCategory.Dialogue)] D_Joke,
        [SoundInfo(ESoundCategory.Dialogue)] D_Liar,
        [SoundInfo(ESoundCategory.Dialogue)] D_Mistake,
        [SoundInfo(ESoundCategory.Dialogue)] D_NotFunny,
        [SoundInfo(ESoundCategory.Dialogue)] D_Ohno,
        [SoundInfo(ESoundCategory.Dialogue)] D_Realization,
        [SoundInfo(ESoundCategory.Dialogue)] D_Simulation,
        [SoundInfo(ESoundCategory.Dialogue)] D_Stop,
        [SoundInfo(ESoundCategory.Dialogue)] D_Survey,

        [SoundInfo(ESoundCategory.Object)] Explosion,
        [SoundInfo(ESoundCategory.Object)] Alarm,
    }

    public enum ESoundCategory
    {
        Music,
        Sfx,
        UI,
        Object,
        Dialogue
    }

    public sealed class SoundInfoAttribute : Attribute
    {
        public ESoundCategory Category { get; }

        internal SoundInfoAttribute(ESoundCategory category)
        {
            Category = category;
        }
    }

    public static class Planets
    {
        public static ESoundCategory GetCategory(this ESounds sounds)
        {
            var attribute = GetAttr(sounds);
            return attribute.Category;
        }
        
        private static SoundInfoAttribute GetAttr(ESounds p)
        {
            return (SoundInfoAttribute) Attribute.GetCustomAttribute(ForValue(p), typeof(SoundInfoAttribute));
        }
        
        private static MemberInfo ForValue(ESounds p)
        {
            return typeof(ESounds).GetField(Enum.GetName(typeof(ESounds), p));
        }
    }
}