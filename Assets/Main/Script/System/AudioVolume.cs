using UnityEngine;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    private Slider slider;
    public enum soundType
    {
        BGM,
        SE
    }

    public soundType soundT;

    void Start()
    {
        slider = this.GetComponent<Slider>();
        if(soundT == soundType.BGM)
        {
            slider.value = AudioManager.BgmVolume;
        }else if(soundT == soundType.SE)
        {
            slider.value = AudioManager.SEVolume;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (soundT == soundType.BGM)
        {
            AudioManager.BgmVolume = slider.value;
        }
        else if (soundT == soundType.SE)
        {
            AudioManager.SEVolume = slider.value;
        }
    }
}
