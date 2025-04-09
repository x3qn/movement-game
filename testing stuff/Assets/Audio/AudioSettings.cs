using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "ScriptableObjects/AudioSettings")]

public class AudioSettings : ScriptableObject
{
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
}
