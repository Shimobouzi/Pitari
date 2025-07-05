using UnityEngine;
using UnityEngine.UI;

public class StageGage : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject stageEnd;
    [SerializeField]
    private Slider stageGage;

    private float maxDistance;
    void Start()
    {
        maxDistance = Vector2.Distance(player.transform.position, stageEnd.transform.position);
        stageGage.maxValue = maxDistance;
    }

    void Update()
    {
        stageGage.value = maxDistance - Vector2.Distance(player.transform.position, stageEnd.transform.position);
    }
}
