using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using Unity.Mathematics;

public class MapGeneretor : MonoBehaviour
{   
    //構造体
    /// <summary>四隅のマス目を持つもの</summary>
    public struct Lurd
    {
        public int left;
        public int up;
        public int right;
        public int down;

        public Lurd Set(int tl,int tu, int tr, int td)
        {
            left = tl;
            up = tu;
            right = tr;
            down = td;
            return this;
        }

        public Lurd(int tl, int tu, int tr, int td)
        {
            this.left = tl;
            this.up = tu;
            this.right = tr;
            this.down = td;
        }
    }
    /// <summary>1エリアと分割線の四隅</summary>
    public struct AreaDiv
    {
        public Lurd area;
        public Lurd division;
        public Direction dir;

        public void Set(Lurd tArea, Lurd tDiv)
        {
            area = tArea;
            division = tDiv;
        }

        /// <summary>コンストラクタ</summary>
        /// <param name="tArea"></param>
        /// <param name="tDiv"></param>
        /// <param name="tDir"></param>
        public  AreaDiv(Lurd tArea, Lurd tDiv, Direction tDir)
        {
            area = tArea;
            division = tDiv;
            dir = tDir;
        }
        public AreaDiv(Lurd tArea)
        {
            area = tArea;
            division = new Lurd();
            dir = new Direction();
        }
    }
	/// <summary>縦横方向</summary>
    public enum Direction
    {
        /// <summary>横</summary>
        HOR,
        /// <summary>縦</summary>
        VER
    };

    //オブジェクト
    [Tooltip("1Fの大きさ"), SerializeField]
    private GameObject BaceMap;
    [Tooltip("ヨコマスの長さ"), SerializeField]
    private int width;
    [Tooltip("タテマスの長さ"), SerializeField]
    private int height;
    /// <summary>ステージごとの最小分割線</summary>
    private int div;
    [Tooltip("マス目の状態管理"), SerializeField]
    private string[,] strings;
	/// <summary>タイルオブジェクト格納用空のオブジェクト</summary>
	private Transform tileParent;
    [Tooltip("エリアオブジェクト"), SerializeField]
    private Area area;
    [Tooltip("タイル子オブジェクト"), SerializeField]
    private GameObject[,] tiles;


    //メンバ変数
    [Tooltip("自己生成フラグ"), SerializeField]
    private bool isStart;
	/// <summary>部屋数のカウント</summary>
    private int mRoomNum;


    //メンバ関数
    /// <summary>フロア生成</summary>
	public string[,] Init( int twidth,int theight, int tdiv)
    {
        //Debug.Log($"{this.name}:フロア生成開始");
        //マップの大きさを決める
        BaceMap = GameObject.Find("MapGen");
//width = (int)BaceMap.GetComponent<RectTransform>().sizeDelta.x / 100;
        //height = (int)BaceMap.GetComponent<RectTransform>().sizeDelta.y / 100;
		
         width = twidth;
		height = theight;
        div = tdiv;
        //BaceMap.GetComponent<RectTransform>().sizeDelta = new Vector2(width * 100, height * 100);
		BaceMap.transform.position = new Vector3(0, 0, BaceMap.transform.position.z);
		
		//プレファブ格納用オブジェクトを取得
		tileParent = transform.Find("Tiles");


        tiles = new GameObject[width, height];
        //Debug.Log($"タイル数：{width},{height}");

        strings = new string[width, height]; //1フロアの大きさをマス目に分割
        for (int i = 0; i < width; i++)　//いったんすべて壁で埋める
        {
            for (int j = 0; j < height; j++)
            {
                strings[i, j] = "w";
            }
        }

        mRoomNum = 0;
        //エリアの分割
        allRooms = new Lurd[mRoomNum];
        area = new Area(new AreaDiv(new Lurd(0, 0, width - 1, height - 1)), this);
        bool flg = area.CheckDiv();//自己分裂
        //Debug.Log(flg);

		Debug.Log($"{this.name}:フロア生成終了");

		//タイル生成
		int x = (int)BaceMap.transform.position.x; //+ Common.Common.FLOOR_WIDTH;
		int y = (int)BaceMap.transform.position.y; //- Common.Common.FLOOR_HEIGHT;
		MapDrawing(x, y);


		return strings;
    }        
    /// <summary>ランダムな部屋のランダムな場所を返す</summary>
    public Point GetRandomRoom()
    {
        Lurd lurd = allRooms[UnityEngine.Random.Range(0, allRooms.Length)];
        Point point = new Point();
        point.X = UnityEngine.Random.Range(lurd.left, lurd.right);
        point.Y = UnityEngine.Random.Range(lurd.up, lurd.down);
        return point;
    }
    /// <summary>部屋追加処理</summary>
    public void InsertRoom(Lurd troom)
    {
        //Debug.Log($"部屋追加　部屋個数:{roomNum+1}");
        mRoomNum++;
        Array.Resize(ref allRooms, mRoomNum);
        allRooms[mRoomNum - 1] = troom;
    }  
    /// <summary>スタートからゴールまでの直線描画(デバック)</summary>
    public void Draw(Lurd stgo)
    {
		
        if(stgo.right < stgo.left)
        {
            int tmp = stgo.right;
            stgo.right = stgo.left;
            stgo.left = tmp;
        }
        if(stgo.down < stgo.up)
        {
            int tmp = stgo.down;
            stgo.down = stgo.up;
            stgo.up= tmp;
        }

        if( stgo.left == stgo.right)
        {
            stgo.down++;
        }
        if(stgo.up == stgo.down) 
        {
            stgo.right++;
        }

        //Debug.Log($"{stgo.left},{stgo.right}");

        for (int i = stgo.left, j = stgo.up; i != stgo.right || j != stgo.down;i++,j++)
        {
            //tiles[i, j].SetActive(false);
            strings[i, j] = " ";
            if (i >= stgo.right) i--;
            if (j >= stgo.down) j--;
        }
    }
    /// <summary>部屋の四角形描画(デバッグ)</summary>
    public void DrawArea(Lurd tlurd)
    {
        for (int i = tlurd.left; i <= tlurd.right; i++) 
        {
            for(int j = tlurd.up; j <= tlurd.down; j++)
            {
                //tiles[i, j].SetActive(false);
                strings[i, j] = " ";
            }
        }
    }
    /// <summary>フロアの大きさに応じたタイルを敷き詰める</summary>
    private void  MapDrawing(float x, float y)
    {

		for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
				GameObject prefab = null;
				if (strings[i, j] == "w")
				{
					prefab = (GameObject)Resources.Load("Prefabs/Square");
				}
				else if (strings[i, j] == " ")
				{
					prefab = (GameObject)Resources.Load("Prefabs/Floor");
				}
				else
				{
					print("マップ生成エラー");
					return;
				}
                var clone = Instantiate(prefab,tileParent);
                //clone.transform.SetParent(BaceMap.transform, false);
                clone.transform.localPosition = new Vector3(x + i * 100, y - j * 100, 0);
				//Debug.Log(clone.transform.position);
                tiles[i,j]  = clone;



			}


		}

    }


    //Set関数


    //Get関数
    /// <summary>最低分割幅を返す</summary>
    public int GetDiv()
    {
        return div;
    }
    /// <summary>リクエストされたタイルの座標を返す</summary>
    public Vector3 GetTilePos(Point tpoint)
    {
        return tiles[tpoint.X,tpoint.Y].transform.position;
    }


    [Tooltip("部屋管理用配列"), SerializeField]
    private Lurd[] allRooms;


    // Start is called before the first frame update
    void Start()
    {
        if(isStart)
        { 
            //Init();
        }
    }

    //エリア分割用クラス
    private　class Area
    {
        //オブジェクト
        /// <summary>マップジェネレーター</summary>
        private MapGeneretor mg;
        /// <summary>自身から分割したエリアの情報</summary>
        private Area[] childArea;
        /// <summary>部屋の情報</summary>
        private Lurd room;


        //メンバ変数
        /// <summary>横幅</summary>
        private int mWidth;
        /// <summary>縦幅</summary>
        private int mHeight;
        /// <summary>自身の分割線情報</summary>
        private AreaDiv mAd;
        /// <summary>部屋があるかどうか</summary>
        private bool isRoom;
        /// <summary>分割地点</summary>
        private int mDiv; 


        //メンバ関数
        /// <summary>分割が可能か確認,タテかヨコか決める</summary>
        public bool CheckDiv()
        {
            //Debug.Log("分割確認");
            bool w = (mWidth - mDiv <= 0);
            bool h = (mHeight - mDiv<= 0);
            if (w && h)//分割不可
            {
                //ebug.Log("分割不可");
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
            //Debug.Log("分割開始");
            int div;
            bool[] flg = {false,false };
            Lurd[] area = { mAd.area, mAd.area , mAd.area};
            AreaDiv ad= new AreaDiv();
            if(dir == Direction.VER)
            {
                div = UnityEngine.Random.Range(mAd.area.left + 6, mAd.area.right - 6);
                //Debug.Log($"タテ分割点{div}");
                area[0].right = div-1;
                area[1].left = div+1;
                area[2].left = area[2].right = div;

            }
            else//dir == Direction.HOR
            {
                div = UnityEngine.Random.Range(mAd.area.up+ 6, mAd.area.down- 6);
                //Debug.Log($"ヨコ分割点{div}");
                area[0].down = div-1;
                area[1].up= div+1;
                area[2].up = area[2].down = div;

            }

            //mg.Draw(area[2]);

            ad.dir = dir;
            for (int i = 0; i < area.Length-1; i++)
            {
                ad.Set(area[i], area[2]);
                childArea[i] = CreatArea(ad);
                flg[i] = childArea[i].CheckDiv();

            }

            Point point = new Point(area[2].left, area[2].up);
            Area[] areas = 
            {   childArea[0].GetChildRoom(point, dir)
               ,childArea[1].GetChildRoom(point, dir)
            };
            CreateRoad(areas, area[2], dir);


        }
        /// <summary>エリアの生成</summary>
        private Area CreatArea(AreaDiv tad)
        {
            //分割エリアの生成、分割
            return new Area(tad,mg);
        }
        /// <summary>分割不能なエリアに部屋を作る</summary>
        private void CreateRoom()
        {
            int x, y;
            isRoom = true; 

            x = UnityEngine.Random.Range(mAd.area.left + 1, mAd.area.left+ 1 + mWidth / 3);
            y = UnityEngine.Random.Range(mAd.area.up + 1, mAd.area.up + 1 + mHeight / 3);
            room.left = x; room.up = y;
            x = UnityEngine.Random.Range(room.left+mWidth/2, mAd.area.right-1);
            y= UnityEngine.Random.Range(room.up+mHeight/2, mAd.area.down-1);
            room.right = x; room.down = y;
            Point length = new Point(room.right-room.left,room.down-room.up);
            //Debug.Log($"横幅:{room.left},{room.right}");
            //Debug.Log($"縦幅:{room.up},{room.down}");
            //Debug.Log($"部屋幅：{length}");
            mg.DrawArea(room);

            //CreateRoad(this, mAd.division, mAd.dir)
            mg.InsertRoom(room);
        }
        /// <summary>分岐点へ向かって道を伸ばす</summary>
        private void CreateRoad(Area[] tarea, Lurd tdiv, Direction tdir)
        {
            Lurd[] road = new Lurd[3];

            for(int i = 0; i < tarea.Length; i++)
            {
                if(tdir == Direction.HOR)
                {
                    road[i].left = road[i].right = UnityEngine.Random.Range(tarea[i].room.left, tarea[i].room.right);
                    road[i].up = tarea[i].room.up;
                    road[i].down = tdiv.down;
                }
                else 
                {
                    road[i].up = road[i].down  = UnityEngine.Random.Range(tarea[i].room.up, tarea[i].room.down);
                    road[i].left = tarea[i].room.left;
                    road[i].right = tdiv.right;
                }
            mg.Draw(road[i]);

            }

            //伸ばした道をつなぐ
            road[2].left = road[0].right;
            road[2].up = road[0].down;
            road[2].right = road[1].right;
            road[2].down = road[1].down;

            mg.Draw(road[2]);
        }
        /// <summary>最下層の部屋を取得</summary>
        private Area GetChildRoom(Point point, Direction dir)
        {
            Area tArea;
            
            if(isRoom)
            {
                tArea = this;
            }
            else
            {
                int x0, x1, y0, y1;
                x0 = childArea[0].mAd.area.left - point.X;
                x1 = childArea[1].mAd.area.left - point.X;
                y0 = childArea[0].mAd.area.up - point.Y;
                y1 = childArea[1].mAd.area.up - point.Y;
                if (x0*x0 < x1*x1  && dir== Direction.VER  || y0*y0 < y1*y1 && dir == Direction.HOR)
                {
                    //Debug.Log("0");
                    tArea = childArea[0].GetChildRoom(point ,dir);

                }
                else
                {
                    //Debug.Log("1");
                    tArea = childArea[1].GetChildRoom(point, dir);
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
            mWidth = mAd.area.right-mAd.area.left;
            mHeight = mAd.area.down-mAd.area.up;

            childArea = new Area[2];
            mg = tMg;
            isRoom = false;

            mDiv = mg.GetDiv();
        }

    }
    
}
