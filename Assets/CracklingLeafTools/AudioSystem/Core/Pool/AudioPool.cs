using UnityEngine;
using System.Collections.Generic;

namespace CracklingLeafTools.AudioSystem.Core
{
    public class AudioPool : MonoBehaviour
    {
        [Header("Pool Settings")]
        [SerializeField] int initialPoolSize = 10;
        [SerializeField] bool allowExpansion = true;

        List<PooledAudioSource> pooledSources = new();

        Transform poolParent;

        #region Initialization
        public void Initialize(Transform parent = null)
        {
            ClearPool();

            poolParent = parent != null ? parent :  transform;

            //prewarm
            for(int i = 0; i < initialPoolSize; i++) { CreateNewSource(); }
        }

        void ClearPool()
        {
            for(int i = 0; i < pooledSources.Count; i++)
            {
                if (pooledSources[i] != null && pooledSources[i].Source != null)
                {
                    Destroy(pooledSources[i].Source.gameObject);
                }
            }
        }

        #endregion

        #region Audio Source Management

        public void StopAll()
        {
            for(int i = 0; i < pooledSources.Count; i++)
            {
                PooledAudioSource pooled = pooledSources[i];
                if(pooled != null && pooled.IsActive)
                {
                    if(pooled.Source != null && pooled.Source.isPlaying)
                    {
                        pooled.Source.Stop();
                    }
                    pooled.ReturnToPool();
                }
            }
        }

        public AudioSource GetAvailableSource(PooledAudioConfig config)
        {
            //find an idle source
            foreach(PooledAudioSource pooled in pooledSources)
            {
                if(!pooled.IsActive) { return pooled.Activate(gameObject.transform.position, config); }
            }

            //expand pool if needed
            if(allowExpansion) { return CreateNewSource().Activate(gameObject.transform.position, config); }

            return null;    //pool exhausted
        }

        PooledAudioSource CreateNewSource()
        {
            GameObject go = new GameObject("PooledAudioSource");
            go.transform.SetParent(poolParent);
            AudioSource src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;

            PooledAudioSource pooled = new PooledAudioSource(src, OnSourceFinished);
            pooledSources.Add(pooled);

            return pooled;
        }

        public void StartWatcher(AudioSource source)
        {
            for(int i = 0; i < pooledSources.Count; i++)
            {
                if (pooledSources[i].Source == source)
                {
                    pooledSources[i].StartWatcher();
                }
            }
        }

        void OnSourceFinished(PooledAudioSource pooled)
        {
            pooled.ReturnToPool();
        }
        #endregion

        #region Editor and Debug

        private void OnValidate()
        {
            if (initialPoolSize < 1) initialPoolSize = 1;
        }

        #endregion
    }
}

