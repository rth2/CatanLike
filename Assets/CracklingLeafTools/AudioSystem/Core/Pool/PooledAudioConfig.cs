using UnityEngine;

[CreateAssetMenu(fileName = "PooledAudioConfig", menuName = "CracklingLeafTools/Audio System/Configs/PooledAudioConfig")]
public class PooledAudioConfig : ScriptableObject
{
    [Header("Playback")]
    public int priority = 128;

    [Header("Spatial Defaults")]
    [Range(0f, 1f)]
    public float spatialBlend = 0f;

    public float panStereo = 0f;
    public float dopplerLevel = 0f;

    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
    public float minDistance = 1f;
    public float maxDistance = 100f;

}
