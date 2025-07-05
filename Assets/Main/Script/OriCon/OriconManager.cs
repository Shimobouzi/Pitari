using UnityEngine;

public class OriconManager : MonoBehaviour
{
    public static OriconManager instance;
    [SerializeField]
    private float accelThreshold = 2.5f;

    /* ���蓯���U�蔻��p�ϐ� ------------------------- */
    bool leftSwing = false;
    bool rightSwing = false;
    float swingTimer = 0f;
    const float swingWindow = 0.3f;   // ���肪���낤���e���ԁi�b�j
    /* -------------------------------------------------- */
    PicoSensorHub hub;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        hub = FindFirstObjectByType<PicoSensorHub>();
    }

    public bool pvcController()
    {
        /* hub ���擾�ł��Ă��Ȃ��ꍇ�͉������Ȃ� */
        if (hub == null) return false;

        // ���r�R���g���[���[�F���E���ꂼ��̉����x
        Vector3 l = hub.leftAcc;
        Vector3 r = hub.rightAcc;

        /* ---- ���E�ǂ��炩���������l��������t���O ---- */
        if (l.magnitude > accelThreshold) leftSwing = true;
        if (r.magnitude > accelThreshold) rightSwing = true;

        /* ---- �������낦�ΑO�i ---- */
        if (leftSwing && rightSwing)
        {
            return true;
            /* �t���O�ƃ^�C�}�[�����Z�b�g 
            leftSwing = rightSwing = false;
            swingTimer = 0f;*/
        }
        else
        {
            /* �Ў肾���U���Ă����ԂŃ^�C�}�[���� */
            if (leftSwing || rightSwing)
            {
                swingTimer += Time.deltaTime;
                if (swingTimer > swingWindow)
                {
                    /* �K�莞�ԓ��ɂ����Е������Ȃ����� �� �t���O������ */
                    leftSwing = rightSwing = false;
                    swingTimer = 0f;
                }
            }
        }

        return false;

        
    }
}


