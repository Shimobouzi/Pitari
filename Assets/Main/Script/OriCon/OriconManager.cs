using UnityEngine;

public class OriconManager : MonoBehaviour
{
    public static OriconManager instance;
    [SerializeField]
    private float accelThreshold = 2.5f;

    [SerializeField]
    private float DashAT = 5f;

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

    /// <summary>
    /// ���r�R���g���[���[�ł̗���U�蓮������o���A�����𖞂������ꍇ��true��Ԃ�
    /// </summary>
    public bool pvcController()
    {

        if (hub == null) return false;

        Vector3 l = hub.leftAcc;
        Vector3 r = hub.rightAcc;

        // �����x��臒l�𒴂�����U�����Ɣ���
        if (l.magnitude > accelThreshold) leftSwing = true;
        if (r.magnitude > accelThreshold) rightSwing = true;

        // ���肪�����ĐU��ꂽ�ꍇ �� �����itrue�j
        if (leftSwing && rightSwing)
        {
            leftSwing = rightSwing = false;
            swingTimer = 0f;
            return true;
        }
        // �ǂ���̎���U���Ă��Ȃ��ꍇ �� ���s�ifalse�j
        else if (!leftSwing && !rightSwing)
        {
            return false;
        }
        else
        {
            // �Е������U���Ă����� �� �^�C�}�[�𑪒�
            swingTimer += Time.deltaTime;

            if (swingTimer > swingWindow)
            {
                // �����Е������ԓ��ɗ��Ȃ����� �� �t���O�𖳌������ă��Z�b�g
                leftSwing = rightSwing = false;
                swingTimer = 0f;
            }

            // ���o�O�C���|�C���g�F
            // �ȑO�̃R�[�h�ł́A������return true�Ƃ��Ă�������
            // �u�Ў肾���U���������ł�true���Ԃ�v��ԂɂȂ��Ă����B
            // ���ʂƂ��āA�O�i�A�j���[�V�������Ӑ}�����������Ă����B
            return false; // ���肪����Ȃ������̂�false��Ԃ�
        }

        // ���ȑO�̃R�[�h�ł͂�����return true���Ă������A���ׂĂ̕����return���邽�ߕs�v�B
    }


    public bool pvcDash()
    {
        Debug.Log(accelThreshold);

        if (hub == null) return false;

        Vector3 l = hub.leftAcc;
        Vector3 r = hub.rightAcc;

        // �����x��臒l�𒴂�����U�����Ɣ���
        if (l.magnitude > DashAT) leftSwing = true;
        if (r.magnitude > DashAT) rightSwing = true;

        // ���肪�����ĐU��ꂽ�ꍇ �� �����itrue�j
        if (leftSwing && rightSwing)
        {
            leftSwing = rightSwing = false;
            swingTimer = 0f;
            return true;
        }
        // �ǂ���̎���U���Ă��Ȃ��ꍇ �� ���s�ifalse�j
        else if (!leftSwing && !rightSwing)
        {
            return false;
        }
        else
        {
            // �Е������U���Ă����� �� �^�C�}�[�𑪒�
            swingTimer += Time.deltaTime;

            if (swingTimer > swingWindow)
            {
                // �����Е������ԓ��ɗ��Ȃ����� �� �t���O�𖳌������ă��Z�b�g
                leftSwing = rightSwing = false;
                swingTimer = 0f;
            }

            // ���o�O�C���|�C���g�F
            // �ȑO�̃R�[�h�ł́A������return true�Ƃ��Ă�������
            // �u�Ў肾���U���������ł�true���Ԃ�v��ԂɂȂ��Ă����B
            // ���ʂƂ��āA�O�i�A�j���[�V�������Ӑ}�����������Ă����B
            return false; // ���肪����Ȃ������̂�false��Ԃ�
        }

        // ���ȑO�̃R�[�h�ł͂�����return true���Ă������A���ׂĂ̕����return���邽�ߕs�v�B
    }

    public void GetPicoHub()
    {
        hub = FindFirstObjectByType<PicoSensorHub>();
    }

}


