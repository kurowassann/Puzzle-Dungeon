using System.Drawing;
using UnityEngine;
using Common;

public class CharacterBace : MonoBehaviour
{
	
	//定数
    /// <summary>ゲームマネージャー</summary>
    protected GameManager cGm;
	/// <summary>マップマネージャ</summary>
	protected MapManager cMm;
    /// <summary>敵かプレイヤか</summary>
    protected string cName;
    /// <summary>UI表示連携用</summary>
    protected UIManager um;




    //メンバ変数
    /// <summary>アクティブ状態</summary>
    protected bool isActive;
	/// <summary>行動が完了したか</summary>
	private bool isAction;
	/// <summary>ステート管理</summary>
    protected Status mStatus;
	/// <summary>ステートが変わったときに一度だけ処理を行う</summary>
	private bool isStatusChange;
    /// <summary>自分のマス目</summary>
    protected PosId mPosId;
	//体力
	protected int mHp;
	//移動開始位置
    private Vector3 mStartPos;
    /// <summary>目指す座標</summary>
    protected Vector3 mGoalPos;
	//移動スピード
    private Vector3 mSpeed;
	//移動方向
    private Vector3 mVec;
	//攻撃モーション待ち時間
    protected float mAttackTimer;


	//メンバ関数
    /// <summary>初期化処理</summary>
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
	/// <summary>回転処理</summary>
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
    /// <summary>移動描画処理</summary>
    virtual protected void Move()
    {
		if (isStatusChange)
		{
			if(cGm.GetDebug())
			{
				Debug.Log("移動を開始します");
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
	/// <summary>移動終了処理</summary>
    virtual protected void MoveEnd()
    {
        transform.position = mGoalPos;
        Debug.Log("移動終了");
        mGoalPos = Vector3.zero;
		SetStatus(Status.REAR_GAP);
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
	virtual public bool Damage()
	{
		mHp--;
		Debug.Log($"ダメージをうけた！{cName}の残りHPは:{mHp}");
		if(mHp <= 0)//HPが0になったら自信を消滅
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
					if(cGm.GetDebug())
					{
						Debug.Log("入力待機状態になりました、行動してください");
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
						Debug.Log($"{this.name}行動が終了しました。");
					}
					isAction = true;
					isStatusChange = false;
				}
				break;
		}
	}
	/// <summary>ターン終了時</summary>
	public void TurnEnd()
	{
		isAction = false;
		SetStatus(Status.STAY);
	}

	//Set関数
	/// <summary>ステータス書き換え</summary>
	public virtual void SetStatus(Status tstatus)
	{
		mStatus = tstatus;
		isStatusChange = true;
	}
    /// <summary>カメラの外に出ないように座標移動、マス更新</summary>
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

	//Get関数
    /// <summary>現在のマスを返す</summary>
    public Point GetPos()
    {
        return mPosId.GetPos();
    }
	/// <summary>ゲーム内に登場しているか</summary>
	public bool GetActive()
	{
		return isActive;
	}
	/// <summary>行動が完了したか</summary>
	public bool GetAction()
	{
		return isAction;
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
