using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>���������}�b�v���Ǘ�����N���X</summary>
public class MapManager : MonoBehaviour
{
    //SerializeField unity���Q��
    [Tooltip("�f�o�b�O��\�����邩"), SerializeField]
    private bool isDebug;
    [Tooltip("�����ŊJ�n���邩"),SerializeField]
    private bool isStart;
    [Tooltip("�}�b�v�����I�u�W�F�N�g"), SerializeField]
    private GameObject mapGen;

    //�I�u�W�F�N�g
    /// <summary>�����Ǘ��p�N���X</summary>
    private RoomManager cRm;
    /// <summary>�L���Ǘ��N���X</summary>
    private AisleManager cAm;
    /// <summary>�}�b�v�����p�N���X</summary>
    private MapGeneretor cMg;
    /// <summary>�^�C�����</summary>
    private string[,] cTiles;
    /// <summary>�^�C���̃I�u�W�F�N�g</summary>
    private GameObject[,] cTileObjects;

    //�����o�ϐ�
    /// <summary>�����\���p�ԍ�</summary>
    private int mNum;
    /// <summary>�L���\���p�ԍ�</summary>
    private int mNum2;

    //�����o�֐�
    //private
    /// <summary>���ׂẴ^�C����\��</summary>
    private void TileOpen()
    {
        for (int i = 0; i < cTileObjects.GetLength(0); i++)
        {
            for (int j = 0; j < cTileObjects.GetLength(1); j++)
            {
                cTileObjects[i, j].SetActive(true);
            }
        }
    }


    //public
    /// <summary>����������</summary>
    public void Init()
    {
        //�}�b�v�����I�u�W�F�N�g�̐���
        GameObject clone = Instantiate(mapGen, this.transform);
        cMg = clone.GetComponent<MapGeneretor>();
        cMg.Init(120,40,15);

        //���������}�b�v�̏����󂯎��
        cTiles = cMg.GetStrings();
        cTileObjects = cMg.GetTileObjects();
        cRm = cMg.GetRoomManager();
        cAm = cMg.GetAisleManager();


        mNum = 0;
        mNum2 = 0;
    }
    /// <summary>�ړ����悤�Ƃ��Ă��镔��������</summary>
    public TileInfo CheckTile(Point tpos)
    {
        TileInfo tile;
        switch (cTiles[tpos.X, tpos.Y])
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


    //Set�֐�
    /// <summary>�ʒu�̍X�V</summary>
    public Vector3 SetPos(CharacterBace tchara, Point tafter, string tstr)
    {

        Point point = tchara.GetPos();
        //print(point);
        cTiles[point.X, point.Y] = " ";
        cTiles[tafter.X, tafter.Y] = tstr;
        //print($"���񂾓G�̍��W�F{tafter}");
        return GetTilePos(tafter);

    }


    //Get�֐�
    /// <summary>�����_���ȕ����̃����_���ȍ��W��Ԃ�</summary>
    public Point GetRamdomPos()
    {
        Point pos;

        pos = cRm.GetRandomRoom();

        return pos;
    }
    //���W�̈ʒu��Ԃ�
    public Vector3 GetTilePos(Point tpos)
    {
        return cTileObjects[tpos.X, tpos.Y].transform.position;
    }
    /// <summary>�v���C���̏����l</summary>
    public PosId GetRandomPlayer()
    {
        PosId posId;
        int num;
        Point pos;

        num= Random.Range(0, cRm.GetRoomCount());

        pos = cRm.GetRandomRoom(num);

        posId = new PosId(pos,num, RoomAisle.ROOM);

        return posId;
    }


    /// <summary>�X�^�[�g�֐�</summary>
    public void Start()
    {
        Debug.Log($"{this.name}�X�^�[�g");

        if(isStart) { Init(); }
    }

    public void Update() 
    {
        if(isDebug)
        { 
            if(Input.GetKeyDown(KeyCode.A))
            {
                if(mNum2 < cAm.GetAisleCount())
                {
                    print($"�L���ԍ�{mNum2}��\�����܂�");
                    cAm.OpenOneAisle(mNum2);
                    mNum2++;
                }
                else
                {
                    print($"�\���ł���L����{mNum2}�ŏI���ł�");
                }

            }

            if (Input.GetKeyDown(KeyCode.Space) ) 
            {
                if (mNum < cRm.GetRoomCount())
                {
                    print($"�����ԍ�{mNum}��\�����܂�");
                    cRm.OpenOneRoom(mNum); 
                    
                    mNum++;
                }
                else
                {
                    print($"�\���ł��镔����{mNum}�ŏI���ł�");
                }

            }

            if(Input.GetKeyDown(KeyCode.O))
            {
                print("�}�b�v�S�̂�\��");
                TileOpen();
            }
        }
    }

}
