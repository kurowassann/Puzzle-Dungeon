using System.Drawing;
using UnityEngine;
using Common;

public class CharacterBace : MonoBehaviour
{
	
	//�萔
    /// <summary>�Q�[���}�l�[�W���[</summary>
    protected GameManager cGm;
	/// <summary>�}�b�v�}�l�[�W��</summary>
	protected MapManager cMm;
    /// <summary>�G���v���C����</summary>
    protected string cName;
    /// <summary>UI�\���A�g�p</summary>
    protected UIManager um;




    //�����o�ϐ�
    /// <summary>�A�N�e�B�u���</summary>
    protected bool isActive;
	/// <summary>�s��������������</summary>
	private bool isAction;
	/// <summary>�X�e�[�g�Ǘ�</summary>
    protected Status mStatus;
	/// <summary>�X�e�[�g���ς�����Ƃ��Ɉ�x�����������s��</summary>
	private bool isStatusChange;
    /// <summary>�����̃}�X��</summary>
    protected PosId mPosId;
	//�̗�
	protected int mHp;
	//�ړ��J�n�ʒu
    private Vector3 mStartPos;
    /// <summary>�ڎw�����W</summary>
    protected Vector3 mGoalPos;
	//�ړ��X�s�[�h
    private Vector3 mSpeed;
	//�ړ�����
    private Vector3 mVec;
	//�U�����[�V�����҂�����
    protected float mAttackTimer;


	//�����o�֐�
    /// <summary>����������</summary>
    virtual public void Init(GameManager tgm, MapManager tmm, PosId tposId, int thp, string tstr)
    {
        um = GameObject.Find("UIManager").GetComponent<UIManager>();
        um.GeneratePlayerHP(mHp);


        cName = tstr;
		cGm = tgm;
		cMm = tmm;
		SetStatus(Status.STAY);
        SetPos(tposId.GetPos());
        transform.position = mGoalPos;
		mHp = thp;
		isActive = true;
		isAction = false;

		mAttackTimer = 0;
    }
	/// <summary>��]����</summary>
	virtual protected void Trun()
	{
        if (isStatusChange)
        {
            isStatusChange = false;
        }
		else
		{
			mGoalPos = transform.position;
			MoveEnd();

		}

    }
    /// <summary>�ړ��`�揈��</summary>
    virtual protected void Move()
    {
		if (isStatusChange)
		{
			if(cGm.GetDebug())
			{
				Debug.Log("�ړ����J�n���܂�");
			}
			isStatusChange = false;
		}


		if (mGoalPos == Vector3.zero) 
		{

			return;
		}

        this.transform.position += mSpeed * Time.deltaTime/Common.Common.MOTION_SPEED;

        Vector3 vec = mGoalPos - transform.position;
        if(mVec != vec.normalized || mStatus == Status.TRUN)
        {
            MoveEnd();
        }
    }
	/// <summary>�ړ��I������</summary>
    virtual protected void MoveEnd()
    {
        transform.position = mGoalPos;
        Debug.Log("�ړ��I��");
        mGoalPos = Vector3.zero;
		SetStatus(Status.REAR_GAP);
    }
	/// <summary>�U�����s��</summary>
	protected void Attack()
	{
		if (isStatusChange)
		{
			Debug.Log("�U�����J�n���܂��B");
			//master.PlayerAttack();
			isStatusChange = false;
		}



		mAttackTimer += Time.deltaTime;

		if(mAttackTimer > 0.5f) 
		{
			AttackEnd();
			mAttackTimer = 0;
		}
	}
	/// <summary>�U���I������</summary>
	virtual protected void AttackEnd()
	{
		SetStatus(Status.REAR_GAP);
		mAttackTimer = 0;
	}
	/// <summary>�U�����󂯂�</summary>
	virtual public bool Damage()
	{
		mHp--;
		Debug.Log($"�_���[�W���������I{cName}�̎c��HP��:{mHp}");
		if(mHp <= 0)//HP��0�ɂȂ����玩�M������
		{
			cMm.SetPos(this, new Point(0,0), "w");
			mPosId.SetPos(new Point(0, 0));

			isActive = false;
			SetStatus(Status.REAR_GAP);	
			this.gameObject.SetActive(false);

			return true;
		}
		return false;
	}
	/// <summary>�A�j���[�V�����̃��Z�b�g</summary>
    protected void ResetTrigger(Animator _animator, string[] _names)
    {
        foreach (var name in _names)
        {
            _animator.ResetTrigger(name);
        }
    }
	/// <summary>�X�e�[�^�X��������</summary>
    private void StatusBranch()
	{
		switch (mStatus)
		{
			case Status.STAY:
				if (isStatusChange)
				{
					if(cGm.GetDebug())
					{
						Debug.Log("���͑ҋ@��ԂɂȂ�܂����A�s�����Ă�������");
					}
					isStatusChange = false;
				}
				break;
			case Status.MOVE:
				Move();
				break;
			case Status.TRUN:
				Trun();
				break;
			case Status.ATTACK:
				Attack();
				break;
			case Status.REAR_GAP:
				if (isStatusChange)
				{
					if (cGm.GetDebug()) 
					{
						Debug.Log($"{this.name}�s�����I�����܂����B");
					}
					isAction = true;
					isStatusChange = false;
				}
				break;
		}
	}
	/// <summary>�^�[���I����</summary>
	public void TurnEnd()
	{
		isAction = false;
		SetStatus(Status.STAY);
	}

	//Set�֐�
	/// <summary>�X�e�[�^�X��������</summary>
	public virtual void SetStatus(Status tstatus)
	{
		mStatus = tstatus;
		isStatusChange = true;
	}
    /// <summary>�J�����̊O�ɏo�Ȃ��悤�ɍ��W�ړ��A�}�X�X�V</summary>
    protected void SetPos(Point tpos)
    {
        mGoalPos = cMm.SetPos(this, tpos, cName);
		mPosId.SetPos(tpos);
		if(cName == "p")
		{
			mPosId.Set(cMm.ChangeId(mPosId));
		}
		cGm.ItemCheck(mPosId.GetPos());
        mStartPos = transform.position;
        mSpeed = mGoalPos - mStartPos;
        mVec = mSpeed.normalized;
    }

	//Get�֐�
    /// <summary>���݂̃}�X��Ԃ�</summary>
    public Point GetPos()
    {
        return mPosId.GetPos();
    }
	/// <summary>�Q�[�����ɓo�ꂵ�Ă��邩</summary>
	public bool GetActive()
	{
		return isActive;
	}
	/// <summary>�s��������������</summary>
	public bool GetAction()
	{
		return isAction;
	}
	/// <summary>���݂̏��</summary>
	public Status GetStatus()
	{
		return mStatus;
	}


    virtual protected void  Update() 
    {
		StatusBranch();
    }
    
	
}
