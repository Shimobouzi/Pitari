using UnityEngine;

public class NextStage : MonoBehaviour
{
    public enum StageName
    {
        Stage1,
        Stage2,
        Stage3,
        Clear
    }

    public StageName stage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == ("Player"))
        {
            Debug.Log("次のステージ");
            if (stage == StageName.Stage1)
            {
                PitariSceneManager.Instance.ToStage1();
            }

            if (stage == StageName.Stage2)
            {
                PitariSceneManager.Instance.ToStage2();
            }

            if (stage == StageName.Stage3)
            {
                PitariSceneManager.Instance.ToStage3();
            }

            if (stage == StageName.Clear)
            {
                PitariSceneManager.Instance.ToResult(true);
            }
        }
    }
}
