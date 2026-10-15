using UnityEngine;

namespace CracklingLeafTools.AudioSystem.Core
{
    /// <summary>
    /// Defines all data required to play a specific sound.
    /// </summary>
    [CreateAssetMenu(
        fileName = "SoundDefinition",
        menuName = "CracklingLeafTools/Audio System/Sounds/Sound Definition")]
    public class SoundDefinition : ScriptableObject
    {
        [Header("Basic Info")]
        public string soundKey;

        [Header("Audio Clip")]
        public AudioClip clip;

        [Header("Playback Settings")]
        public float volume = 1f;
        public float pitch = 1f;
        public bool loop = false;

        [Header("Mixer Routing (optional)")]
        public UnityEngine.Audio.AudioMixerGroup outputGroup;

        [Header("Advanced (Optional, Future)")]
        public float randomPitchMin = 1f;
        public float randomPitchMax = 1f;

    }
}
