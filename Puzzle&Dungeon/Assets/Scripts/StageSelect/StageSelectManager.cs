using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


using Data;

public class StageSelectManager : MonoBehaviour
{
    const float TMP_SIZE = 2.6f / 1.7f;


    enum CursorLine
    {
        OPTIONS,
        STAGES,
    }


    [Header("�X�e�[�W�I���n")]
    [Tooltip("�X�e�[�W�̍ő吔")]
    const float MAX_STAGE_NUM = 4;
    [Tooltip("�X�e�[�W�_�̐e"), SerializeField]
    Transform stagesParent;
    [Tooltip("�X�e�[�W�_��Prefab"), SerializeField]
    GameObject stagePointPrefab;
    [Tooltip("�X�e�[�W�_�̃��X�g")]
    List<GameObject> stages;

    [Header("�X�e�[�W�I���̃{�^���n")]

    [Tooltip("�{�^���̃��X�g"), SerializeField]
    List<StageSelectButton> stageSelectButtons;
    [Tooltip("���{�^���̃A�j���[�^�["), SerializeField]
    Animator leftAnimator;
    [Tooltip("�E�{�^���̃A�j���[�^�["), SerializeField]
    Animator rightAnimator;


    [Tooltip("���ݑI�𒆂̃X�e�[�W�ԍ�,0�n�܂�"), SerializeField]
    public int currentIndex;



    [Header("�X�e�[�W�̏ڍ׏��n")]
    [Tooltip("�X�e�[�W���̃}�X�^�[�f�[�^")]
    JsonStageMaster jsonStageMaster;
    [Tooltip("�G�̃}�X�^�[�f�[�^")]
    JsonEnemyMaster jsonEnemyMaster;
    [Tooltip("�X�e�[�W�̏ڍ׃p�l��"), SerializeField]
    GameObject stageDetailPanel;
    [Tooltip("�X�e�[�W��"), SerializeField]
    Text stageName;
    [Tooltip("�X�e�[�W�̐����e�L�X�g"), SerializeField]
    Text detailText;
    [Tooltip("�X�e�[�W�ɏo�Ă���G���A�C�R��"), SerializeField]
    Image detailWeakEnemyIcon;
    [Tooltip("�X�e�[�W�ɏo�Ă���{�X�A�C�R��"), SerializeField]
    Image detailBossEnemyIcon;



    // Start is called before the first frame update
    void Start()
    {
        // ������
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!OptionManager.Instance.isOptionOpened)
        {
            // ���͂̎󂯎��
            if (SelectInput())
            {
                // �X�e�[�W���̕\��
                StageDetailDisplay();
                UIScaler();
            }
        }
        
    }

    /// <summary>
    /// ����������
    /// </summary>
    void Init()
    {
        // �X�e�[�W���̎擾
        jsonStageMaster = LoadData.LoadAllStageData();
        // �G���擾
        jsonEnemyMaster = LoadData.LoadAllEnemyData();

        // �X�e�[�W�_�̐���
        StagePointGen();
        // todo:���O�ɃN���A�����X�e�[�W�̃J�[�\���ʒu���L������
        currentIndex = 0;

        // �{�^���̎擾
        // StageSelectButton�^��S�擾
        stageSelectButtons = FindObjectsOfType<StageSelectButton>().ToList();


        // �X�e�[�W���̕\��
        StageDetailDisplay();
        
        UIScaler();
    }

    /// <summary>
    /// �X�e�[�W�̐������|�C���g�𐶐�
    /// </summary>
    void StagePointGen()
    {
        if (jsonStageMaster == null)
        {
            print("�X�e�[�W��񂪓ǂݍ��߂ĂȂ�");
            return;
        }
        stages = new List<GameObject>();
        // ����
        for (int i = 0; i < jsonStageMaster.stage_datas.Length; i++)
        {
            
            if (jsonStageMaster.stage_datas[i].stage_id != Common.Common.ERROR_ID)
            {
                var stage = Instantiate(stagePointPrefab, stagesParent);
                if (stage != null)
                {
                    var stageIdName = jsonStageMaster.stage_datas[i].stage_id_name;
                    stage.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + stageIdName + "_BG");
                    stages.Add(stage);
                }
                else
                {
                    print("�����Ɏ��s");
                }
            }
        }
    }


    /// <summary>
    /// �X�e�[�W�I����Ԃ̓��͏���
    /// </summary>
    bool SelectInput()
    {
        // todo:���_���ړ��ł���A�[�̓��[�v
        var ret = false;

        // �J�[�\�������Ɉړ�
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = stages.Count - 1;
            }
            leftAnimator.SetTrigger("Push");
            ret = true;
        }

        // �J�[�\�����E�Ɉړ�
        if (Input.GetKeyDown(KeyCode.D))
        {

            currentIndex++;
            if (currentIndex > stages.Count - 1)
            {
                currentIndex = 0;
            }
            rightAnimator.SetTrigger("Push");

            ret = true;
        }

        // �e�{�^���̏���
        foreach (var button in stageSelectButtons)
        {
            // �{�^���ɑΉ�����L�[��������Ă���
            if (Input.GetKey(button.GetKeyCode()))
            {
                // �^�C�}�[�����߂�����
                if(button.AddTime())
                {
                    // ��������������
                    button.GetAction().Invoke();
                    button.ResetTimer();
                }
            }
            else
            {
                // �^�C�}�[�̌��Z
                button.SubTime();
            }
        }
        return ret;
    }

    /// <summary>
    /// �T�C�Y�����ŃJ�[�\����\��
    /// </summary>
    void UIScaler()
    {
        // �X�e�[�W�̃T�C�Y�ύX
        for (int i = 0; i < stages.Count; i++)
        {
            if (i == currentIndex)
            {
                stages[i].transform.localScale = Vector3.one * TMP_SIZE;
            }
            else
            {
                stages[i].transform.localScale = Vector3.one;
            }
        }
    }

    /// <summary>
    /// �X�e�[�W�̏ڍ׏��\��
    /// </summary>
    void StageDetailDisplay()
    {
        stageName.text = jsonStageMaster.stage_datas[currentIndex].stage_name;
        detailText.text = jsonStageMaster.stage_datas[currentIndex].stage_desc;
        var len = jsonStageMaster.stage_datas[currentIndex].enemy_datas.Length;
        var cnt = 0;
        for(int i = 0; i < len; i++)
        {
            if (jsonStageMaster.stage_datas[currentIndex].enemy_datas[i] != null)
            {
                cnt++;
            }
        }
        if(cnt < 2)
        {
            var eId = jsonStageMaster.stage_datas[currentIndex].enemy_datas[0].enemy_id;
            var eName = jsonEnemyMaster.GetEnemyData(eId).enemy_id_name;

            detailWeakEnemyIcon.sprite = Resources.Load<Sprite>("Images/Enemy/" + eName + "/" +eName +  "_anim01");
            detailWeakEnemyIcon.gameObject.SetActive(true);
            detailBossEnemyIcon.gameObject.SetActive(false);
        }
        else
        {
            var eId = jsonStageMaster.stage_datas[currentIndex].enemy_datas[0].enemy_id;
            var eName = jsonEnemyMaster.GetEnemyData(eId).enemy_id_name;
            detailWeakEnemyIcon.sprite = Resources.Load<Sprite>("Images/Enemy/" + eName + "/" + eName + "_anim01");

            //print("Images/Enemy/" + eName + "/" + eName + "_anim01");

            eId = jsonStageMaster.stage_datas[currentIndex].enemy_datas[1].enemy_id;
            eName = jsonEnemyMaster.GetEnemyData(eId).enemy_id_name;
            detailBossEnemyIcon.sprite = Resources.Load<Sprite>("Images/Enemy/" + eName + "/" + eName + "_anim01");

            detailWeakEnemyIcon.gameObject.SetActive(true);
            detailBossEnemyIcon.gameObject.SetActive(true);

        }
    }

    /// <summary>
    /// ����{�^��������
    /// </summary>
    public void OnSubmitButton()
    {
        print("�X�e�[�W���莞�̏���");
        var stageId = jsonStageMaster.stage_datas[currentIndex].stage_id;
        PlayerPrefs.SetInt(Common.Common.KEY_SELECTED_STAGE_ID, stageId);
        PlayerPrefs.Save();
        
        Common.Common.LoadScene("GameScene");

    }

    /// <summary>
    /// �^�C�g���ɖ߂�{�^��
    /// </summary>
    public void OnBackTitleButton()
    {
        Common.Common.LoadScene("Title");
    }

    /// <summary>
    /// �I�v�V�����{�^��
    /// </summary>
    public void OnOptionButton()
    {
        OptionManager.Instance.OpenOption();
    }
}