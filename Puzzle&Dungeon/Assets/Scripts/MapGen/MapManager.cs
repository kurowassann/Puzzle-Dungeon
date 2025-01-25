using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>生成したマップを管理するクラス</summary>
public class MapManager : MonoBehaviour
{
    //SerializeField unity内参照
    [Tooltip("デバッグを表示するか"), SerializeField]
    private bool isDebug;
    [Tooltip("自分で開始するか"),SerializeField]
    private bool isStart;
    [Tooltip("マップ生成オブジェクト"), SerializeField]
    private GameObject mapGen;

    //オブジェクト
    //
    private GameManager cGm;
    /// <summary>部屋管理用クラス</summary>
    private RoomManager cRm;
    /// <summary>廊下管理クラス</summary>
    private AisleManager cAm;
    /// <summary>マップ生成用クラス</summary>
    private MapGeneretor cMg;
    /// <summary>タイル情報</summary>
    private string[,] cTiles;
    /// <summary>タイルのオブジェクト</summary>
    private GameObject[,] cTileObjects;
    //すべての接続部分の情報
    private List<RoomJoint> cJoints;

    //メンバ変数
    /// <summary>部屋表示用番号</summary>
    private int mNum;
    /// <summary>廊下表示用番号</summary>
    private int mNum2;
    /// <summary>プレイヤの場所</summary>
    private RoomAisle mPlayerRA;

    //メンバ関数
    //private
    /// <summary>すべてのタイルを表示</summary>
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
    /// <summary> 接続部分をリスト化</summary>
    private void CreateJointList()
    {
        for (int i = 0; i < cAm.GetAisleCount(); i++)
        {
            RoomJoint[] Rjs = cAm.GetAisle(i).GetRoomJoint();
            for (int j = 0; j < Rjs.Length; j++)
            {
                cJoints.Add(Rjs[j]);
                //Debug.Log($"接続部分は{Rjs[j].GetPos()}です");
            }
        }

    }


    //public
    /// <summary>初期化処理</summary>
    public void Init(GameManager tgm)
    {
        cGm = tgm;

        //マップ生成オブジェクトの生成
        GameObject clone = Instantiate(mapGen, this.transform);
        cMg = clone.GetComponent<MapGeneretor>();
        cMg.Init(45,30,12);

        //生成したマップの情報を受け取る
        cTiles = cMg.GetStrings();
        cTileObjects = cMg.GetTileObjects();
        cRm = cMg.GetRoomManager();
        cAm = cMg.GetAisleManager();
        cJoints = new List<RoomJoint>();
        CreateJointList();

        mNum = 0;
        mNum2 = 0;
    }
    /// <summary>移動しようとしている部分が何か</summary>
    public TileInfo CheckTile(Point tpos)
    {
        TileInfo tile;
        switch (cTiles[tpos.X, tpos.Y])
        {
            case " ":
                //Debug.Log("移動可能");
                //Debug.Log(tiles[tpoint.X, tpoint.Y]);
                tile = TileInfo.ROUTE;
                break;
            case "w":
                //Debug.Log("移動不可");
                //Debug.Log(tiles[tpoint.X, tpoint.Y]);

                tile = TileInfo.WALL;
                break;
            case "e":
                //Debug.Log("敵がいるよ");
                tile = TileInfo.ENEMY;
                break;
            case "p":
                //Debug.Log("プレイヤがいるよ");
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
        //移動前の部屋番号・部屋IDと移動後の座標が送られてくる

        //IDにつながる場所の座標か調べる(接続部の座標のリストを持っておく)
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

        //場所移動なかった場合
        if(num == -1)
        {
            return posId;
        }
        else 
        {
            posId.SetId(num, ra); 
            mPlayerRA = posId.GetRA();
            //部屋点灯チェック
            if(ra == RoomAisle.JOINT)
            {
                bool rFlg;
                bool aFlg;
                
                
                rFlg = cRm.OpenOneRoom(cJoints[num].GetId(RoomAisle.ROOM));
                aFlg = cAm.OpenOneAisle(cJoints[num].GetId(RoomAisle.AISLE));

                if(!rFlg)
                {
                    //敵の生成を依頼する。
                    cGm.RoomOpen(cJoints[num].GetId(RoomAisle.ROOM));
                }
            }
        }



        return posId;
    }

    //Set関数
    /// <summary>位置の更新</summary>
    public Vector3 SetPos(CharacterBace tchara, Point tafter, string tstr)
    {
        Point point = tchara.GetPos();
        cTiles[point.X, point.Y] = " ";
        cTiles[tafter.X, tafter.Y] = tstr;
        return GetTilePos(tafter);
    }


    //Get関数
    /// <summary>ランダムな部屋のランダムな座標を返す</summary>
    /*
    public Point GetRamdomPos()
    {
        
        Point pos;

        pos = cRm.GetRandomRoom();

        return pos;
        
    }*/
    //座標の位置を返す
    public Vector3 GetTilePos(Point tpos)
    {
        return cTileObjects[tpos.X, tpos.Y].transform.position;
    }
    /// <summary>プレイヤの初期値</summary>
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


    /// <summary>スタート関数</summary>
    public void Start()
    {
        Debug.Log($"{this.name}スタート");

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
                    print($"廊下番号{mNum2}を表示します");
                    cAm.OpenOneAisle(mNum2);
                    mNum2++;
                }
                else
                {
                    print($"表示できる廊下は{mNum2}で終わりです");
                }

            }

            if (Input.GetKeyDown(KeyCode.Space) ) 
            {
                if (mNum < cRm.GetRoomCount())
                {
                    print($"部屋番号{mNum}を表示します");
                    cRm.OpenOneRoom(mNum); 
                    
                    mNum++;
                }
                else
                {
                    print($"表示できる部屋は{mNum}で終わりです");
                }

            }

            if(Input.GetKeyDown(KeyCode.O))
            {
                print("マップ全体を表示");
                TileOpen();
            }

            if(Input.GetKeyDown(KeyCode.B))
            {
                
            }

        }//デバック終わり
    }

}
