using UnityEngine;

public class AudioVolumeChange : MonoBehaviour
{
    private AudioSource[] audioClips;

    private void Start()
    {
        audioClips = GetComponents<AudioSource>();
    }

    private void Update()
    {
        if (audioClips == null) return;
        audioClips[0].volume = AudioManager.BgmVolume;
        for (int i = 1; i < audioClips.Length; i++)
        {
            audioClips[i].volume = AudioManager.SEVolume;
        }
    }
}
