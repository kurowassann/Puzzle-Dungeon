using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

/// <summary>�Q�[���𓮂���</summary>
public class GameManager : MonoBehaviour 
{
    [Tooltip("�f�o�b�N�m�F�t���O"), SerializeField]
    private bool isDebug;

    //�萔
    [Tooltip("�}�b�v�̊Ǘ��I�u�W�F�N�g"),SerializeField]
    private MapManager cMm;
    [Tooltip("�v���C���Ǘ��I�u�W�F�N�g"), SerializeField]
    private Player cPlayer;
    [Tooltip("�G�Ǘ��I�u�W�F�N�g"), SerializeField]
    private EnemyManager cEm;
    

    //�����o�ϐ�
    /// <summary>�X�e�[�^�X�ύX���̏����p</summary>
    private bool isStatusChange;
    /// <summary>�X�e�[�^�X</summary>
    private GameStatus mStatus;
    /// <summary>�^�[���I���p�^�C�}�[</summary>
    private float mEndTimer;
    

    //�����o�֐�
    //private

    /// <summary>�X�e�[�^�X���̕���</summary>
    private void StatusBranch()
    {
        switch (mStatus) 
        {
            case GameStatus.STAY:
                BranchStay();
                break;
            case GameStatus.ACTION:
                BranchAction();
                break;
            case GameStatus.COUNT:
                BranchCount();
                break;
        }
    }
    /// <summary>�X�e�C��</summary>
    private void BranchStay()
    {
        if(isStatusChange)
        {
            if (isDebug) 
            {
                print("�Q�[�����[�h�F�ҋ@");
            }
            isStatusChange = false;
        }

    }
    /// <summary>�A�N�V������</summary>
    private void BranchAction()
    {
        if (isStatusChange)
        {
            if (isDebug) 
            {
                print("�Q�[�����[�h�F�A�N�V����");
            }
            isStatusChange = false;
        }

        if (TurnCheck())
        {
            SetStatus(GameStatus.COUNT);
        }
    }
    /// <summary>�J�E���g��</summary>
    private void BranchCount()
    {
        if (isStatusChange)
        {
            if (isDebug) 
            {
                print("�Q�[�����[�h�F�J�E���g");
            }
            mEndTimer = 0;
            isStatusChange= false;
        }
        mEndTimer += Time.deltaTime;
        if(0.1f < mEndTimer)
        {
            TurnEnd();
        }
    }

    /// <summary>�^�[���̏I�����m�F</summary>
    private bool TurnCheck()
    {
        if(cPlayer.GetAction() && cEm.GetAction())
        {
            return true;
        }

        return false;
    }
    /// <summary>�^�[���̏I��</summary>
    private void TurnEnd()
    {
        cPlayer.TurnEnd();

        SetStatus(GameStatus.STAY);
    }

    //public
   public void PlayerActionEnd()
    {
        SetStatus(GameStatus.ACTION);
        cEm.EnemyActionSelect();
    }
    public void RoomOpen(int num)
    {
        cEm.Spawn(num);
    }

    //Set�֐�
    /// <summary>�X�e�[�^�X�`�F���W</summary>
    private void SetStatus(GameStatus tstatus)
    {
        mStatus = tstatus;
        isStatusChange = true;
    }

    //Get�֐�
    public bool GetDebug()
    {
        return isDebug;
    }
    public Point GetPlayer()
    {
        return cPlayer.GetPos();
    }

    /// <summary>�N�����ɌĂ�</summary>
    private void Start()
    {

        mStatus = new GameStatus();
        mStatus = GameStatus.STAY;
       isStatusChange = false;
        mEndTimer = 0;

        //�}�b�v�̐���
        cMm.Init(this);

        //�v���C���̐���
        //�����ʒu�̌���
        PosId posId = cMm.GetRandomPlayer();
        cPlayer.Init(this, cMm,posId,1,"p");


        //�G�Ǘ�������
        cEm.Init(this,cMm);



    }

    //
    private void Update()
    {
        StatusBranch();   

    }
}
