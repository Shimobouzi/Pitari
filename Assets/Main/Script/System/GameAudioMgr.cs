using UnityEngine;

public class GameAudioMgr : MonoBehaviour 
{
    private void Awake()
    {
        AudioManager.BgmVolume = 0.5f;
        AudioManager.SEVolume = 0.5f;
        // サウンドをロード
        AudioManager.LoadBgm("bgmT", "pitari_BGM_gakkou_loop");
        AudioManager.LoadSE("playerWalk", "gakkou_player_ashioto_wood");
        AudioManager.LoadSE("hayaoniPre", "gakkou_hayaoni_ashioto");
    }

    private void Start()
    {
        AudioManager.PlayBgm("bgmT");
    }

}
