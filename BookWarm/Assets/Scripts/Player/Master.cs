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
    [Tooltip("�}�b�v�������N�G�X�g�p"), SerializeField]
    private MapGeneretor MG;

    [Tooltip("�v���C��"), SerializeField]
    private Player player;

    [Tooltip("�G�l�~�[�Ǘ���"), SerializeField]
    private EnemyManager EM;


    [Tooltip("�^�C���̏��"), SerializeField]
    private string[,] tiles;


	//�㌄�J�E���g
	private bool enemyActionFlg;
	private float mTimer;

	/// <summary>�S�[���n�_</summary>
	private Point mGoalPos;
	private bool isGoalFlg;


    // Start is called before the first frame update
    void Start()
    {
		//�X�e�[�W�f�[�^�ǂݎ��
		int num = PlayerPrefs.GetInt(Common.Common.KEY_SELECTED_STAGE_ID);
		print(num);
		var data = LoadData.GetGameData(num);
		print(data);

        //�e�I�u�W�F�N�g�擾
        MG = GameObject.Find("MapGen").GetComponent<MapGeneretor>();
        player = GameObject.Find("Player").GetComponent<Player>();
        EM = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        //�������ꂽ�^�C���̏������炤
        tiles = MG.Init(data.stage_data.floor_width,data.stage_data.floor_height, data.stage_data.min_division);

        //�v���C���̐���
        Generate("p", player);
        player.Init();
        //�G�̐���
        EM.Init(this, data);

		//�S�[���̐���
		CreateGoal();

		enemyActionFlg = false;
		mTimer = 0;
		isGoalFlg = false;
    }

	/// <summary>�v���C�������苗��</summary>
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
		do//�����ʒu�̂��Ԃ���Ȃ���
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

	/// <summary>�L�����N�^�[�̐����A�����ŉ��҂����܂�</summary>
	public void Generate(string tstr, CharacterBace tchara)
    {
        Point point;
        TileInfo tile;
		bool flg = true;
        do//�����ʒu�̂��Ԃ���Ȃ���
        {
            point = MG.GetRandomRoom();
            tile = CheckTile(point);
			Debug.Log(tile);
			if(tstr != "p")
			{
				flg = CheckPlayerAround(point);
				if(!flg)
				{
					Debug.Log("�߂�����");
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

    /// <summary>�}�X�ڏ��X�V</summary>
    public Vector3 SetPos(CharacterBace tchara, Point tafter, string tstr)
    {

        Point point = tchara.GetPos();
		print(point);
        tiles[point.X, point.Y] = " ";
        tiles[tafter.X, tafter.Y] = tstr;
        //print($"���񂾓G�̍��W�F{tafter}");
        return MG.GetTilePos(tafter);

    }

    /// <summary>���͐�̃^�C�����m�F</summary>
    public TileInfo CheckTile(Point tpoint)
    {
        if (tiles[tpoint.X, tpoint.Y] == " ")
        {
            //Debug.Log("�ړ��\");
            //Debug.Log(tiles[tpoint.X, tpoint.Y]);
            return TileInfo.ROUTE;
        }
        if (tiles[tpoint.X, tpoint.Y] == "w")
        {
            //Debug.Log("�ړ��s��");
            //Debug.Log(tiles[tpoint.X, tpoint.Y]);

            return TileInfo.WALL;
        }
        if (tiles[tpoint.X, tpoint.Y] == "e")
        {
            //Debug.Log("�G�������");
            return TileInfo.ENEMY;
        }
        if (tiles[tpoint.X, tpoint.Y] == "p")
        {
            //Debug.Log("�v���C���������");
            return TileInfo.PLAYER;
        }
        return TileInfo.NONE;
    }

    /// <summary>�v���C���̈ʒu��Ԃ�</summary>
    public Point GetPlayer()
    {
        return player.GetPos();
    }

	//
	public Point  GetSize()
	{ 
		return new Point(tiles.GetLength(0),tiles.GetLength(1));
	}

	/// <summary>�v���C���[�̍U���\����G���ɗ���</summary>
	/// <param name="tpos"></param>
	public void PlayerAttack(Point tpos)
	{
		EM.Damege(tpos);
	}

	/// <summary>�G����̍U���w�����v���C���[�ɗ���</summary>
	public void EnemyAttack()
	{
		player.Damage();
	}

	/// <summary>�G�̍s�������ׂďI��������Ƃ��󂯎��</summary>
	public void EnemyActionFinish()
	{
		enemyActionFlg = true;
	}

    /// <summary>�v���C���̍s�����m�F������G�ɍs���w�����o���B</summary>
    public void PlayerAction()
    {
		if (player.GetPos() == mGoalPos)
		{
			print("�S�[���ɂ��ǂ蒅���܂���");
			isGoalFlg = true ;
			return;
		}

		Debug.Log("�G�̍s���Ɉڂ�܂�");
        EM.EnemyActionSelect();
    }

	/// <summary>�v���C�����G�̍s�����I�������f�B���C�����ăv���C���̍s�����ł���悤�ɂ���</summary>
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
			print("�J�E���g");
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
