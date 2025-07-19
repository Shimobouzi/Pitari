using UnityEngine;

public class SoundLoader : MonoBehaviour
{
    [SerializeField] private string csvPath = "sound_data";

    private void Awake()
    {
        LoadCsv();
    }

    private void LoadCsv()
    {
        //CSV�ǂݍ���
        TextAsset csvFlie = Resources.Load<TextAsset>(csvPath);

        if(csvFlie == null)
        {
            Debug.LogError("CSV���擾�ł��܂���ł���");
            return;
        }

        string[] lines = csvFlie.text.Split('\n');  //���s�ŕ�����

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();          //�󔒂̏���
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            if(i == 0)                              //�w�b�_�[����
            {
                continue;
            }

            string[] parts = line.Split(',');

            if (parts.Length < 3)
            {
                Debug.LogError("CSV���s�\���ł�");
                continue;
            }

            string key = parts[0].Trim();
            string type = parts[1].Trim().ToLower();
            string file = parts[2].Trim();
            string channelStr = parts.Length > 3 ? parts[3].Trim() : "";

            if(!int.TryParse(channelStr, out int channel))
            {
                Debug.LogWarning("channel�̐��l�𐳂����擾�ł��܂���");
                channel = -1;
            }

            Debug.Log($"[{i}] key:{key}, type:{type}, file:{file}, channel:{channel}");

            if (type == "bgm")
            {
                AudioManager.LoadBgm(key, file);
            }
            else if (type == "se")
            {
                AudioManager.LoadSE(key, file, channel);
            }
            else
            {
                Debug.LogWarning($"����`��type�ł�: {type}�i�s: {i + 1}�j");
            }
        }
    }
}
