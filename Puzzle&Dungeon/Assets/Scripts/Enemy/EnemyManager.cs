using System;
using System.Drawing;
using UnityEngine;
using Common;
using Data;
using static UnityEngine.EventSystems.EventTrigger;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    
    //�萔
    /// <summary>�Q�[���Ǘ���</summary>
    private GameManager cGm;
    /// <summary>�}�b�v�Ǘ���</summary>
    private MapManager cMm;
    /// <summary>ui�}�l�[�W��</summary>
    private UIManager cUm;
    /// <summary>�G�Ǘ��p�z��</summary>
    private List<Enemy> cEnemys;
    /// <summary>�G�l�~�[�����p�I�u�W�F�N�g</summary>
    private GameObject cEnemy;
    /// <summary>�o�H�T���]���p�^�C��</summary>
    private Tile[,] cTiles;
    /// <summary>�^�C���̃`�F�b�N���</summary>
    private enum TileStatus
    {
        NONE,
        OPEN,
        CLOSE,
        RECRUIT,
        START,
        END
    }

	/// <summary>�Q�[���f�[�^</summary>
	private JsonGameData mGameData;

    //�����o�ϐ�
    /// <summary>�X�e�[�^�X</summary>
	private Status mStatus;
    /// <summary>�X�e�[�^�X�ύX�����ĂԂ���</summary>
	private bool isStatus;
    /// <summary>�s�����s������</summary>
	private int[] mActionOrder;
    /// <summary>�s���̏I�����݂�t���O</summary>
    private bool isAction;


    //�����o�֐�  
    //private
    /// <summary>�G�̒��I</summary>
	private int LotteryEnemy(JsonStageEnemyData[] tdata)
	{
		int sumRate =0;

		for(int i=0;i <tdata.Length;i++)
		{
			sumRate += (int)tdata[i].spawn_rate;
		}
		print($"�������v�F{sumRate}");
		int ram = UnityEngine.Random.Range(1,sumRate+1);
		print($"���I���ʁF{ram}");

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
	/// <summary>id����v����G�������Ă���</summary>
	private JsonEnemyData LookUseEnemys(JsonGameData tdata, int tnum)
	{
		//print(tdata.enemy_datas[0].enemy_name);

		//�X�e�[�W�Ɍ����G������
		for(int i=0;i< tdata.enemy_datas.Length;i++)
		{
				if (tdata.enemy_datas[i] == null)
				{
					print("����ȓG�͂��Ȃ���");
					continue;
				}

				if(tnum == tdata.enemy_datas[i].enemy_id)
				{
					print($"{tdata.enemy_datas[i].enemy_name}���o�������܂��B");
					return tdata.enemy_datas[i];
				}
		}
		return null;
	}   
    /// <summary>�����^�C�����Z�b�g</summary>
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
    /// <summary>�ڕW�܂ł̌o�H�T��</summary>
    private void SearchTarget(Point tpPos)
    {

        //�����Ώۂ̈ʒu���擾(���[�v)
        for(int i = 0;i < cEnemys.Count;i++)
        {
            SeachOne(i, tpPos);
        }
    }
	/// <summary>�o�H�T��</summary>
    private void SeachOne(int i,Point tpPos)
    {
        cEnemys[i].ResetRoute();
        Point my = cEnemys[i].GetPos();
        Debug.Log(my);
        cTiles[my.X, my.Y].SetTile(TileStatus.START);
        cTiles[tpPos.X, tpPos.Y].SetTile(TileStatus.END);

        bool flg;
        flg = CheckAround(my, tpPos, cTiles[my.X, my.Y].GetBfore() + 1);

        //�I������
        //���͂ɓ_����t����Ƃ��ɃS�[��������
        while (!flg)
        {
            //�]���������}�X�̌���(���[�v)
            Point nextTile = SearchTile();
            cTiles[nextTile.X, nextTile.Y].SetTile(TileStatus.CLOSE);

            //���͂̃}�X�ɓ_����t����
            flg = CheckAround(nextTile, tpPos, cTiles[nextTile.X, nextTile.Y].GetBfore() + 1);
        }

        //�S�[������X�^�[�g�܂œ��؂������̂ڂ��ċL�����Ă���
        FollowRoute(tpPos, cEnemys[i], 0);

        //mEnemy[i].ShowRoute();

        //�^�C�������Z�b�g���Ď���
        ResetTiles();

    }

    //public
    /// <summary>����������</summary>
    public void Init(GameManager tgm,MapManager tmm)
    {
        //�����l�ݒ�
        {
            cUm = GameObject.Find("UIManager").GetComponent<UIManager>();


            //mGameData = tgameData;
            cGm = tgm;
            cMm = tmm;
            //enemy = (GameObject)Resources.Load("Prefabs/Enemy");
            cEnemy = (GameObject)Resources.Load("Prefabs/Ant");
            cEnemys = new List<Enemy>();


			Point point = cMm.GetTileLength();//�}�X�ڂ̑傫�����m�肽��
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
        //�w�萔�G�𐶐�����
        for (int i = 0; i < cEnemys.LongLength; i++)
        {
            //��������G�����߂�
            print(mGameData);
            print(mGameData.stage_data);
            print(mGameData.stage_data.enemy_datas);
			int id = LotteryEnemy(mGameData.stage_data.enemy_datas);
			print($"��������̂�id{id}");
			var enemyData = LookUseEnemys(mGameData, id);
			print($"{enemyData.enemy_name}");
			//�v���t�@�u��K�p������
			enemy = (GameObject)Resources.Load("Prefabs/" + enemyData.enemy_id_name);

			var clone = Instantiate(enemy);
            clone.transform.SetParent(this.gameObject.transform, false);
            clone.transform.localPosition = new Vector3(0,0, 0);
            mEnemys[i] = clone.GetComponent<Enemy>();
            //master.GeneratePlayer("e", mEnemys[i]);
            mEnemys[i].Init(this);
        }
        //�o�H�T��
        SearchTarget(master.GetPlayer());
        */
    }
    /// <summary>����</summary>
	public void Generate(string tstr, Enemy bace)
	{
		//cGm.GeneratePlayer(tstr, bace);
        bace.Init(this,cGm,cMm,new PosId(),1,"e");

    }
    /// <summary>�����_�����̓G����</summary>
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
    //�G������𒴂��Ă��Ȃ���

    /// <summary>���Ɍ������n�߂�^�C�������߂�</summary>
    private Point SearchTile()
    {
        Point point = Point.Empty;

        for(int i= 0;i<cTiles.GetLength(0);i++)
        {
            for(int j = 0;j<cTiles.GetLength(1);j++)
            {
                //�]�����Ă��Ȃ��^�C���͖���
                if (cTiles[i,j].GetTile() != TileStatus.OPEN)
                {
                    //Debug.Log($"�^�C��{i},{j}�̏��:{mTiles[i, j].GetTile()}");
                    continue;
                }

                if (cTiles[i,j].GetScore() < cTiles[point.X,point.Y].GetScore() || point == Point.Empty)
                {
                    //Debug.Log($"�^�C���X�V:{i},{j}");
                    point.X = i; point.Y = j;
                }
            }
        }

        return point;
    }
    /// <summary>�l�����`�F�b�N</summary>
    private bool CheckAround(Point tpos, Point tendPos, float tbfore)
    {
        Point point;
        bool flg;
        //Debug.Log((int)Enum.Parse(typeof(Vector), "NONE"));

        //���������V���b�t��
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
            //print($"��������{vec[i]}");
            //���悤�Ƃ��Ă���}�X���ړ��\�}�X���A�܂��v�Z���ĂȂ��}�X��
            point = tpos;
            if (vec[i] == Vector.RIGHT) { point.X++; }//RIGHT
            if (vec[i] == Vector.LEFT) { point.X--; }//LEFT
            if (vec[i] == Vector.UP) { point.Y--; }//UP
            if (vec[i] == Vector.DOWN) { point.Y++; }//DOWN
            if (cTiles[point.X, point.Y].GetTile() == TileStatus.END) 
            {
                Debug.Log("�v���C������!!");
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
    /// <summary>�ŒZ�o�H���L�^����</summary>
    private void FollowRoute(Point tpos, Enemy tenemy, int num)
    {
        num++;
        if(num == 800)
        {
            Debug.LogError("�o�H�T���Ɏ��s");
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
    /// <summary>�v���C���[�ɋ߂����ɍs�������߂�</summary>
	private int[] SortEnemys()
	{
		//�v���C���ɋ߂����ɍs�����s��
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
    /// <summary>�s������</summary>
    public void EnemyActionSelect()
    {

        //print("aaa");
        isAction = false;

		//�s��������
		mActionOrder =  SortEnemys();

		//�s��
        bool first = true, second = true;
        for (int i = 0; i < cEnemys.Count; i++)
        {
			Enemy tenemy = cEnemys[mActionOrder[i]];

            if (!tenemy.GetActive()) { continue; }




			//���݂��Ȃ��ꍇ�X�L�b�v
			if (tenemy.GetActive() == false )
			{
				tenemy.SetStatus(Status.REAR_GAP);
				continue; 
			}

			//�v���C�����߂��ꍇ�v���C���̍s���ɑΉ������������s��
			float z = Mathf.Abs(tenemy.GetPos().X - cGm.GetPlayer().X) + Mathf.Abs(tenemy.GetPos().Y - cGm.GetPlayer().Y);
			if (z < 5)
			{
				SeachOne(mActionOrder[i], cGm.GetPlayer());
			}


			first = tenemy.Tracking();
            //z = Mathf.Abs(tenemy.GetPos().X - master.GetPlayer().X) + Mathf.Abs(tenemy.GetPos().Y - master.GetPlayer().Y);
            if (!first)
            {
                print("1�x�ڂ̍s�������������̂Ń��[�g�̍Č������s���܂�");
                SeachOne(mActionOrder[i], cGm.GetPlayer());
            }
            second = tenemy.Tracking();
			if(first == second) //�ړ����Ȃ��������Ɉړ��ς݂�
			{
				Debug.Log("�ړ��悪����܂���ł���");
				tenemy.SetStatus(Status.REAR_GAP);
			}
        }
        //SearchTarget(master.GetPlayer());

        SetStatus(Status.ATTACK);
    }
	/// <summary>�X�e�[�g�����s��Ԃɂ���</summary>
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
	/// <summary>�U���̓G���U�����I���Ă��邩</summary>
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
	/// <summary>�v���C���̍U������</summary>
	public void EnemyAttack()
	{
		master.EnemyAttack();
	}
    */
	/// <summary>�����̍��W�̓G�Ƀ_���[�W��^����</summary>
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
	/// <summary>�ړ��̓G���ړ����I���Ă��邩</summary>
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
    /// <summary>���ׂĂ��������Ă��邩</summary>
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

    //Set�֐�
    /// <summary>�X�e�[�^�X�̕ύX</summary>
	private void SetStatus(Status tstatus)
	{
		mStatus = tstatus;
		isStatus = true;
	}

    //Get�֐�
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
					//Debug.Log("�v���C���̍s���҂��ł�");
					isStatus = false;
				}
				break;
			case Status.MOVE:
				if(isStatus)
				{
					//Debug.Log("�ړ��̓G�̏������s���܂�");
					ChangeAction(Status.MOVE);
                    ChangeAction(Status.TRUN);
					isStatus = false;
				}
				Move();
				break;
			case Status.ATTACK:
				if(isStatus)
				{
					//Debug.Log("�U���̓G�̏������s���܂�");
					ChangeAction(Status.ATTACK);
					isStatus = false;
				}
				Attack();
				break;
			case Status.REAR_GAP:
				if(isStatus)
				{
					//Debug.Log("�s�����������܂���");
					//master.EnemyActionFinish();
					RearGap();
					isStatus = false;
				}
				break;
		}


    }


    //�T�����i�[�p�N���X
    private class Tile
    {
        /// <summary>�}�X�^�[���</summary>
        private MapManager cMm;

        //�ǂ����痈����
        private Vector mDir;
        //�X�R�A
        private float mScore;
        //�����ɗ���܂ł̕���
        private float mBfore;
        //���g�̏��
        private TileStatus mStatus;

        /// <summary>���݂̏�Ԃ�Ԃ�</summary>
        public TileStatus GetTile()
        {
            return mStatus;
        }
        /// <summary>mBfore��Ԃ�</summary>
        public float GetBfore()
        {
            return mBfore;
        }
        /// <summary>mScore��Ԃ�</summary>
        public float GetScore()
        {
            return mScore;
        }
        /// <summary>mDir��Ԃ�</summary>
        public Vector GetDir()
        {
            return mDir;
        }

        /// <summary>�����̃X�e�[�^�X�ɕύX</summary>
        /// <param name="tstatus"></param>
        public void SetTile(TileStatus tstatus)
        {
            mStatus = tstatus;
        }
        /// <summary>���������ۑ�</summary>
        public void SetDir(Vector tvec)
        {
            mDir = tvec;
        }

        //����������
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

        /// <summary>�^�C���̏��v�Z</summary>
        public TileStatus OpenTile(Point tpos, Point tendPos,Vector tdir, float tbfore)
        {
            SetTile(TileStatus.OPEN);
            mDir = tdir;
            mBfore=tbfore;
            mScore =CheckLength(tpos,tendPos) + mBfore;
            //Debug.Log($"�^�C��:{tpos}�X�R�A:{mScore}");
            return mStatus;
        }
        
        /// <summary>�X�^�[�g����S�[���܂ōŒZ�̈ړ���</summary>
        private float CheckLength(Point tstart, Point tgoal)
        {
            float length = Mathf.Abs(tgoal.Y - tstart.Y) + Mathf.Abs(tgoal.X - tstart.X);
            if (cMm.CheckTile(tstart) == TileInfo.ENEMY)
            {
                print("�G�����邽�߃y�i���e�B");
                length += 100.1f;
            }
            /*
            float width = Mathf.Abs(tgoal.X - tstart.X);
            float height = Mathf.Abs(tgoal.Y - tstart.Y);

            //�������̌���
            Point point = tstart;
            for(int i = 0;i < width;i++)
            {
                point.X += i;
                var tmp = master.CheckTile(point);
                print($"�`�F�b�N�����^�C��{tmp}");
            }
            */

            //float length;
            //Debug.Log(length);
            return length;
            
        }

    }

	

}
