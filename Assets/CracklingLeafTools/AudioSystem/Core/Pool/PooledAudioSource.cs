using UnityEngine;

namespace CracklingLeafTools.AudioSystem.Core
{
    public class PooledAudioSource
    {
        public AudioSource Source { get; private set; }
        public bool IsActive { get; private set; }

        System.Action<PooledAudioSource> onFinished;

        public PooledAudioSource(AudioSource source, System.Action<PooledAudioSource> callback)
        {
            Source = source;
            onFinished = callback;
        }

        /// <summary>
        /// Ensures that the audiosource is set to a neutral, expected state and ready to use.
        /// </summary>
        public AudioSource Activate(Vector3 position, PooledAudioConfig config)
        {
            IsActive = true;
            Source.gameObject.SetActive(true);
            Source.priority = config.priority;  //unity default priority

            //playback
            Source.Stop();
            Source.clip = null;
            Source.volume = 1f;
            Source.pitch = 1f;
            Source.loop = false;

            //spatial
            Source.transform.position = position;
            Source.spatialBlend = config.spatialBlend;
            Source.panStereo = config.panStereo;
            Source.dopplerLevel = config.dopplerLevel;
            Source.rolloffMode = config.rolloffMode;
            Source.minDistance = config.minDistance;
            Source.maxDistance = config.maxDistance;

            //routing
            Source.outputAudioMixerGroup = null;

            //make sure no old watcher exists
            PoolWatcher existing = Source.GetComponent<PoolWatcher>();
            if (existing != null)
            {
                Object.Destroy(existing);
            }

            return Source;
        }

        public void StartWatcher() { Source.gameObject.AddComponent<PoolWatcher>().BeginWatching(this, onFinished); }

        public void ReturnToPool()
        {
            IsActive = false;

            PoolWatcher watcher = Source.GetComponent<PoolWatcher>();
            if (watcher != null)
            {
                Object.Destroy(watcher);
            }

            if (Source != null)
            {
                Source.Stop();
                Source.clip = null;
                Source.gameObject.SetActive(false);
            }
        }
    }
}
