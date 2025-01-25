using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;


public class Player : CharacterBace
{
    
    //�I�u�W�F�N�g
    [Tooltip("�X�e�B�b�N"), SerializeField]
    private InputAction inputMover;
    [Tooltip("�X�e�B�b�N"), SerializeField]
    private InputActionAsset inputActions;
    /// <summary>UI�\���A�g�p</summary>
	//private UIManager um;
    [Tooltip("�A�j���[�^�[�R���|�[�l���g")]
    Animator animator;


    //�����o�ϐ�
    /// <summary>�U���ʒu</summary>
    private Point AttackPoint;


    //�����o�֐� 
    /// <summary>����������</summary>
    public override void Init(GameManager tgm, MapManager tmm, PosId tposId, int thp, string tstr)
    {
        base.Init(tgm, tmm, tposId, thp, tstr);

        //um = GameObject.Find("UIManager").GetComponent<UIManager>();
        //um.GeneratePlayerHP(mHp);

        //Debug.Log("�v���C��������");
        //SetCam();

        // �A�j���[�^�[�R���|�[�l���g�̎擾
        animator = GetComponent<Animator>();
    }
    // �I�u�W�F�N�g���A�N�e�B�u�ɂȂ����Ƃ��ɌĂ΂��C�x���g
    private void OnDisable()
    {
        inputMover.Disable();
        Debug.Log("a");
    }
    // �I�u�W�F�N�g����\���ɂȂ����Ƃ��ɌĂ΂��C�x���g
    private void OnEnable()
    {
        inputMover = inputActions.FindAction("Player/Move");

        inputMover.Enable();

    }    
    /// <summary>�_���[�W����</summary>
    /// <returns></returns>
    public override bool Damage()
	{
		bool ret = base.Damage();
        /*
		um.DisplayPlayerHP(mHp);
		um.AddLog("�v���C���Ƀ_���[�W�I�I");
		if(ret == true) 
		{
			um.AddLog("����܂���");
		}
        */
		return ret;
	}   
    /// <summary>�̗͉�</summary>
    public void Heal()
    {
        mHp++;
        if(mHp > 2)
        {
            mHp = 2;
        }
        /*
        um.DisplayPlayerHP(mHp);
        um.AddLog("�v���C���[��HP����");
        */
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
        switch (animationName)
        {
            case "MoveRight@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];
                break;
            case "MoveLeft@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];
                break;
            case "MoveUp@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];
                break;
            case "MoveDown@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];
                break;
        }

        animator.SetTrigger(triggerName);
    }
    /// <summary>�U���I������</summary>
    protected override void AttackEnd()
    {
        base.AttackEnd();
        //master.PlayerAttack(AttackPoint);
        var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string triggerName = "";
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_MOVE_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_END_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_ATTACK_DIR);
        //print("aaa" + animationName);
        switch (animationName)
        {
            case "AttackRight@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];
                break;
            case "AttackLeft@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];
                break;
            case "AttackUp@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];
                break;
            case "AttackDown@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];
                break;
        }

        animator.SetTrigger(triggerName);

        Heal();
    }
    /// <summary>�L�[���͂ɑ΂��锽��</summary>
    private void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetStatus(Status.REAR_GAP);
        }

        //�L�[���͕���
        if (inputMover.ReadValue<Vector2>() != new Vector2(0, 0))
        {
            //�X�e�[�g���s����t��Ԃ��ǂ���
            if (mStatus == Status.STAY)
            {
                TileInfo TI;
                Point point = mPosId.GetPos();

                // �Ăяo���A�j���[�V�����g���K�[�̖��O
                Vector vec;
                // �A�j���[�V�����̃��Z�b�g
                ResetTrigger(animator, Common.Common.CHARA_ANIMS_MOVE_DIR);
                ResetTrigger(animator, Common.Common.CHARA_ANIMS_END_DIR);
                ResetTrigger(animator, Common.Common.CHARA_ANIMS_ATTACK_DIR);



                if (inputMover.ReadValue<Vector2>().x == 1.0f)//��
                {
                    point.X += 1;
                    vec = Vector.RIGHT;

                }
                else if (inputMover.ReadValue<Vector2>().x == -1.0f)//��
                {
                    point.X -= 1;
                    vec = Vector.LEFT;
                }
                else if (inputMover.ReadValue<Vector2>().y == 1.0f)//��
                {
                    point.Y -= 1;
                    vec = Vector.UP;

                }
                else if (inputMover.ReadValue<Vector2>().y == -1.0f)//��
                {
                    point.Y += 1;
                    vec = Vector.DOWN;

                }
                else { return; }
                TI = cMm.CheckTile(point);
                //Debug.Log(point);
                if (TI == TileInfo.ROUTE)
                {
                    Debug.Log($"{point}�Ɉړ����J�n���܂��B");
                    SetPos(point);
                    SetStatus(Status.MOVE);
                    // �A�j���[�V�����̌Ăяo��
                    var triggerName = Common.Common.CHARA_ANIMS_MOVE_DIR[(int)vec];
                    animator.SetTrigger(triggerName);
                }
                else if (TI == TileInfo.ENEMY)
                {
                    //Debug.Log("�U��");

                    SetStatus(Status.ATTACK);
                    AttackPoint = point;
                    //master.PlayerAttack(point);

                    // �A�j���[�V�����N����
                    //
                    //("�U�����܂�:" + point);
                    var triggerName = Common.Common.CHARA_ANIMS_ATTACK_DIR[(int)vec];
                    animator.SetTrigger(triggerName);
                }
            }

        }

    }

    //Set�֐�
    /// <summary>�J������ǔ�</summary>
    private void SetCam()
    {
        /*
        float z = Camera.main.transform.position.z;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, z);
 */
        }

    // Update is called once per frame
    override protected void Update()
    {
        KeyInput();

        base.Update();


    }
    /// <summary>Update�̂��ƁA�Ō�ɌĂ΂��</summary>
    private void LateUpdate()
    {
        SetCam();
    }
}
