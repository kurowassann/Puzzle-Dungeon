using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    [Tooltip("�A�C�e���m�F�p�t���O"), SerializeField]
    private bool isItem;

    //�萔
    [Tooltip("�}�b�v�̊Ǘ��I�u�W�F�N�g"),SerializeField]
    private MapManager cMm;
    [Tooltip("�v���C���Ǘ��I�u�W�F�N�g"), SerializeField]
    private Player cPlayer;
    [Tooltip("�G�Ǘ��I�u�W�F�N�g"), SerializeField]
    private EnemyManager cEm;
    [Tooltip("�A�C�e���Ǘ��I�u�W�F�N�g"), SerializeField]
    private BonusManager cBm;
    [Tooltip("�A�C�e���I�u�W�F�N�g"), SerializeField]
    private GameObject oItem;
    

    //�����o�ϐ�
    /// <summary>�X�e�[�^�X�ύX���̏����p</summary>
    private bool isStatusChange;
    /// <summary>�X�e�[�^�X</summary>
    private GameStatus mStatus;
    /// <summary>�^�[���I���p�^�C�}�[</summary>
    private float mEndTimer;
    //
    private GameObject mItem;
    /// <summary>�A�C�e���̍��W���X�g</summary>
    private Point mItemPos;

    

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

    /// <summary>�A�C�e���̐���</summary>
    private void ItemSpawn(int num)
    {
        mItem.SetActive(true);

        Point pos = cMm.GetPosId(num).GetPos();

        mItemPos = pos;

        Vector3 vec = cMm.GetTilePos(pos);

        mItem.transform.position = vec;
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
    //
    public void EnemyAttack()
    {
        cPlayer.Damage();
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
        if(isEnemy)
        {
            cEm.Spawn(num);
        }

        if(isItem)
        {
            ItemSpawn(num);
        }
    }
    //
    public void ItemCheck(Point tpos)
    {
        if (mItemPos == tpos)//�ړ��ꏊ�ɃA�C�e����������
        {
            switch( cBm.LotNextBonus1())
            {
                case AllBonus.HEELUP:
                    cPlayer.HpUp();
                    break;
            }



            //�A�C�e������
                mItemPos = new Point(0, 0);
            mItem.SetActive(false);
        }
        
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

        //�A�C�e���𐶐����Ă���
        var clone = Instantiate(oItem, this.transform);
        mItem = clone;
        mItem.SetActive(false);
        mItemPos = new Point(0,0);

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
