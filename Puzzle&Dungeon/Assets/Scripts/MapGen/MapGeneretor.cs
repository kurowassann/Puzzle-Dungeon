using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using Unity.Mathematics;
using Common;
using TMPro;
using UnityEngine.UIElements;

/// <summary>マップの生成のみを行うクラス</summary>
public class MapGeneretor : MonoBehaviour
{
    //SerializeField
    [Tooltip("デバック表示"), SerializeField]
    private bool isDebug;
    [Tooltip("自己生成フラグ"), SerializeField]
    private bool isStart;
    [Tooltip("プレファブ格納用オブジェクト"), SerializeField]
    private Transform tileParent;

    //オブジェクト 定数
    /// <summary>部屋管理クラス</summary>
    private RoomManager cRm;
    /// <summary>廊下管理クラス</summary>
    private AisleManager cAm;
    /// <summary>ヨコマスの長さ</summary>
    private int cWidth;
    /// <summary>タテマスの長さ</summary>
    private int cHeight;
    /// <summary>ステージごとの最小分割線</summary>
    private int cDiv;
    /// <summary>マス目の状態管理</summary>
    private string[,] cStrings;
    /// <summary>タイルオブジェクト格納用空のオブジェクト</summary>
    private Transform cTileParent;
    /// <summary>エリアオブジェクト</summary>
    private Area cArea;
    /// <summary>タイル子オブジェクト</summary>
    private GameObject[,] cTiles;


    //メンバ変数


    //メンバ関数
    /// private
    /// <summary>フロアの大きさに応じたタイルを敷き詰める</summary>
    private void MapDrawing(float x, float y)
    {

        for (int i = 0; i < cWidth; i++)
        {
            for (int j = 0; j < cHeight; j++)
            {
                GameObject prefab = null;
                if (cStrings[i, j] == "w")
                {
                    prefab = (GameObject)Resources.Load("Prefabs/Square");
                }
                else if (cStrings[i, j] == " ")
                {
                    prefab = (GameObject)Resources.Load("Prefabs/Floor");
                }
                else
                {
                    print("マップ生成エラー");
                    return;
                }
                var clone = Instantiate(prefab, tileParent);
                //clone.transform.SetParent(BaceMap.transform, false);
                clone.transform.localPosition = new Vector3(x + i * 100, y - j * 100, 0);
                //Debug.Log(clone.transform.position);
                cTiles[i, j] = clone;

                cTiles[i, j].SetActive(false);



            }


        }

    }
    //ルームのタイルオブジェクトをまとめる
    private void RoomSum(Room troom)
    {
        List<GameObject> gos = new List<GameObject>();

        for(int i = troom.GetValue(Value.LEFT)-1;i < troom.GetValue(Value.RIGHT)+2;i++)
        {
            for(int j = troom.GetValue(Value.TOP)-1;j < troom.GetValue(Value.BOTTOM)+2;j++)
            {
                gos.Add(cTiles[i,j]);
            }
        }

        troom.SetTiles(gos);
    }
    /// <summary>廊下のタイルオブジェクトをまとめる</summary>
    private void AisleSum(Aisle taisle)
    {

        //廊下の直線数ループ
        for(int i = 0;i < taisle.GetLength();i++)
        {
            List<GameObject> gos = new List<GameObject>();
            Lurd lurd = taisle.GetLurd(i);
            lurd = LineCorrection(lurd);

            for (int j = lurd.GetValue(Value.LEFT), k = lurd.GetValue(Value.TOP); j != lurd.GetValue(Value.RIGHT) || k != lurd.GetValue(Value.BOTTOM); j++, k++)
            {
                //tiles[i, j].SetActive(false);
                gos.Add(cTiles[j,k]);
                if (j >= lurd.GetValue(Value.RIGHT)) j--;
                if (k >= lurd.GetValue(Value.BOTTOM)) k--;
            }

            taisle.SetTiles(gos);

        }

    }
    /// <summary>直線描画補正</summary>
    private　Lurd LineCorrection(Lurd tlurd)
    {
        Lurd lurd = tlurd;

        //直線の方向によって値を修正
        if (lurd.GetValue(Value.RIGHT) < lurd.GetValue(Value.LEFT))
        {
            int tmp = lurd.GetValue(Value.RIGHT);
            lurd.SetValue(Value.RIGHT, lurd.GetValue(Value.LEFT));
            lurd.SetValue(Value.LEFT, tmp);
        }
        if (lurd.GetValue(Value.BOTTOM) < lurd.GetValue(Value.TOP))
        {
            int tmp = lurd.GetValue(Value.BOTTOM);
            lurd.SetValue(Value.BOTTOM, lurd.GetValue(Value.TOP));
            lurd.SetValue(Value.TOP, tmp);
        }

        if (lurd.GetValue(Value.LEFT) == lurd.GetValue(Value.RIGHT))
        {
            lurd.SetValue(Value.BOTTOM, lurd.GetValue(Value.BOTTOM) + 1);
        }
        if (lurd.GetValue(Value.TOP) == lurd.GetValue(Value.BOTTOM))
        {
            lurd.SetValue(Value.RIGHT, lurd.GetValue(Value.RIGHT) + 1);
        }


        return lurd;
    }


    //public
    /// <summary>フロア生成</summary>
	public void Init(int twidth, int theight, int tdiv)
    {
        cRm = new RoomManager();
        cAm = new AisleManager();

        if (isDebug)//開始コメント
        {
            Debug.Log($"{this.name}:フロア生成開始");
        }


        //生成サイズの確定
        cWidth = twidth;
        cHeight = theight;
        cDiv = tdiv;
        if (isDebug)//格納データ確認
        {
            Debug.Log($"タイル数：{cWidth},{cHeight} 分割幅：{cDiv}");
        }

        //プレファブ格納用オブジェクトを取得
        cTileParent = tileParent;

        cTiles = new GameObject[cWidth, cHeight];
        cStrings = new string[cWidth, cHeight]; //1フロアの大きさをマス目に分割
        //いったんすべて壁で埋める
        for (int i = 0; i < cWidth; i++)
        {
            for (int j = 0; j < cHeight; j++)
            {
                cStrings[i, j] = "w";
            }
        }

        //エリアの分割
        cArea = new Area(new AreaDiv(new Lurd(0, 0, cWidth - 1, cHeight - 1)), this);
        bool flg = cArea.CheckDiv();//自己分裂


        //タイル生成
        int x = (int)transform.position.x; //+ Common.Common.FLOOR_WIDTH;
        int y = (int)transform.position.y; //- Common.Common.FLOOR_HEIGHT;
        MapDrawing(x, y);


        //ルームにタイルを紐づけ
        for(int i = 0;i < cRm.GetRoomCount();i++)
        {
            RoomSum(cRm.GetRoom(i));
        }

        //廊下にタイルを紐づけ
        for(int i = 0; i<cAm.GetAisleCount();i++)
        {
            AisleSum(cAm.GetAisle(i));
        }

        if (isDebug)//生成の終了
        {
            Debug.Log($"{this.name}:フロア生成終了");
        }

    }
    /// <summary>部屋追加処理</summary>
    public int InsertRoom(Lurd troom)
    {
        DrawArea(troom);

        int Id = cRm.AddRoom(troom);
        return Id;
    }
    /// <summary>通路の追加</summary>
    public void InsertAisle(Lurd[] tlurds, Point[] tpoints, int[] tids)
    {
        cAm.AddAisle(tlurds, tpoints, tids);

        for(int i = 0;i<tlurds.Length;i++)
        {

            Draw(tlurds[i]);
        }
    }
    /// <summary>スタートからゴールまでの直線描画(デバック)</summary>
    public void Draw(Lurd stgo)
    {
        //直線の方向によって値を修正
        stgo = LineCorrection(stgo);
        //Debug.Log($"{stgo.left},{stgo.right}");

        for (int i = stgo.GetValue(Value.LEFT), j = stgo.GetValue(Value.TOP); i != stgo.GetValue(Value.RIGHT) || j != stgo.GetValue(Value.BOTTOM); i++, j++)
        {
            //tiles[i, j].SetActive(false);
            cStrings[i, j] = " ";
            if (i >= stgo.GetValue(Value.RIGHT)) i--;
            if (j >= stgo.GetValue(Value.BOTTOM)) j--;
        }
    }
    /// <summary>部屋の四角形描画(デバッグ)</summary>
    public void DrawArea(Lurd tlurd)
    {
        for (int i = tlurd.GetValue(Value.LEFT); i <= tlurd.GetValue(Value.RIGHT); i++)
        {
            for (int j = tlurd.GetValue(Value.TOP); j <= tlurd.GetValue(Value.BOTTOM); j++)
            {
                //tiles[i, j].SetActive(false);
                cStrings[i, j] = " ";
            }
        }
    }
    /// <summary>すべてのタイルを表示</summary>
    public void TileOpen()
    {
        for (int i = 0; i < cWidth; i++)
        {
            for (int j = 0; j < cHeight; j++)
            {
                cTiles[i, j].SetActive(true);
            }
        }
    }


    //Set関数


    //Get関数
    /// <summary>最低分割幅を返す</summary>
    public int GetDiv()
    {
        return cDiv;
    }
    /// <summary>リクエストされたタイルの座標を返す</summary>
    public Vector3 GetTilePos(Point tpoint)
    {
        return cTiles[tpoint.X, tpoint.Y].transform.position;
    }
    /// <summary>部屋のまとまりを渡す</summary>
    public RoomManager GetRoomManager()
    {
        return cRm;
    }
    //廊下のまとまりを渡す
    public AisleManager GetAisleManager()
    {
        return cAm;
    }
    /// <summary>タイルの情報を渡す</summary>
    public String[,] GetStrings()
    {
        return cStrings;
    }
    /// <summary>タイルのオブジェクトを渡す</summary>
    /// <returns></returns>
    public GameObject[,] GetTileObjects()
    {
        return cTiles;
    }
    /// <summary>デバック表示のありなし</summary>
    public bool GetDebug()
    {
        return isDebug;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isStart)
        {
            //Init();
        }
    }

    //エリア分割用クラス
    private class Area
    {
        //オブジェクト
        /// <summary>マップジェネレーター</summary>
        private MapGeneretor cMg;
        /// <summary>自身から分割したエリア</summary>
        private Area[] cChildArea;
        /// <summary>部屋の情報</summary>
        private Lurd cRoom;
        private int cRoomId;


        //メンバ変数
        /// <summary>横幅</summary>
        private int mWidth;
        /// <summary>縦幅</summary>
        private int mHeight;
        /// <summary>自身の切られた位置</summary>
        private AreaDiv mAd;
        /// <summary>部屋があるかどうか</summary>
        private bool isRoom;
        /// <summary>分割地点</summary>
        private int mDiv;


        //メンバ関数
        /// <summary>分割が可能な場合分割、無理なら部屋を生成</summary>
        public bool CheckDiv()
        {
            if(cMg.GetDebug()) Debug.Log("分割確認");
            bool w = (mWidth - mDiv <= 0);
            bool h = (mHeight - mDiv <= 0);
            if (w && h)//分割不可
            {
                if(cMg.GetDebug()) Debug.Log("分割不可");
                CreateRoom();
                return false;
            }
            //分割可能
            else if (w && !h)
            {
                //Debug.Log("横分割可能");
                DivAerea(Direction.HOR);
            }
            else if (h && !w)
            {
                //Debug.Log("縦分割可能");
                DivAerea(Direction.VER);
            }
            else
            {
                //Debug.Log("どちらでも分割可能");
                int len = Enum.GetValues(typeof(Direction)).Length;
                DivAerea((Direction)UnityEngine.Random.Range(0, len));
            }

            return true;
        }
        /// <summary> エリアの分割</summary>
        private void DivAerea(Direction dir)
        {
            if(cMg.GetDebug()) Debug.Log("分割開始");
            int div;
            bool[] flg = { false, false };
            Lurd tmp = mAd.GetLurd(ArDi.AREA);
            Lurd[] area = { tmp, tmp, tmp };
            AreaDiv ad = new AreaDiv();
            if (dir == Direction.VER)
            {
                div = UnityEngine.Random.Range(tmp.GetValue(Value.LEFT) + 6, tmp.GetValue(Value.RIGHT) - 6);
                //Debug.Log($"タテ分割点{div}");
                area[0].SetValue(Value.RIGHT, div - 1);
                area[1].SetValue(Value.LEFT, div + 1);
                area[2].SetValue(Value.LEFT, div);
                area[2].SetValue(Value.RIGHT, div);

            }
            else//dir == Direction.HOR
            {
                div = UnityEngine.Random.Range(tmp.GetValue(Value.TOP) + 6, tmp.GetValue(Value.BOTTOM) - 6);
                //Debug.Log($"ヨコ分割点{div}");
                area[0].SetValue(Value.BOTTOM, div - 1);
                area[1].SetValue(Value.TOP, div + 1);
                area[2].SetValue(Value.TOP, div);
                area[2].SetValue(Value.BOTTOM, div);

            }

            ad.SetDir(dir);
            for (int i = 0; i < area.Length - 1; i++)
            {
                ad.Set(area[i], area[2]);
                cChildArea[i] = CreatArea(ad);
                flg[i] = cChildArea[i].CheckDiv();

            }

            Point point = new Point(area[2].GetValue(Value.LEFT), area[2].GetValue(Value.TOP));
            Area[] areas =
            {
                cChildArea[0].GetChildRoom(point, dir)
               ,cChildArea[1].GetChildRoom(point, dir)
            };
            CreateRoad(areas, area[2], dir);


        }
        /// <summary>エリアの生成</summary>
        private Area CreatArea(AreaDiv tad)
        {
            //分割エリアの生成、分割
            return new Area(tad, cMg);
        }
        /// <summary>分割不能なエリアに部屋を作る</summary>
        private void CreateRoom()
        {
            int x, y;
            isRoom = true;
            Lurd tmp = mAd.GetLurd(ArDi.AREA);

            x = UnityEngine.Random.Range(tmp.GetValue(Value.LEFT) + 1, tmp.GetValue(Value.LEFT) + 1 + mWidth / 3);
            y = UnityEngine.Random.Range(tmp.GetValue(Value.TOP) + 1, tmp.GetValue(Value.TOP) + 1 + mHeight / 3);
            cRoom.SetValue(Value.LEFT, x); cRoom.SetValue(Value.TOP, y);
            x = UnityEngine.Random.Range(cRoom.GetValue(Value.LEFT) + mWidth / 2, tmp.GetValue(Value.RIGHT) - 1);
            y = UnityEngine.Random.Range(cRoom.GetValue(Value.TOP) + mHeight / 2, tmp.GetValue(Value.BOTTOM) - 1);
            cRoom.SetValue(Value.RIGHT, x); cRoom.SetValue(Value.BOTTOM, y);

            //CreateRoad(this, mAd.division, mAd.dir)
            cRoomId = cMg.InsertRoom(cRoom);

        }
        /// <summary>分岐点へ向かって道を伸ばす</summary>
        private void CreateRoad(Area[] tarea, Lurd tdiv, Direction tdir)
        {
            Lurd[] road = new Lurd[3];
            //RoomJoint[] roomJoints = new RoomJoint[2];
            Point[] points = new Point[2];

            for (int i = 0; i < tarea.Length; i++)
            {
                if (tdir == Direction.HOR)
                {
                    //分割線がどっち側にあるか判断する必要あり
                    if(tdiv.GetValue(Value.TOP) < tarea[i].cRoom.GetValue(Value.TOP))
                    {
                        road[i].SetValue(Value.TOP, tarea[i].cRoom.GetValue(Value.TOP)-1);
                    }
                    else
                    {
                        road[i].SetValue(Value.TOP, tarea[i].cRoom.GetValue(Value.BOTTOM)+1);
                    }

                    int value = UnityEngine.Random.Range(tarea[i].cRoom.GetValue(Value.LEFT), tarea[i].cRoom.GetValue(Value.RIGHT));
                    road[i].SetValue(Value.LEFT, value);
                    road[i].SetValue(Value.RIGHT, value);
                    road[i].SetValue(Value.BOTTOM, tdiv.GetValue(Value.BOTTOM));

                    points[i] = new Point(road[i].GetValue(Value.LEFT), road[i].GetValue(Value.TOP));
                }
                else
                {
                    //分割線がどっち側にあるか判断する必要あり
                    if (tdiv.GetValue(Value.LEFT) < tarea[i].cRoom.GetValue(Value.LEFT))
                    {
                        road[i].SetValue(Value.LEFT, tarea[i].cRoom.GetValue(Value.LEFT)-1);
                    }
                    else
                    {
                        road[i].SetValue(Value.LEFT, tarea[i].cRoom.GetValue(Value.RIGHT)+1);
                    }

                    int num = UnityEngine.Random.Range(tarea[i].cRoom.GetValue(Value.TOP), tarea[i].cRoom.GetValue(Value.BOTTOM));
                    road[i].SetValue(Value.TOP, num);
                    road[i].SetValue(Value.BOTTOM, num);
                    road[i].SetValue(Value.RIGHT, tdiv.GetValue(Value.RIGHT));

                    points[i] = new Point(road[i].GetValue(Value.LEFT), road[i].GetValue(Value.TOP));
                }
                //cMg.Draw(road[i]);

            }

            //伸ばした道をつなぐ
            road[2].SetValue(Value.LEFT, road[0].GetValue(Value.RIGHT));
            road[2].SetValue(Value.TOP, road[0].GetValue(Value.BOTTOM));
            road[2].SetValue(Value.RIGHT, road[1].GetValue(Value.RIGHT));
            road[2].SetValue(Value.BOTTOM, road[1].GetValue(Value.BOTTOM));

            //1と2を入れ替える
            Lurd tmp = road[2];
            road[2] = road[1];
            road[1] = tmp;

            int[] Ids =
            {
                tarea[0].cRoomId,
                tarea[1].cRoomId,
            };

            cMg.InsertAisle(road, points, Ids);
        }
        /// <summary>最下層の部屋を取得</summary>
        private Area GetChildRoom(Point point, Direction dir)
        {
            Area tArea;

            if (isRoom)
            {
                tArea = this;
            }
            else
            {
                int x0, x1, y0, y1;
                x0 = cChildArea[0].mAd.GetLurd(ArDi.AREA).GetValue(Value.LEFT) - point.X;
                x1 = cChildArea[1].mAd.GetLurd(ArDi.AREA).GetValue(Value.LEFT) - point.X;
                y0 = cChildArea[0].mAd.GetLurd(ArDi.AREA).GetValue(Value.TOP) - point.Y;
                y1 = cChildArea[1].mAd.GetLurd(ArDi.AREA).GetValue(Value.TOP) - point.Y;
                if (x0 * x0 < x1 * x1 && dir == Direction.VER || y0 * y0 < y1 * y1 && dir == Direction.HOR)
                {
                    //Debug.Log("0");
                    tArea = cChildArea[0].GetChildRoom(point, dir);

                }
                else
                {
                    //Debug.Log("1");
                    tArea = cChildArea[1].GetChildRoom(point, dir);
                }
            }

            return tArea;
        }


        //Set関数


        //Get関数


        /// <summary>コンストラクタ</summary>
        public Area(AreaDiv tAd, MapGeneretor tMg)
        {
            mAd = tAd;
            mWidth = mAd.GetLurd(ArDi.AREA).GetValue(Value.RIGHT) - mAd.GetLurd(ArDi.AREA).GetValue(Value.LEFT);
            mHeight = mAd.GetLurd(ArDi.AREA).GetValue(Value.BOTTOM) - mAd.GetLurd(ArDi.AREA).GetValue(Value.TOP);

            cChildArea = new Area[2];
            cMg = tMg;
            isRoom = false;

            mDiv = cMg.GetDiv();
        }

    }

}
