using System.Drawing;
using UnityEngine;
using Common;

public class CharacterBace : MonoBehaviour
{

	//オブジェクト　定数的扱い
    [Tooltip("マスターリクエスト用"), SerializeField]
    protected Master master;


	//メンバ変数
	/// <summary>アクティブ状態</summary>
	private bool isActive;
	/// <summary>ステート管理</summary>
    protected Status mStatus;
	/// <summary>ステートが変わったときに一度だけ処理を行う</summary>
	private bool isStatusChange; 
	[Tooltip("敵かプレイヤか"),SerializeField]
    protected string mName;
    [Tooltip("自分のマス目"), SerializeField]
    protected Point mPoint;
	//体力
	protected int mHp;
	//移動開始位置
    private Vector3 mStartPos;
    [Tooltip("目指す座標"), SerializeField]
    protected Vector3 mGoalPos;
	//移動スピード
    private Vector3 mSpeed;
	//移動方向
    private Vector3 mVec;
	//攻撃モーション待ち時間
    protected float mAttackTimer;


	//メンバ関数
    /// <summary>初期化処理</summary>
    virtual public void Init(Master tmas, Point tpoint, int thp, string tstr)
    {   
        Debug.Log("基底クラス初期化");
        mName = tstr;
        master = tmas;
		SetStatus(Status.STAY);
        SetPos(tpoint);
        transform.position = mGoalPos;
		mHp = thp;
		isActive = true;

		mAttackTimer = 0;
    }
    /// <summary>移動描画処理</summary>
    virtual protected void Move()
    {
		if (isStatusChange)
		{
			Debug.Log("移動を開始します");
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
	/// <summary>移動終了処理</summary>
    virtual protected void MoveEnd()
    {
        transform.position = mGoalPos;
        //Debug.Log("移動終了");
        mGoalPos = Vector3.zero;
        mStatus = Status.REAR_GAP;
    }
	/// <summary>攻撃を行う</summary>
	protected void Attack()
	{
		if (isStatusChange)
		{
			Debug.Log("攻撃を開始します。");
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
	/// <summary>攻撃終了処理</summary>
	virtual protected void AttackEnd()
	{
		SetStatus(Status.REAR_GAP);
		mAttackTimer = 0;
	}
	/// <summary>攻撃を受ける</summary>
	public virtual bool Damage()
	{
		mHp--;
		Debug.Log($"ダメージをうけた！{mName}の残りHPは:{mHp}");
		if(mHp <= 0)//HPが0になったら自信を消滅
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
	/// <summary>アニメーションのリセット</summary>
    protected void ResetTrigger(Animator _animator, string[] _names)
    {
        foreach (var name in _names)
        {
            _animator.ResetTrigger(name);
        }
    }
	/// <summary>ステータス処理分岐</summary>
    private void StatusBranch()
	{
		switch (mStatus)
		{
			case Status.STAY:
				if (isStatusChange)
				{
					Debug.Log("入力待機状態になりました、行動してください");
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
					Debug.Log($"{this.name}行動が終了しました。");
					//master.PlayerAction();
					isStatusChange = false;
				}
				break;
		}
	}

	//Set関数
	/// <summary>ステータス書き換え</summary>
	public virtual void SetStatus(Status tstatus)
	{
		mStatus = tstatus;
		isStatusChange = true;
	}
    /// <summary>カメラの外に出ないように座標移動、マス更新</summary>
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

	//Get関数
    /// <summary>現在のマスを返す</summary>
    public Point GetPos()
    {
        return mPoint;
    }
	/// <summary>ゲーム内に登場しているか</summary>
	public bool GetActive()
	{
		return isActive;
	}
	/// <summary>現在の状態</summary>
	public Status GetStatus()
	{
		return mStatus;
	}

    virtual protected void  Update() 
    {
		StatusBranch();
    }
    

}
