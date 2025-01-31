using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Common;

public class Enemy : CharacterBace
{
    
    //�I�u�W�F�N�g
    protected EnemyManager cEm;
    protected Animator animator;

    //�����o�ϐ�
    [SerializeField]
    string eName; 
    /// <summary>�J�������ɑ��݂��Ă��邩</summary>
    protected bool mVisible;
    /// <summary>���ɍs���s��</summary>
    protected Status mNextAction;
    /// <summary>�ړ�����</summary>
    protected Vector mVec;
    /// <summary>�������Ă������</summary>
    private Vector mDir;
    /// <summary>�v���C���[�܂ł̌o�H�i�[�z��</summary>
    protected Common.Vector[] mRoute;
    /// <summary>�o�H�̂ǂ��ɂ��邩</summary>
    protected int mRouteNum;
    /// <summary>���X�|�[���܂ł̃J�E���g</summary>
    protected int mRespawnCount;

    //�����o�֐�
    //����������
    virtual public void Init(EnemyManager tem, GameManager tgm, MapManager tmm, PosId tposId, int thp, string tstr)
    {
        base.Init(tgm,  tmm,  tposId,  thp,  tstr);

        print("�G�l�~�[�̏�����");
        cEm = tem;
        mRoute = new Common.Vector[0];
        mRouteNum = 0;
        mNextAction = Status.STAY;
        mRespawnCount = 0;

        animator = GetComponent<Animator>();
        mVec = Vector.NONE;
        mDir = Vector.DOWN;

        //animator.SetTrigger("Reset");

    }      
    /// <summary>���̃^�[���̍s�������肷��</summary>
    public bool Tracking()
    {
        if (mRoute.Length == mRouteNum)
        {
            print("���[�g�̖��[�ł�");
            return false;
        }
        if (mNextAction != Status.STAY)
        {
            print("���łɍs�������܂��Ă��܂��B");
            return false;
        }
        Point point = mPosId.GetPos();

        //���̈ړ����
        switch(mRoute[mRouteNum])
        {
            case Common.Vector.UP:
                point.Y--;
                mVec = Vector.UP;
                break;
            case Common.Vector.DOWN:
                point.Y++;
                mVec = Vector.DOWN;
                break;
            case Common.Vector.LEFT:
                point.X--;
                mVec = Vector.LEFT;
                break;
            case Common.Vector.RIGHT:
                point.X++;
                mVec = Vector.RIGHT;
                break;
        }

        if (mDir != mVec && eName == "Ant")
        {
            mNextAction = Status.TRUN;
            mDir = mVec;
            //mGoalPos = transform.position;
            return true;
        }

        if (cMm.CheckTile(point) == Common.TileInfo.ROUTE)
        {
            SetPos(point);
            mNextAction = Status.MOVE;
            mRouteNum++;
            return true;
        }
        if (cMm.CheckTile(point) == Common.TileInfo.PLAYER)
        {
            Debug.Log("�ړ��̕����̓v���C���[�ł�");
            mNextAction = Status.ATTACK;
            //master.EnemyAttack();

            return true;
        }
        return false;
    }
    /// <summary>�ړ��I������</summary>
    protected override void MoveEnd()
    {
        base.MoveEnd();
        var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string triggerName = "";
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_MOVE_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_END_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_ATTACK_DIR);
        //print("aaa" + animationName);

        triggerName = SwitchMoveEnd(animationName);

        animator.SetTrigger(triggerName);
    }
    /// <summary>�ړ��I���A�j���[�V��������</summary>
    virtual protected string SwitchMoveEnd(string _animName)
    {
        //print(_animName);
        string triggerName = "";

        if (_animName == "MoveRight@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];

        }
        else if (_animName == "MoveLeft@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];

        }
        else if (_animName == "MoveUp@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];

        }
        else if (_animName == "MoveDown@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];

        }

        //switch (_animName)
        //{
        //    case "MoveRight@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];
        //        break;
        //    case "MoveLeft@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];
        //        break;
        //    case "MoveUp@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];
        //        break;
        //    case "MoveDown@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];
        //        break;
        //}

        //print("Enemy�ړ��I�����̃A�j���[�V����:" + triggerName);

        return triggerName;
    }
    /// <summary>�U���I������</summary>
    protected override void AttackEnd()
    {
        base.AttackEnd();
        //cEm.EnemyAttack();
        
        var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string triggerName = "";
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_MOVE_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_END_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_ATTACK_DIR);
        print("aaa" + animationName);
        triggerName = SwitchAttackEnd(animationName);

        print("Enemy�U���I�����̃A�j���[�V����:" + triggerName);

        animator.SetTrigger(triggerName);
    }   
    /// <summary>�U���I���A�j���[�V��������</summary>
    virtual protected string SwitchAttackEnd(string _animName)
    {
        string triggerName = "";

        if (_animName == "AttackRight@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];

        }
        else if( _animName == "AttackLeft@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];

        }
        else if (_animName == "AttackUp@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];

        }
        else if (_animName == "AttackDown@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];

        }

        //switch (_animName)
        //{
        //    case "AttackRight@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];
        //        break;
        //    case "AttackLeft@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];
        //        break;
        //    case "AttackUp@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];
        //        break;
        //    case "AttackDown@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];
        //        break;
        //}

        print("Enemy�U���I�����̃A�j���[�V����:" + triggerName);

        return triggerName;
    }   
    /// <summary>���X�|�[���܂ł̃J�E���g���s�������B���Ń��X�|�[��������</summary>
    public void RespawnCount()
    {
        //mRespawnCount++;
        if (5 < mRespawnCount)
        {
            Debug.Log("���X�|�[�����܂�");
            this.gameObject.SetActive(true);
            isActive = true;
            mRespawnCount = 0;
            cEm.Generate("e", this);
        }
    }  
    //���X�|�[��
    public void Respawn()
    {

    }
    /// <summary>�o�H�̃��Z�b�g</summary>
    public void ResetRoute()
    {
        Array.Resize(ref mRoute, 0);
        mRouteNum = 0;
    }
    /// <summary>�o�H�L�^�ǉ�</summary>
    public void AddRoute(Common.Vector tvec)
    {
        Array.Resize(ref mRoute, mRoute.Length + 1);
        mRoute[mRoute.Length - 1] = tvec;
    }
    //�L�^����Ă��郋�[�g�̕\��
    public void ShowRoute()
    {
        for (int i = 0; i < mRoute.Length; i++)
        {
            Debug.Log(mRoute[i]);
        }
    }

    //Set�֐�
    /// <summary>���肵���s����\��</summary>
    public void SetAction(Status tstatus)
    {
        mNextAction = tstatus;
    }
    /// <summary>�s���̒���</summary>
    public override void SetStatus(Status tstatus)
    {
        base.SetStatus(tstatus);
        string triggerName = "";
        switch (tstatus)
        {
            case Status.ATTACK:
                triggerName = Common.Common.CHARA_ANIMS_ATTACK_DIR[(int)mVec];
                break;
            case Status.MOVE:
            case Status.TRUN:
                triggerName = Common.Common.CHARA_ANIMS_MOVE_DIR[(int)mVec];
                break;
			default:
				return;
        }
        //print("�G�̍s���J�n:" + triggerName);
        animator?.SetTrigger(triggerName);
    }
    
    //Get�֐�
    /// <summary>�����o�H�̉���ڂɂ��邩</summary>
    public int GetNum()
    {
        return mRouteNum;
    }
/// <summary>�o�H�����肩</summary>
    public int GetRoute()
    {
        return mRoute.Length;
    }
    /// <summary>���̃^�[���ǂ̍s������邩</summary>
    public Status GetNextAction()
    {
        return mNextAction;
    }
    
}
