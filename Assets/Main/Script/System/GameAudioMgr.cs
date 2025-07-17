using UnityEngine;

public class GameAudioMgr : MonoBehaviour 
{
    private void Awake()
    {
        AudioManager.BgmVolume = 0.5f;
        AudioManager.SEVolume = 0.5f;
        // サウンドをロード
        AudioManager.LoadBgm("bgmT", "n_bgm_title");
        AudioManager.LoadBgm("bgmG", "gakkou_bgm_loop");
        AudioManager.LoadBgm("bgmE", "eki_main_bgm");
        AudioManager.LoadBgm("bgmJ", "pitari_jinja_main_bgm");
        AudioManager.LoadSE("playerWalk", "gakkou_player_ashioto_wood");
        AudioManager.LoadSE("hayaoniPre", "gakkou_hayaoni_ashioto");
    }

    private void Start()
    {
        AudioManager.PlayBgm("bgmT");
    }

}
