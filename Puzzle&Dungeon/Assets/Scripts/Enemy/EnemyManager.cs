using System;
using System.Drawing;
using UnityEngine;
using Common;
using Data;
using static UnityEngine.EventSystems.EventTrigger;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    
    //定数
    /// <summary>ゲーム管理元</summary>
    private GameManager cGm;
    /// <summary>マップ管理元</summary>
    private MapManager cMm;
    /// <summary>uiマネージャ</summary>
    private UIManager cUm;
    /// <summary>敵管理用配列</summary>
    private List<Enemy> cEnemys;
    /// <summary>エネミー生成用オブジェクト</summary>
    private GameObject cEnemy;
    /// <summary>経路探索評価用タイル</summary>
    private Tile[,] cTiles;
    /// <summary>タイルのチェック状態</summary>
    private enum TileStatus
    {
        NONE,
        OPEN,
        CLOSE,
        RECRUIT,
        START,
        END
    }

	/// <summary>ゲームデータ</summary>
	private JsonGameData mGameData;

    //メンバ変数
    /// <summary>ステータス</summary>
	private Status mStatus;
    /// <summary>ステータス変更時一回呼ぶもの</summary>
	private bool isStatus;
    /// <summary>行動を行う順番</summary>
	private int[] mActionOrder;
    /// <summary>行動の終了をみるフラグ</summary>
    private bool isAction;


    //メンバ関数  
    //private
    /// <summary>敵の抽選</summary>
	private int LotteryEnemy(JsonStageEnemyData[] tdata)
	{
		int sumRate =0;

		for(int i=0;i <tdata.Length;i++)
		{
			sumRate += (int)tdata[i].spawn_rate;
		}
		print($"割合合計：{sumRate}");
		int ram = UnityEngine.Random.Range(1,sumRate+1);
		print($"抽選結果：{ram}");

		sumRate = 0;
		for(int i = 0; i < tdata.Length; i++)
		{
			sumRate += (int)tdata[i].spawn_rate;
			if(ram <= sumRate )
			{
				return tdata[i].enemy_id;
			}
		}

		return 0;
	}
	/// <summary>idが一致する敵を持ってくる</summary>
	private JsonEnemyData LookUseEnemys(JsonGameData tdata, int tnum)
	{
		//print(tdata.enemy_datas[0].enemy_name);

		//ステージに現れる敵を見る
		for(int i=0;i< tdata.enemy_datas.Length;i++)
		{
				if (tdata.enemy_datas[i] == null)
				{
					print("そんな敵はいないよ");
					continue;
				}

				if(tnum == tdata.enemy_datas[i].enemy_id)
				{
					print($"{tdata.enemy_datas[i].enemy_name}を出現させます。");
					return tdata.enemy_datas[i];
				}
		}
		return null;
	}   
    /// <summary>検索タイルリセット</summary>
	private void ResetTiles()
    {
        for(int i = 0;i< cTiles.GetLength(0);i++)
        {
            for(int j = 0;j<cTiles.GetLength(1);j++)
            {
                cTiles[i,j].Init();
            }
        }
    }
    /// <summary>目標までの経路探索</summary>
    private void SearchTarget(Point tpPos)
    {

        //検索対象の位置を取得(ループ)
        for(int i = 0;i < cEnemys.Count;i++)
        {
            SeachOne(i, tpPos);
        }
    }
	/// <summary>経路探索</summary>
    private void SeachOne(int i,Point tpPos)
    {
        cEnemys[i].ResetRoute();
        Point my = cEnemys[i].GetPos();
        Debug.Log(my);
        cTiles[my.X, my.Y].SetTile(TileStatus.START);
        cTiles[tpPos.X, tpPos.Y].SetTile(TileStatus.END);

        bool flg;
        flg = CheckAround(my, tpPos, cTiles[my.X, my.Y].GetBfore() + 1);

        //終了条件
        //周囲に点数を付けるときにゴールがある
        while (!flg)
        {
            //評価がいいマスの検索(ループ)
            Point nextTile = SearchTile();
            cTiles[nextTile.X, nextTile.Y].SetTile(TileStatus.CLOSE);

            //周囲のマスに点数を付ける
            flg = CheckAround(nextTile, tpPos, cTiles[nextTile.X, nextTile.Y].GetBfore() + 1);
        }

        //ゴールからスタートまで道筋をさかのぼって記憶していく
        FollowRoute(tpPos, cEnemys[i], 0);

        //mEnemy[i].ShowRoute();

        //タイルをリセットして次へ
        ResetTiles();

    }

    //public
    /// <summary>初期化処理</summary>
    public void Init(GameManager tgm,MapManager tmm)
    {
        //初期値設定
        {
            cUm = GameObject.Find("UIManager").GetComponent<UIManager>();


            //mGameData = tgameData;
            cGm = tgm;
            cMm = tmm;
            //enemy = (GameObject)Resources.Load("Prefabs/Enemy");
            cEnemy = (GameObject)Resources.Load("Prefabs/Ant");
            cEnemys = new List<Enemy>();


			Point point = cMm.GetTileLength();//マス目の大きさが知りたい
            cTiles = new Tile[point.X, point.Y];
            for (int i = 0; i < cTiles.GetLength(0); i++)
            {
                for (int j = 0; j < cTiles.GetLength(1); j++)
                {
                    cTiles[i, j] = new Tile(cMm);
                }
            }
            ResetTiles();

			SetStatus(Status.STAY);
			
            isAction = false;
        }
        /*
        //指定数敵を生成する
        for (int i = 0; i < cEnemys.LongLength; i++)
        {
            //生成する敵を決める
            print(mGameData);
            print(mGameData.stage_data);
            print(mGameData.stage_data.enemy_datas);
			int id = LotteryEnemy(mGameData.stage_data.enemy_datas);
			print($"生成するのはid{id}");
			var enemyData = LookUseEnemys(mGameData, id);
			print($"{enemyData.enemy_name}");
			//プレファブを適用させる
			enemy = (GameObject)Resources.Load("Prefabs/" + enemyData.enemy_id_name);

			var clone = Instantiate(enemy);
            clone.transform.SetParent(this.gameObject.transform, false);
            clone.transform.localPosition = new Vector3(0,0, 0);
            mEnemys[i] = clone.GetComponent<Enemy>();
            //master.GeneratePlayer("e", mEnemys[i]);
            mEnemys[i].Init(this);
        }
        //経路探索
        SearchTarget(master.GetPlayer());
        */
    }
    /// <summary>生成</summary>
	public void Generate(string tstr, Enemy bace)
	{
		//cGm.GeneratePlayer(tstr, bace);
        bace.Init(this,cGm,cMm,new PosId(),1,"e");

    }
    /// <summary>部屋点灯じの敵生成</summary>
    public void Spawn(int num)
    {
        cEnemy = (GameObject)Resources.Load("Prefabs/Ant" );


        var clone = Instantiate(cEnemy);
        clone.transform.SetParent(this.gameObject.transform, false);
        clone.transform.localPosition = new Vector3(0, 0, 0);
        //master.GeneratePlayer("e", mEnemys[i]);
        clone.GetComponent<Enemy>().Init(this, cGm, cMm, cMm.GetPosId(num), 1, "e");
        cEnemys.Add(clone.GetComponent<Enemy>());

    }
    //敵が上限を超えていないか

    /// <summary>次に検索を始めるタイルを決める</summary>
    private Point SearchTile()
    {
        Point point = Point.Empty;

        for(int i= 0;i<cTiles.GetLength(0);i++)
        {
            for(int j = 0;j<cTiles.GetLength(1);j++)
            {
                //評価ついていないタイルは無視
                if (cTiles[i,j].GetTile() != TileStatus.OPEN)
                {
                    //Debug.Log($"タイル{i},{j}の状態:{mTiles[i, j].GetTile()}");
                    continue;
                }

                if (cTiles[i,j].GetScore() < cTiles[point.X,point.Y].GetScore() || point == Point.Empty)
                {
                    //Debug.Log($"タイル更新:{i},{j}");
                    point.X = i; point.Y = j;
                }
            }
        }

        return point;
    }
    /// <summary>四方向チェック</summary>
    private bool CheckAround(Point tpos, Point tendPos, float tbfore)
    {
        Point point;
        bool flg;
        //Debug.Log((int)Enum.Parse(typeof(Vector), "NONE"));

        //検索順をシャッフル
        Vector[] vec =
        {
            Vector.RIGHT,
            Vector.LEFT,
            Vector.UP,
            Vector.DOWN
        };
        
        System.Random rng = new System.Random();
        int n = vec.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Vector tmp = vec[k];
            vec[k] = vec[n];
            vec[n] = tmp;
        }
        

        for (int i = 0;i < vec.Length;i++)
        {
            //print($"検索方向{vec[i]}");
            //見ようとしているマスが移動可能マスか、まだ計算してないマスか
            point = tpos;
            if (vec[i] == Vector.RIGHT) { point.X++; }//RIGHT
            if (vec[i] == Vector.LEFT) { point.X--; }//LEFT
            if (vec[i] == Vector.UP) { point.Y--; }//UP
            if (vec[i] == Vector.DOWN) { point.Y++; }//DOWN
            if (cTiles[point.X, point.Y].GetTile() == TileStatus.END) 
            {
                Debug.Log("プレイヤ発見!!");
                cTiles[point.X, point.Y].SetDir(vec[i]);
                return true;
            }
            flg = (cMm.CheckTile(point) != TileInfo.WALL && cTiles[point.X, point.Y].GetTile() == TileStatus.NONE);
            if (flg)
            {
                cTiles[point.X, point.Y].OpenTile(point, tendPos, vec[i], tbfore);
            }
        }

        return false;
    }
    /// <summary>最短経路を記録する</summary>
    private void FollowRoute(Point tpos, Enemy tenemy, int num)
    {
        num++;
        if(num == 800)
        {
            Debug.LogError("経路探索に失敗");
            return;
        }

        if (cTiles[tpos.X,tpos.Y].GetTile() == TileStatus.START)
        {
            return;
        }

        Vector vec = cTiles[tpos.X, tpos.Y].GetDir();
        //Debug.Log(vec);

        Point point = tpos;
        if (vec == Vector.RIGHT) { point.X--; }//RIGHT
        if (vec == Vector.LEFT) { point.X++; }//LEFT
        if (vec == Vector.UP) { point.Y++; }//UP
        if (vec == Vector.DOWN) { point.Y--; }//DOWN

        FollowRoute(point,tenemy, num);

        tenemy.AddRoute(vec);
    }    
    /// <summary>プレイヤーに近い順に行動を決める</summary>
	private int[] SortEnemys()
	{
		//プレイヤに近い順に行動を行う
		int[] length = new int[cEnemys.Count];
		int[] actionOrder = new int[cEnemys.Count];
		bool[] decision = new bool[cEnemys.Count];
		for (int i = 0; i < cEnemys.Count; i++)
		{
            if (!cEnemys[i].GetActive()) { continue; }
			length[i] = Mathf.Abs(cEnemys[i].GetRoute() - cEnemys[i].GetNum());
			decision[i] = false;
		}
		Array.Sort(length);
		for (int i = 0; i < cEnemys.Count; i++)
		{
			for (int j = 0; j < cEnemys.Count; j++)
			{
                if (cEnemys[i] == null) { continue; }
                if (length[i] == Mathf.Abs(cEnemys[j].GetRoute() - cEnemys[j].GetNum()) && !decision[j])
				{
					//Debug.Log(decision[j]);
					decision[j] = true;
					actionOrder[i] = j;
					break;
				}
			}
			//Debug.Log(actionOrder[i]);
		}

		return actionOrder;
	}
    /// <summary>行動決定</summary>
    public void EnemyActionSelect()
    {

        //print("aaa");
        isAction = false;

		//行動順整理
		mActionOrder =  SortEnemys();

		//行動
        bool first = true, second = true;
        for (int i = 0; i < cEnemys.Count; i++)
        {
			Enemy tenemy = cEnemys[mActionOrder[i]];

            if (!tenemy.GetActive()) { continue; }




			//存在しない場合スキップ
			if (tenemy.GetActive() == false )
			{
				tenemy.SetStatus(Status.REAR_GAP);
				continue; 
			}

			//プレイヤが近い場合プレイヤの行動に対応した動きを行う
			float z = Mathf.Abs(tenemy.GetPos().X - cGm.GetPlayer().X) + Mathf.Abs(tenemy.GetPos().Y - cGm.GetPlayer().Y);
			if (z < 5)
			{
				SeachOne(mActionOrder[i], cGm.GetPlayer());
			}


			first = tenemy.Tracking();
            //z = Mathf.Abs(tenemy.GetPos().X - master.GetPlayer().X) + Mathf.Abs(tenemy.GetPos().Y - master.GetPlayer().Y);
            if (!first)
            {
                print("1度目の行動が無かったのでルートの再検索を行います");
                SeachOne(mActionOrder[i], cGm.GetPlayer());
            }
            second = tenemy.Tracking();
			if(first == second) //移動がなかった時に移動済みに
			{
				Debug.Log("移動先がありませんでした");
				tenemy.SetStatus(Status.REAR_GAP);
			}
        }
        //SearchTarget(master.GetPlayer());

        SetStatus(Status.ATTACK);
    }
	/// <summary>ステートを実行状態にする</summary>
	private void ChangeAction(Status tstatus)
	{
		for(int i = 0;i < cEnemys.Count;i++)
		{
			Enemy enemy = cEnemys[mActionOrder[i]];
			if(enemy.GetNextAction() == tstatus)
			{
				enemy.SetStatus(tstatus);
			}
		}
	}
	/// <summary>攻撃の敵が攻撃を終えているか</summary>
	private void Attack()
	{ 
		for(int i = 0; i < cEnemys.Count; i++)
		{
			Enemy enemy = cEnemys[mActionOrder[i]];
			if(enemy.GetNextAction() == Status.ATTACK)
			{
				if(enemy.GetStatus() != Status.REAR_GAP)
				{
					return;
				}
			}
		}
		SetStatus(Status.MOVE);

	}
    /*
	/// <summary>プレイヤの攻撃する</summary>
	public void EnemyAttack()
	{
		master.EnemyAttack();
	}
    */
	/// <summary>引数の座標の敵にダメージを与える</summary>
	public void Damege(Point tpos)
	{
		for (int i = 0; i < cEnemys.Count;i++)
		{
			if (cEnemys[i].GetPos() == tpos && cEnemys[i].GetActive())
			{
				cEnemys[i].Damage();
               

			}
		}
	}
	/// <summary>移動の敵が移動を終えているか</summary>
	private void Move()
	{
		for (int i = 0; i < cEnemys.Count; i++)
		{
			Enemy enemy = cEnemys[mActionOrder[i]];
			if (enemy.GetNextAction() == Status.MOVE)
			{
				if (enemy.GetStatus() != Status.REAR_GAP)
				{
					return;
				}
			}
		}
		SetStatus(Status.REAR_GAP);
	}
    /// <summary>すべてが完了しているか</summary>
	private void RearGap()
	{
		for( int i = 0;i < cEnemys.Count;i++)
		{
			Enemy enemy= cEnemys[mActionOrder[i]];
            if(enemy.GetActive() == false)
            {
                enemy.RespawnCount();
            }

			enemy.SetStatus(Status.STAY);
			enemy.SetAction(Status.STAY);
		}
        isAction = true;
		SetStatus(Status.STAY);
	}

    //Set関数
    /// <summary>ステータスの変更</summary>
	private void SetStatus(Status tstatus)
	{
		mStatus = tstatus;
		isStatus = true;
	}

    //Get関数
    public bool GetAction()
    {
        return isAction;
    }

	// Update is called once per frame
	void Update()
    {
		switch(mStatus)
		{
			case Status.STAY:
				if(isStatus)
				{
					//Debug.Log("プレイヤの行動待ちです");
					isStatus = false;
				}
				break;
			case Status.MOVE:
				if(isStatus)
				{
					//Debug.Log("移動の敵の処理を行います");
					ChangeAction(Status.MOVE);
                    ChangeAction(Status.TRUN);
					isStatus = false;
				}
				Move();
				break;
			case Status.ATTACK:
				if(isStatus)
				{
					//Debug.Log("攻撃の敵の処理を行います");
					ChangeAction(Status.ATTACK);
					isStatus = false;
				}
				Attack();
				break;
			case Status.REAR_GAP:
				if(isStatus)
				{
					//Debug.Log("行動が完了しました");
					//master.EnemyActionFinish();
					RearGap();
					isStatus = false;
				}
				break;
		}


    }


    //探索情報格納用クラス
    private class Tile
    {
        /// <summary>マスター情報</summary>
        private MapManager cMm;

        //どこから来たか
        private Vector mDir;
        //スコア
        private float mScore;
        //ここに来るまでの歩数
        private float mBfore;
        //自身の状態
        private TileStatus mStatus;

        /// <summary>現在の状態を返す</summary>
        public TileStatus GetTile()
        {
            return mStatus;
        }
        /// <summary>mBforeを返す</summary>
        public float GetBfore()
        {
            return mBfore;
        }
        /// <summary>mScoreを返す</summary>
        public float GetScore()
        {
            return mScore;
        }
        /// <summary>mDirを返す</summary>
        public Vector GetDir()
        {
            return mDir;
        }

        /// <summary>引数のステータスに変更</summary>
        /// <param name="tstatus"></param>
        public void SetTile(TileStatus tstatus)
        {
            mStatus = tstatus;
        }
        /// <summary>来た方向保存</summary>
        public void SetDir(Vector tvec)
        {
            mDir = tvec;
        }

        //初期化処理
        public Tile(MapManager tmm)
        {
            Init();
            cMm = tmm;
        }
        public void Init()
        {
            mDir =  Vector.NONE;
            mScore = 0.0f;
            mBfore = 0.0f;
            mStatus = TileStatus.NONE;
        }

        /// <summary>タイルの情報計算</summary>
        public TileStatus OpenTile(Point tpos, Point tendPos,Vector tdir, float tbfore)
        {
            SetTile(TileStatus.OPEN);
            mDir = tdir;
            mBfore=tbfore;
            mScore =CheckLength(tpos,tendPos) + mBfore;
            //Debug.Log($"タイル:{tpos}スコア:{mScore}");
            return mStatus;
        }
        
        /// <summary>スタートからゴールまで最短の移動回数</summary>
        private float CheckLength(Point tstart, Point tgoal)
        {
            float length = Mathf.Abs(tgoal.Y - tstart.Y) + Mathf.Abs(tgoal.X - tstart.X);
            if (cMm.CheckTile(tstart) == TileInfo.ENEMY)
            {
                print("敵がいるためペナルティ");
                length += 100.1f;
            }
            /*
            float width = Mathf.Abs(tgoal.X - tstart.X);
            float height = Mathf.Abs(tgoal.Y - tstart.Y);

            //横方向の検索
            Point point = tstart;
            for(int i = 0;i < width;i++)
            {
                point.X += i;
                var tmp = master.CheckTile(point);
                print($"チェックしたタイル{tmp}");
            }
            */

            //float length;
            //Debug.Log(length);
            return length;
            
        }

    }

	

}
