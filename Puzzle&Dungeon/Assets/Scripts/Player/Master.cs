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
    /*
    //�I�u�W�F�N�g(�萔)
    /// <summary> �}�b�v�������N�G�X�g�p</summary> 
    private MapManager mm;
    /// <summary>�Q�[���i�s�N���X</summary>
    private GameManager gm;


    [Tooltip("�^�C���̏��"), SerializeField]
    private string[,] tiles;
    [Tooltip("�v���C��"), SerializeField]
    private Player player;

    //�����o�ϐ�
    /// <summary>�G�̍s�����I�������</summary>
    private bool enemyActionFlg;
    /// <summary>���Ԍv��</summary>
    private float mTimer;
    /// <summary>�S�[���n�_</summary>
    private Point mGoalPos;
    /// <summary>�Q�[���I���p</summary>
    private bool isGoalFlg;

    //�����o�֐�
    /// <summary>�v���C�������苗����Ă��邩</summary>
    private bool CheckPlayerAround(Point tpoint)
    {
        float z = Mathf.Abs(tpoint.X - GetPlayer().X) + Mathf.Abs(tpoint.Y - GetPlayer().Y);
        //Debug.Log(z);
        return (5 < z);
    }
    /// <summary>�S�[���ʒu�̐���</summary>
    private void CreateGoal()
    {
        Point point;
        TileInfo tile;
        bool flg = true;
        do//�����ʒu�̂��Ԃ���Ȃ���
        {
            point = rm.GetRandomRoom();
            tile = CheckTile(point);
            Debug.Log(tile);
            flg = CheckPlayerAround(point);

        } while (!(tile == TileInfo.ROUTE && flg));

        var goal = (GameObject)Resources.Load("Prefabs/Goal");
        var clone = Instantiate(goal, this.transform);
        clone.transform.position = MG.GetTilePos(point);

        mGoalPos = point;

    }
    /// <summary>�v���C���[�̐����A�����ŉ��҂����܂�</summary>
    public void GeneratePlayer(Player tplayer)
    {
        Point point;
        TileInfo tile;
        bool flg = true;
        do//�����ʒu�̂��Ԃ���Ȃ���
        {
            point = rm.GetRandomRoom(0);
            tile = CheckTile(point);
            Debug.Log(tile);
        } while (!(tile == TileInfo.ROUTE && flg));
        int hp = 1;
                hp = Common.Common.PULAYER_HP;
        tplayer.Init(this, point, hp, "p");
        //tiles[point.X, point.Y] = tstr;
    }
    /// <summary>���͐�̃^�C�����m�F</summary>
    public TileInfo CheckTile(Point tpoint)
    {
        TileInfo tile;
        switch (tiles[tpoint.X, tpoint.Y])
        {
            case " ":
                //Debug.Log("�ړ��\");
                //Debug.Log(tiles[tpoint.X, tpoint.Y]);
                tile = TileInfo.ROUTE;
                break;
            case "w":
                //Debug.Log("�ړ��s��");
                //Debug.Log(tiles[tpoint.X, tpoint.Y]);

                tile = TileInfo.WALL;
                break;
            case "e":
                //Debug.Log("�G�������");
                tile = TileInfo.ENEMY;
                break;
            case "p":
                //Debug.Log("�v���C���������");
                tile = TileInfo.PLAYER;
                break;
            default:
                tile = TileInfo.NONE;
                break;
        }

        return tile;
    }
    /// <summary>�v���C���[�̍U���\����G���ɗ���</summary>
    public void PlayerAttack(Point tpos)
    {
        em.Damege(tpos);
    }
    /// <summary>�v���C���̍s�����m�F������G�ɍs���w�����o���B</summary>
    public void PlayerAction()
    {
        if (player.GetPos() == mGoalPos)
        {
            print("�S�[���ɂ��ǂ蒅���܂���");
            isGoalFlg = true;
            return;
        }

        //Debug.Log("�G�̍s���Ɉڂ�܂�");
        em.EnemyActionSelect();
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
    /// <summary>�v���C�����G�̍s�����I�������f�B���C�����ăv���C���̍s�����ł���悤�ɂ���</summary>
    private void EndTurn()
    {
        if (enemyActionFlg)
        {
            mTimer += Time.deltaTime;

            if (mTimer > Common.Common.REAR_GAP_TIME)
            {
                player.SetStatus(Status.STAY);
                enemyActionFlg = false;

                mTimer = 0;
            }
        }
    }

    //Set�֐�
    /// <summary>�}�X�ڏ��X�V</summary>
    public Vector3 SetPos(CharacterBace tchara, Point tafter, string tstr)
    {

        Point point = tchara.GetPos();
        //print(point);
        tiles[point.X, point.Y] = " ";
        tiles[tafter.X, tafter.Y] = tstr;
        //print($"���񂾓G�̍��W�F{tafter}");
        return MG.GetTilePos(tafter);

    }

    //Get�֐�
    /// <summary>�v���C���̈ʒu��Ԃ�</summary>
    public Point GetPlayer()
    {
        return player.GetPos();
    }
    /// <summary>�X�e�[�W�̑傫�����擾</summary>
    public Point GetSize()
    {
        return new Point(tiles.GetLength(0), tiles.GetLength(1));
    }

    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�W�f�[�^�ǂݎ��
        int num = PlayerPrefs.GetInt(Common.Common.KEY_SELECTED_STAGE_ID);
        //print(num);
        var data = LoadData.GetGameData(num);
        //print(data);

        //�e�K�v�I�u�W�F�N�g�擾
        mm = new MapManager();
        player = GameObject.Find("Player").GetComponent<Player>();
        em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        //�������ꂽ�^�C���̏������炤
        tiles = MG.Init(data.stage_data.floor_width, data.stage_data.floor_height, data.stage_data.min_division);
        //rm = MG.GetRoomManager();        


        //�v���C���̐���
        GeneratePlayer(player);
        //�G�̐���
        em.Init(this, data);

        //�S�[���̐���
        CreateGoal();

        enemyActionFlg = false;
        mTimer = 0;
        isGoalFlg = false;
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

        if (isGoalFlg)
        {
            mTimer += Time.deltaTime;
            if (0.5f <= mTimer)
            {
                GameObject.Find("EndUI").GetComponent<EndManager>().OpenClear();
            }
        }


        EndTurn();
    }
    */
}
