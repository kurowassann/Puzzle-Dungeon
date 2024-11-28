using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Ant : Enemy
{
    //����������
    override public void Init(EnemyManager tem)
    {
        em = tem;
        mRoute = new Common.Vector[0];
        mRouteNum= 0;
        mNextAction = Status.STAY;
        mRespawnCount = 0;

        animator = GetComponent<Animator>();
        mVec = Vector.NONE;

    }


    protected override string SwitchAttackEnd(string _animName)
    {
        string triggerName = "";
        print("Ant�U���̃A�j���[�V����");


        switch (_animName)
        {
            case "AttackRight@Ant":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];
                break;
            case "AttackLeft@Ant":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];
                break;
            case "AttackUp@Ant":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];
                break;
            case "AttackDown@Ant":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];
                break;
        }

        print("Ant�U���I�����̃A�j���[�V����:" + triggerName);
        return triggerName;
    }

    protected override string SwitchMoveEnd(string _animName)
    {
        string triggerName = "";
        print("Ant�ړ��̃A�j���[�V����");

        switch (_animName)
        {
            case "MoveRight@Ant":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];
                break;
            case "MoveLeft@Ant":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];
                break;
            case "MoveUp@Ant":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];
                break;
            case "MoveDown@Ant":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];
                break;
        }

        print("Ant�ړ��I�����̃A�j���[�V����:" + triggerName);

        return triggerName;
    }

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
                triggerName = Common.Common.CHARA_ANIMS_MOVE_DIR[(int)mVec];
                break;
        }
        print("Ant�̍s���J�n:" + triggerName);
        animator?.SetTrigger(triggerName);
    }
}
