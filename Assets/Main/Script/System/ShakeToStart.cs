using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShakeToStart : MonoBehaviour
{
    [SerializeField]
    private Image StartGage;

    private float passTime = 0;

    private float maxTime = 3f;
    void Update()
    {
        if (PitariDB.Instance.GetConBool() == 0)
        {
            Joycon();
        }
        else if (PitariDB.Instance.GetConBool() == 1)
        {
            Oricon();
        }
    }

    public void ShakeGameStart(bool isStart)
    {
        if(isStart)
        {
            passTime += Time.deltaTime;
            StartGage.fillAmount = passTime / maxTime;
        }else if (!isStart && passTime > 0)
        {
            passTime -= Time.deltaTime;
            StartGage.fillAmount = passTime / maxTime;
        }

        if(passTime == maxTime)
        {
            PitariSceneManager.Instance.ToStage1();
        }
    }

    private void Joycon()
    {
        ShakeGameStart(Input_Player.Instance.RightMove_performed);
    }

    private void Oricon()
    {
        ShakeGameStart(OriconManager.instance.pvcController());
    }

}
