using System;
using System.Collections.Generic;
using System.Linq;
using CracklingLeafTools.AudioSystem.Core.Editor;
using UnityEngine;

namespace CracklingLeafTools.AudioSystem.Core
{
    [DefaultExecutionOrder(-50)]    //ensures this initializes early
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Dependencies")]
        [SerializeField] UnityEngine.Audio.AudioMixer masterMixer;
        [SerializeField] AudioPool audioPool;

        [Header("Persisting Libraries")]
        [SerializeField] SoundLibrary[] persistentLibraries;

        [Header("Config Files")]
        [SerializeField] PooledAudioConfig config2d;
        [SerializeField] PooledAudioConfig config3d;

        List<SoundLibrary> sceneLibraries;
        Dictionary<string, float> mixerValues = new();

        bool isInitialized = false;

        #region Setup
        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);  //persistent
            Initialize();
        }
        void Initialize()
        {
            if (isInitialized) { return; }
            isInitialized = true;
            sceneLibraries = new();
            audioPool.Initialize();
            //set up pool, load definitions, prepare mixer groups
        }

        #endregion

        #region Library Management
        public void ClearSceneLibraries() { sceneLibraries.Clear(); }
        public void AddSceneLibrary(SoundLibrary library)
        {
            if(library != null && !persistentLibraries.Contains(library) && !sceneLibraries.Contains(library)) { sceneLibraries.Add(library); }
        }
        public void RemoveSceneLibrary(SoundLibrary library)
        {
            sceneLibraries.Remove(library);
        }
        #endregion

        #region Various Playbacks
        /// <summary>
        /// Play sound using the sound definition
        /// </summary>
        /// <param name="def"></param>
        void PlaySound(SoundDefinition def, PooledAudioConfig config)
        {
            if (def == null || def.clip == null) { return; }

            AudioSource src = audioPool.GetAvailableSource(config);
            if (src == null) { return; }

            src.clip = def.clip;
            src.volume = def.volume;
            src.pitch = (def.randomPitchMin != def.randomPitchMax)
                ? UnityEngine.Random.Range(def.randomPitchMin, def.randomPitchMax)
                : def.pitch;

            src.loop = def.loop;

            if (def.outputGroup != null)
            {
                src.outputAudioMixerGroup = def.outputGroup;
            }
            src.Play();
            audioPool.StartWatcher(src);
        }

        void PlaySound(SoundDefinition def, Vector3 position, PooledAudioConfig config)
        {
            if (def == null || def.clip == null) return;

            AudioSource src = audioPool.GetAvailableSource(config);
            if (src == null) return;

            // Positioning
            src.transform.position = position;

            // Standard setup (same as non-positional)
            src.clip = def.clip;
            src.volume = def.volume;
            src.pitch = (def.randomPitchMin != def.randomPitchMax)
                ? UnityEngine.Random.Range(def.randomPitchMin, def.randomPitchMax)
                : def.pitch;

            src.loop = def.loop;

            if (def.outputGroup != null)
            {
                src.outputAudioMixerGroup = def.outputGroup;
            }

            src.Play();
            audioPool.StartWatcher(src);
        }

        #endregion

        #region Public Playback API
        

        public void PlaySound(string soundKey)
        {
            //parts[0] = lib name; parts[1] = soundDef name
            string[] parts = soundKey.Split(SoundKeyGenerator.Separator);
            for(int i = 0; i < parts.Length; i++) { parts[i] = SoundKeyGenerator.MakeStringSafe(parts[i]); }
            //Search scene libraries first
            foreach(SoundLibrary sceneLib in sceneLibraries)
            {
                if(sceneLib.SafeLibraryName == parts[0] && sceneLib.TryFind(parts[1], out SoundDefinition def))
                {
                    PlaySound(def, config2d);
                    return;
                }
            }
            //Search persistent libraries second
            foreach(SoundLibrary lib in persistentLibraries)
            {
                if (lib.SafeLibraryName == parts[0] && lib.TryFind(parts[1], out SoundDefinition sDef))
                {
                    PlaySound(sDef, config2d);
                    return;
                }
            }
        }

        public void PlaySound(string soundKey, Vector3 position)
        {
            string[] parts = soundKey.Split(SoundKeyGenerator.Separator);
            for (int i = 0; i < parts.Length; i++)
                parts[i] = SoundKeyGenerator.MakeStringSafe(parts[i]);

            // Scene libraries first
            foreach (SoundLibrary sceneLib in sceneLibraries)
            {
                if (sceneLib.SafeLibraryName == parts[0] &&
                    sceneLib.TryFind(parts[1], out SoundDefinition def))
                {
                    PlaySound(def, position, config3d);
                    return;
                }
            }

            // Persistent libraries second
            foreach (SoundLibrary lib in persistentLibraries)
            {
                if (lib.SafeLibraryName == parts[0] &&
                    lib.TryFind(parts[1], out SoundDefinition def))
                {
                    PlaySound(def, position, config3d);
                    return;
                }
            }
        }

        /// <summary>
        /// Stops all currently playing sounds. Optional use cases.
        /// </summary>
        public void StopAllSounds()
        {
            if(audioPool != null) { audioPool.StopAll(); }
        }

        #endregion

        #region Volume and Mixer Controls
        public void SetVolume(string parameterName, float linearVolume)
        {
            if (masterMixer == null) return;

            linearVolume = Mathf.Clamp01(linearVolume);

            float dB = (linearVolume > 0f)
                ? Mathf.Lerp(-80f, 0f, linearVolume)
                : -80f;

            masterMixer.SetFloat(parameterName, dB);

            mixerValues[parameterName] = linearVolume;
        }
        #endregion

        #region Debug and Editor Helpers
        private void OnValidate()
        {
            //optional: auto assign mixer or validate references in editor.
        }
        #endregion

    }
}

