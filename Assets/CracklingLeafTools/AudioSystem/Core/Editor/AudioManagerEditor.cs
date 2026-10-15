using UnityEditor;
using UnityEngine;
using CracklingLeafTools.AudioSystem.Core;

namespace CracklingLeafTools.AudioSystem.Core.Editor
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw default inspector first
            DrawDefaultInspector();

            EditorGUILayout.Space();

            // Add button
            if (GUILayout.Button("Generate SoundKey Constants"))
            {
                // Call your generator
                SoundKeyGenerator.GenerateConstants();

                // Optional: log feedback
                Debug.Log("[AudioManagerEditor] SoundKey constants regenerated.");
            }
        }
    }
}
