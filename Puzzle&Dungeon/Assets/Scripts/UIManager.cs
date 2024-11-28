using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Tooltip("���̉���")]
    private float width = 10;
    [Tooltip("���̏c��")]
    private float height = 10;



    [Header("�~�j�}�b�v�n")]
    [Tooltip("�~�j�}�b�v�p�l��")]
    private GameObject mapPanel;


    [Header("�O���b�h���n")]
    // todo:�O���b�h��
    [Tooltip("�O���b�h�p��LineRendererPrefab")]
    private GameObject gridPrefab;
    [Tooltip("�O���b�h�����p�̐e�I�u�W�F�N�g")]
    private Transform gridTransform;



    [Header("HP�n")]
    [Tooltip("HP�v���n�u")]
    private GameObject playerHPPrefab;
    [Tooltip("�v���C���[��HP�̐e�I�u�W�F�N�g")]
    private Transform playerHPParent;
    [Tooltip("HP�̃��X�g")]
    private List<Transform> playerHPList;


    [Header("���O�n")]
    [Tooltip("���O�̍ő�ۑ���")]
    private const int MAX_LOG_NAM = 5;
    [Tooltip("���O���\������鎞��")]
    private const float LOG_ACTIVE_TIME = 1.2f;
    [Tooltip("���O�������鎞��")]
    private const float LOG_FADE_OUT = 0.5f;
    [Tooltip("���O�\���p�̃^�C�}�[")]
    private float logTimer = 0; 
    [Tooltip("���O�̃p�l��")]
    private Image logPanel;
    [Tooltip("�o�͐�̃e�L�X�g�I�u�W�F�N�g")]
    private Text logTextOutputObj;
    [Tooltip("���O�̃��X�g")]
    private List<string> logTexts;

    //todo:�U���A�|�����A

    // Start is called before the first frame update
    void Start()
    {
        // UI�̏�����
        UIInit();


        //GeneratePlayerHP(2);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    AddLog("�v���C���[�̍U��");
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    AddLog("�G�̍U��");
        //}

        if(Input.GetKeyDown(KeyCode.M))
        {
            mapPanel.SetActive(!mapPanel.activeSelf);
        }

        // ���O�̕\���𐧌�
        LogControll();
    }

    /// <summary>
    /// UI�̏���������
    /// </summary>
    void UIInit()
    {
        mapPanel = GameObject.Find("PanelMap");
        mapPanel.SetActive(true);

        playerHPPrefab = Resources.Load<GameObject>("Prefabs/HPImagePrefab");
        playerHPParent = GameObject.Find("PlayerHPParent").transform;

        logPanel = GameObject.Find("PanelLog").GetComponent<Image>();
        var col = logPanel.color;
        col.a = 0;
        logPanel.color = col;

        logTextOutputObj = GameObject.Find("TextLog").GetComponent<Text>();
        col = logTextOutputObj.color;
        col.a = 0;
        logTextOutputObj.color = col;

        logTexts = new List<string>();
        logTimer = 0;

        // �O���b�h���̕`��
        //CreateGrid();
    }

    /// <summary>
    /// �O���b�h���`�揈��
    /// </summary>
    void CreateGrid()
    {
        // Prefab�ǂݍ���
        gridPrefab = Resources.Load<GameObject>("Prefabs/Line");
        // ��������e�I�u�W�F�N�g�擾
        gridTransform = transform.GetChild(0);

        // �c���̕`��
        for(int i = 0; i <= width; i++)
        {
            var line = Instantiate(gridPrefab, gridTransform).GetComponent<LineRenderer>();
            line.startWidth = 0.01f;
            line.endWidth = 0.01f;
            Vector3[] linePoss = new Vector3[2]
            {
                new Vector3(i,       0, 0),
                new Vector3(i, -height, 0),
            };
            line.SetPositions(linePoss);
        }

        // �����̕`��
        for (int i = 0; i <= height; i++)
        {
            var line = Instantiate(gridPrefab, gridTransform).GetComponent<LineRenderer>();
            line.startWidth = 0.01f;
            line.endWidth = 0.01f;
            Vector3[] linePoss = new Vector3[2]
            {
                new Vector3(    0, -i, 0),
                new Vector3(width, -i, 0),
            };
            line.SetPositions(linePoss);
        }
    }



    /// <summary>
    /// �v���C���[����������HP�𐶐����鏈��
    /// </summary>
    /// <param name="_maxHP">HP�̍ő�l</param>
    public void GeneratePlayerHP(int _maxHP)
    {
        playerHPList = new List<Transform>();
        playerHPPrefab = Resources.Load<GameObject>("Prefabs/HPImagePrefab");
        playerHPParent = GameObject.Find("PlayerHPParent").transform;
        for (int i = 0; i < _maxHP; i++)
        {
            var obj = Instantiate(playerHPPrefab, playerHPParent);
            playerHPList.Add(obj.transform);
        }
    }



    /// <summary>
    /// �v���C���[��HP��UI�ɕ\�����鏈��
    /// </summary>
    /// <param name="_currentHP">���݂�HP</param>
    public void DisplayPlayerHP(int _currentHP)
    {
        foreach (var obj in playerHPList)
        {
            obj.GetChild(1).gameObject.SetActive(false);
        }

        for (int i = 0; i < _currentHP; i++)
        {
            playerHPList[i].GetChild(1).gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// ���O���䏈��
    /// ���Ԃŏ��X�ɏ�����
    /// </summary>
    void LogControll()
    {
        // �^�C�}�[�����̊Ԃ͕\����������
        if (logTimer > 0)
        {
            // �^�C�}�[�̌��Z
            logTimer -= Time.deltaTime;
            // ���O�̕\��
            logPanel.gameObject.SetActive(true);
            var col = logPanel.color;
            col.a = 1;
            logPanel.color = col;
            logTextOutputObj.gameObject.SetActive(true);
            col = logTextOutputObj.color;
            col.a = 1;
            logTextOutputObj.color = col;

        }
        else
        {
            // �t�F�[�h�A�E�g�����鏈��
            if (logTextOutputObj.color.a > 0)
            {
                var col = logTextOutputObj.color;
                col.a -= Time.deltaTime / LOG_FADE_OUT;
                logTextOutputObj.color = col;
                col = logPanel.color;
                col.a -= Time.deltaTime / LOG_FADE_OUT;
                logPanel.color = col;
            }
            else
            {
                logPanel.gameObject.SetActive(false);
                logTextOutputObj.gameObject.SetActive(false);
            }
        }
    }


    /// <summary>
    /// ���O��ǉ����鏈��
    /// �ǉ����ɂ��ӂꂽ���������ō폜����
    /// </summary>
    /// <param name="_logText">�ǉ�����e�L�X�g</param>
    public void AddLog(string _logText)
    {
        // ���O�̒ǉ�
        logTexts.Add(_logText);
        // �Â����̂��璴�߂��Ă��镪�폜
        if (logTexts.Count > MAX_LOG_NAM)
        {
            logTexts.RemoveRange(0, logTexts.Count - MAX_LOG_NAM);
        }


        // �\�������̎��s
        if (logTexts.Count != 0)
        {
            DisplayLog();
        }
    }

    /// <summary>
    /// ���O��\�����鏈��
    /// </summary>
    void DisplayLog()
    {
        // �^�C�}�[�̃Z�b�g
        logTimer = LOG_ACTIVE_TIME;
        // �\�����O�̏���
        var logBuilder = new StringBuilder();
        // �e�L�X�g�ɉ��s�t���ň�s���ǉ�
        foreach (var log in logTexts)
        {
            logBuilder.AppendLine(log);
        }
        // �o��
        logTextOutputObj.text = logBuilder.ToString().TrimEnd();
    }
}
