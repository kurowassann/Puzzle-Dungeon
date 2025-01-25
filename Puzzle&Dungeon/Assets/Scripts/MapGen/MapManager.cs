using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using Unity.VisualScripting;
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
    //
    private GameManager cGm;
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
    //���ׂĂ̐ڑ������̏��
    private List<RoomJoint> cJoints;

    //�����o�ϐ�
    /// <summary>�����\���p�ԍ�</summary>
    private int mNum;
    /// <summary>�L���\���p�ԍ�</summary>
    private int mNum2;
    /// <summary>�v���C���̏ꏊ</summary>
    private RoomAisle mPlayerRA;

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
    /// <summary> �ڑ����������X�g��</summary>
    private void CreateJointList()
    {
        for (int i = 0; i < cAm.GetAisleCount(); i++)
        {
            RoomJoint[] Rjs = cAm.GetAisle(i).GetRoomJoint();
            for (int j = 0; j < Rjs.Length; j++)
            {
                cJoints.Add(Rjs[j]);
                //Debug.Log($"�ڑ�������{Rjs[j].GetPos()}�ł�");
            }
        }

    }


    //public
    /// <summary>����������</summary>
    public void Init(GameManager tgm)
    {
        cGm = tgm;

        //�}�b�v�����I�u�W�F�N�g�̐���
        GameObject clone = Instantiate(mapGen, this.transform);
        cMg = clone.GetComponent<MapGeneretor>();
        cMg.Init(45,30,12);

        //���������}�b�v�̏����󂯎��
        cTiles = cMg.GetStrings();
        cTileObjects = cMg.GetTileObjects();
        cRm = cMg.GetRoomManager();
        cAm = cMg.GetAisleManager();
        cJoints = new List<RoomJoint>();
        CreateJointList();

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
    //
    public PosId ChangeId(PosId tpi)
    {
        int num = -1;
        PosId posId = tpi;
        RoomAisle ra = RoomAisle.NONE;
        //�ړ��O�̕����ԍ��E����ID�ƈړ���̍��W�������Ă���

        //ID�ɂȂ���ꏊ�̍��W�����ׂ�(�ڑ����̍��W�̃��X�g�������Ă���)
        switch(mPlayerRA)
        {
            case RoomAisle.JOINT:
                for(int i = 0;i < cJoints.Count;i++)
                {
                    if(posId.GetPos() == cJoints[i].GetRoomPos())
                    {
                        ra = RoomAisle.ROOM;
                        num = i;
                        break;
                    }
                    if(posId.GetPos() == cJoints[i].GetAislePos())
                    {
                        ra = RoomAisle.AISLE;
                        num = i;
                        break;
                    }
                }
                break;
            case RoomAisle.ROOM:
            case RoomAisle.AISLE:
                for(int i = 0; i < cJoints.Count; i++)
                {
                    if(posId.GetPos() == cJoints[i].GetJointPos())
                    {
                        ra = RoomAisle.JOINT;
                        num = i;
                        break;
                    }
                }
                break;
        }

        //�ꏊ�ړ��Ȃ������ꍇ
        if(num == -1)
        {
            return posId;
        }
        else 
        {
            posId.SetId(num, ra); 
            mPlayerRA = posId.GetRA();
            //�����_���`�F�b�N
            if(ra == RoomAisle.JOINT)
            {
                bool rFlg;
                bool aFlg;
                
                
                rFlg = cRm.OpenOneRoom(cJoints[num].GetId(RoomAisle.ROOM));
                aFlg = cAm.OpenOneAisle(cJoints[num].GetId(RoomAisle.AISLE));

                if(!rFlg)
                {
                    //�G�̐������˗�����B
                    cGm.RoomOpen(cJoints[num].GetId(RoomAisle.ROOM));
                }
            }
        }



        return posId;
    }

    //Set�֐�
    /// <summary>�ʒu�̍X�V</summary>
    public Vector3 SetPos(CharacterBace tchara, Point tafter, string tstr)
    {
        Point point = tchara.GetPos();
        cTiles[point.X, point.Y] = " ";
        cTiles[tafter.X, tafter.Y] = tstr;
        return GetTilePos(tafter);
    }


    //Get�֐�
    /// <summary>�����_���ȕ����̃����_���ȍ��W��Ԃ�</summary>
    /*
    public Point GetRamdomPos()
    {
        
        Point pos;

        pos = cRm.GetRandomRoom();

        return pos;
        
    }*/
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
    //
    public Point GetTileLength()
    {
        return new Point(cTiles.GetLength(0), cTiles.GetLength(1));
    }
    //
    public PosId GetPosId(int num)
    {
        PosId pi;

        pi = cRm.GetRandomPos(num);

        return pi;
    }


    /// <summary>�X�^�[�g�֐�</summary>
    public void Start()
    {
        Debug.Log($"{this.name}�X�^�[�g");

        //if(isStart) { Init(); }
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

            if(Input.GetKeyDown(KeyCode.B))
            {
                
            }

        }//�f�o�b�N�I���
    }

}
