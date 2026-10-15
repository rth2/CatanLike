using System.Collections.Generic;
using CracklingLeafTools.AudioSystem.Core;
using CracklingLeafTools.AudioSystem.Core.Editor;
using NUnit.Framework;
using UnityEngine;

namespace CracklingLeafTools.AudioSystem.Core
{
    [CreateAssetMenu(
    fileName = "SoundLibrary",
    menuName = "CracklingLeafTools/Audio System/Sounds/SoundLibrary")]
    public class SoundLibrary : ScriptableObject
    {
        public string libraryName;
        public List<SoundDefinition> sounds = new();

        Dictionary<string, SoundDefinition> lookup;
        string safeLibraryName;

        public string SafeLibraryName
        {
            get
            {
                if(safeLibraryName == null) { safeLibraryName = SoundKeyGenerator.MakeStringSafe(libraryName); }
                return safeLibraryName;
            }
        }
        /// <summary>
        /// Tries to find a sound definition by its safe key
        /// </summary>
        public bool TryFind(string safeKey, out SoundDefinition def)
        {
            lookup ??= BuildLookup();
            return lookup.TryGetValue(safeKey, out def);
        }

        /// <summary>
        /// Builds the lookup dictionary for this library.
        /// </summary>
        Dictionary<string, SoundDefinition> BuildLookup()
        {
            Dictionary<string, SoundDefinition> dict = new();
            foreach(SoundDefinition def in sounds)
            {
                if(def == null) { continue; }
                string safeKey = SoundKeyGenerator.MakeStringSafe(def.soundKey);
                if(!dict.ContainsKey(safeKey))
                {
                    dict[safeKey] = def;
                }
                else
                {
                    Debug.LogWarning($"Duplicate sound key in library '{libraryName}': '{def.soundKey}'");
                }
            }
            return dict;
        }

#if UNITY_EDITOR
        // Auto rebuild whenever list changes in editor
        private void OnValidate()
        {
            lookup = null;
            safeLibraryName = null;
        }
#endif
    }
}
