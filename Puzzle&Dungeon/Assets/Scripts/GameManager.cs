using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

/// <summary>�Q�[���𓮂���</summary>
public class GameManager : MonoBehaviour 
{
    //�f�o�b�N�p
    [Tooltip("�f�o�b�N�m�F�t���O"), SerializeField]
    private bool isDebug;
    [Tooltip("�v���C���m�F�t���O"), SerializeField]
    private bool isPlayer;
    [Tooltip("�G�m�F�t���O"), SerializeField]
    private bool isEnemy;

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
        if(!isEnemy && cPlayer.GetAction())
        {
            return true;
        }
        else if(cPlayer.GetAction() && cEm.GetAction())
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
    /// <summary>�v���C���̍U��</summary>
    public void PlayerAttack(Point tpos)
    {
        cEm.Damege(tpos);
    }
    /// <summary>�v���C���̍s���̊������󂯎��</summary>
   public void PlayerActionEnd()
    {
        SetStatus(GameStatus.ACTION);
        if (isEnemy)
        {
            cEm.EnemyActionSelect();
        }

    }
    /// <summary>���������������</summary>
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
        //�ϐ��̏�����
        mStatus = new GameStatus();
        SetStatus(GameStatus.STAY);
        mEndTimer = 0;

        //�}�b�v�̐���
        cMm.Init(this);

        //�v���C���̐���
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
