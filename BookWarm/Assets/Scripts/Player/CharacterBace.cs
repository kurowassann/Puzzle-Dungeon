using System.Drawing;
using UnityEngine;
using Common;

public class CharacterBace : MonoBehaviour
{

	//�I�u�W�F�N�g�@�萔�I����
    [Tooltip("�}�X�^�[���N�G�X�g�p"), SerializeField]
    protected Master master;


	//�����o�ϐ�
	/// <summary>�A�N�e�B�u���</summary>
	private bool isActive;
	/// <summary>�X�e�[�g�Ǘ�</summary>
    protected Status mStatus;
	/// <summary>�X�e�[�g���ς�����Ƃ��Ɉ�x�����������s��</summary>
	private bool isStatusChange; 
	[Tooltip("�G���v���C����"),SerializeField]
    protected string mName;
    [Tooltip("�����̃}�X��"), SerializeField]
    protected Point mPoint;
	//�̗�
	protected int mHp;
	//�ړ��J�n�ʒu
    private Vector3 mStartPos;
    [Tooltip("�ڎw�����W"), SerializeField]
    protected Vector3 mGoalPos;
	//�ړ��X�s�[�h
    private Vector3 mSpeed;
	//�ړ�����
    private Vector3 mVec;
	//�U�����[�V�����҂�����
    protected float mAttackTimer;


	//�����o�֐�
    /// <summary>����������</summary>
    virtual public void Init(Master tmas, Point tpoint, int thp, string tstr)
    {   
        Debug.Log("���N���X������");
        mName = tstr;
        master = tmas;
		SetStatus(Status.STAY);
        SetPos(tpoint);
        transform.position = mGoalPos;
		mHp = thp;
		isActive = true;

		mAttackTimer = 0;
    }
    /// <summary>�ړ��`�揈��</summary>
    virtual protected void Move()
    {
		if (isStatusChange)
		{
			Debug.Log("�ړ����J�n���܂�");
			//SetPos();
			isStatusChange = false;
		}


		if (mGoalPos == Vector3.zero) { return; }

        this.transform.position += mSpeed * Time.deltaTime/Common.Common.MOTION_SPEED;

        Vector3 vec = mGoalPos - transform.position;
        if(mVec != vec.normalized)
        {
            MoveEnd();
        }
    }
	/// <summary>�ړ��I������</summary>
    virtual protected void MoveEnd()
    {
        transform.position = mGoalPos;
        //Debug.Log("�ړ��I��");
        mGoalPos = Vector3.zero;
        mStatus = Status.REAR_GAP;
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
	public virtual bool Damage()
	{
		mHp--;
		Debug.Log($"�_���[�W���������I{mName}�̎c��HP��:{mHp}");
		if(mHp <= 0)//HP��0�ɂȂ����玩�M������
		{
			master.SetPos(this, new Point(0,0), "w");
			mPoint = new Point(0, 0);

			this.gameObject.SetActive(false);
			isActive = false;
			SetStatus(Status.REAR_GAP);
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
					Debug.Log("���͑ҋ@��ԂɂȂ�܂����A�s�����Ă�������");
					isStatusChange = false;
				}
				break;
			case Status.MOVE:
				Move();
				break;
			case Status.ATTACK:
				Attack();
				break;
			case Status.REAR_GAP:
				if (isStatusChange)
				{
					Debug.Log($"{this.name}�s�����I�����܂����B");
					//master.PlayerAction();
					isStatusChange = false;
				}
				break;
		}
	}

	//Set�֐�
	/// <summary>�X�e�[�^�X��������</summary>
	public virtual void SetStatus(Status tstatus)
	{
		mStatus = tstatus;
		isStatusChange = true;
	}
    /// <summary>�J�����̊O�ɏo�Ȃ��悤�ɍ��W�ړ��A�}�X�X�V</summary>
    protected void SetPos(Point tpoint)
    {
        mGoalPos = master.SetPos(this, tpoint, mName); 
        mPoint = tpoint;
        mStartPos = transform.position;
        //Debug.Log(mGoalPos);
        mSpeed = mGoalPos - mStartPos;
        mVec = mSpeed.normalized;
        //Debug.Log(mPoint);
    }

	//Get�֐�
    /// <summary>���݂̃}�X��Ԃ�</summary>
    public Point GetPos()
    {
        return mPoint;
    }
	/// <summary>�Q�[�����ɓo�ꂵ�Ă��邩</summary>
	public bool GetActive()
	{
		return isActive;
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
