using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

/// <summary>�Q�[���𓮂���</summary>
public class GameManager : MonoBehaviour 
{ 

    //�I�u�W�F�N�g
    [Tooltip("�}�b�v�̊Ǘ��I�u�W�F�N�g"),SerializeField]
    private MapManager cMm;
    [Tooltip("�v���C���Ǘ��I�u�W�F�N�g"), SerializeField]
    private Player cPlayer;

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
            print("�Q�[�����[�h�F�ҋ@");
            isStatusChange = false;
        }

    }
    /// <summary>�A�N�V������</summary>
    private void BranchAction()
    {
        if (isStatusChange)
        {
            print("�Q�[�����[�h�F�A�N�V����");
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
            print("�Q�[�����[�h�F�J�E���g");
            mEndTimer = 0;
            isStatusChange= false;
        }
        mEndTimer += Time.deltaTime;
        if(0.3f < mEndTimer)
        {
            TurnEnd();
        }
    }

    /// <summary>�^�[���̏I�����m�F</summary>
    private bool TurnCheck()
    {
        if(cPlayer.GetAction())
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
    }

    //Set�֐�
    /// <summary>�X�e�[�^�X�`�F���W</summary>
    private void SetStatus(GameStatus tstatus)
    {
        mStatus = tstatus;
        isStatusChange = true;
    }

    //Get�֐�

    /// <summary>�N�����ɌĂ�</summary>
    private void Start()
    {

        mStatus = new GameStatus();
        mStatus = GameStatus.STAY;
       isStatusChange = false;
        mEndTimer = 0;

        //�}�b�v�̐���
        cMm.Init();

        //�v���C���̐���
        //�����ʒu�̌���
        PosId posId = cMm.GetRandomPlayer();
        cPlayer.Init(this, cMm,posId,1,"p");
    }

    //
    private void Update()
    {
        StatusBranch();   

    }
}
