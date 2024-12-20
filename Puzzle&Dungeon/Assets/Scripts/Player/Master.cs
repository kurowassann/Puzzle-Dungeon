using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using Common;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Timeline;
using Data;
using System;

public class Master : MonoBehaviour
{
    [Tooltip("マップ生成リクエスト用"), SerializeField]
    private MapGeneretor MG;

    [Tooltip("プレイヤ"), SerializeField]
    private Player player;

    [Tooltip("エネミー管理元"), SerializeField]
    private EnemyManager EM;


    [Tooltip("タイルの情報"), SerializeField]
    private string[,] tiles;


	//後隙カウント
	private bool enemyActionFlg;
	private float mTimer;

	/// <summary>ゴール地点</summary>
	private Point mGoalPos;
	private bool isGoalFlg;


    // Start is called before the first frame update
    void Start()
    {
		//ステージデータ読み取り
		int num = PlayerPrefs.GetInt(Common.Common.KEY_SELECTED_STAGE_ID);
		print(num);
		var data = LoadData.GetGameData(num);
		print(data);

        //各オブジェクト取得
        MG = GameObject.Find("MapGen").GetComponent<MapGeneretor>();
        player = GameObject.Find("Player").GetComponent<Player>();
        EM = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        //生成されたタイルの情報をもらう
        tiles = MG.Init(data.stage_data.floor_width,data.stage_data.floor_height, data.stage_data.min_division);

        //プレイヤの生成
        Generate("p", player);
        player.Init();
        //敵の生成
        EM.Init(this, data);

		//ゴールの生成
		CreateGoal();

		enemyActionFlg = false;
		mTimer = 0;
		isGoalFlg = false;
    }

	/// <summary>プレイヤから一定距離</summary>
	/// <returns></returns>
	private bool CheckPlayerAround(Point tpoint)
	{
		float z = Mathf.Abs(tpoint.X - GetPlayer().X) + Mathf.Abs(tpoint.Y - GetPlayer().Y);
		Debug.Log(z);
		return (5 < z);
	}

	private void CreateGoal()
	{
		Point point;
		TileInfo tile;
		bool flg = true;
		do//生成位置のかぶりをなくす
		{
			point = MG.GetRandomRoom();
			tile = CheckTile(point);
			Debug.Log(tile);
				flg = CheckPlayerAround(point);
			
		} while (!(tile == TileInfo.ROUTE && flg));

		var goal = (GameObject)Resources.Load("Prefabs/Goal");
		var clone = Instantiate(goal,this.transform);
		clone.transform.position = MG.GetTilePos(point);

		mGoalPos = point;

	}

	/// <summary>キャラクターの生成、引数で何者か決まる</summary>
	public void Generate(string tstr, CharacterBace tchara)
    {
        Point point;
        TileInfo tile;
		bool flg = true;
        do//生成位置のかぶりをなくす
        {
            point = MG.GetRandomRoom();
            tile = CheckTile(point);
			Debug.Log(tile);
			if(tstr != "p")
			{
				flg = CheckPlayerAround(point);
				if(!flg)
				{
					Debug.Log("近すぎる");
					//continue;
				}
			}
        } while (!(tile == TileInfo.ROUTE && flg));
		int hp = 1;
		switch(tstr)
		{
			case "p":
				hp = Common.Common.PULAYER_HP;
				break;
			case "e":
				hp = Common.Common.ENEMY_HP;
				break;
		}
        tchara.Init(this, point,hp, tstr);
		//tiles[point.X, point.Y] = tstr;
    }

    /// <summary>マス目情報更新</summary>
    public Vector3 SetPos(CharacterBace tchara, Point tafter, string tstr)
    {

        Point point = tchara.GetPos();
		print(point);
        tiles[point.X, point.Y] = " ";
        tiles[tafter.X, tafter.Y] = tstr;
        //print($"死んだ敵の座標：{tafter}");
        return MG.GetTilePos(tafter);

    }

    /// <summary>入力先のタイル情報確認</summary>
    public TileInfo CheckTile(Point tpoint)
    {
        if (tiles[tpoint.X, tpoint.Y] == " ")
        {
            //Debug.Log("移動可能");
            //Debug.Log(tiles[tpoint.X, tpoint.Y]);
            return TileInfo.ROUTE;
        }
        if (tiles[tpoint.X, tpoint.Y] == "w")
        {
            //Debug.Log("移動不可");
            //Debug.Log(tiles[tpoint.X, tpoint.Y]);

            return TileInfo.WALL;
        }
        if (tiles[tpoint.X, tpoint.Y] == "e")
        {
            //Debug.Log("敵がいるよ");
            return TileInfo.ENEMY;
        }
        if (tiles[tpoint.X, tpoint.Y] == "p")
        {
            //Debug.Log("プレイヤがいるよ");
            return TileInfo.PLAYER;
        }
        return TileInfo.NONE;
    }

    /// <summary>プレイヤの位置を返す</summary>
    public Point GetPlayer()
    {
        return player.GetPos();
    }

	//
	public Point  GetSize()
	{ 
		return new Point(tiles.GetLength(0),tiles.GetLength(1));
	}

	/// <summary>プレイヤーの攻撃申請を敵側に流す</summary>
	/// <param name="tpos"></param>
	public void PlayerAttack(Point tpos)
	{
		EM.Damege(tpos);
	}

	/// <summary>敵からの攻撃指示をプレイヤーに流す</summary>
	public void EnemyAttack()
	{
		player.Damage();
	}

	/// <summary>敵の行動がすべて終わったことを受け取る</summary>
	public void EnemyActionFinish()
	{
		enemyActionFlg = true;
	}

    /// <summary>プレイヤの行動を確認したら敵に行動指示を出す。</summary>
    public void PlayerAction()
    {
		if (player.GetPos() == mGoalPos)
		{
			print("ゴールにたどり着きました");
			isGoalFlg = true ;
			return;
		}

		Debug.Log("敵の行動に移ります");
        EM.EnemyActionSelect();
    }

	/// <summary>プレイヤ→敵の行動が終わったらディレイを入れてプレイヤの行動をできるようにする</summary>
	private void EndTurn()
	{
		if(enemyActionFlg ) 
		{
			mTimer += Time.deltaTime;

			if(mTimer > Common.Common.REAR_GAP_TIME)
			{
				player.SetStatus(Status.STAY);
				enemyActionFlg = false;

				mTimer = 0;
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (player.GetActive() == false)
		{
			print("カウント");
			mTimer += Time.deltaTime;
			if (0.5f <= mTimer)
			{
				GameObject.Find("EndUI").GetComponent<EndManager>().OpenOverPanel();
			}
		}

		if(isGoalFlg)
		{
			mTimer += Time.deltaTime;
			if (0.5f <= mTimer)
			{
				GameObject.Find("EndUI").GetComponent<EndManager>().OpenClear();
			}
		}


		EndTurn();
    }
}
