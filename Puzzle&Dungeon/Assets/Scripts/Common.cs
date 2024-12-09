using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Common
{
    /// <summary>四隅のマス目を持つもの</summary>
    public struct Lurd
    {
        public int left;
        public int up;
        public int right;
        public int down;

        public Lurd Set(int tl, int tu, int tr, int td)
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
        public AreaDiv(Lurd tArea, Lurd tDiv, Direction tDir)
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


    public enum Abc
    {
        PLAYER_HP,
        PLAYER_MP,
    }

	//移動方向
    public enum Vector
    {
    	LEFT,
        RIGHT,
        UP,
        DOWN,
        NONE
    };
    
   	//タイルの状態
    public enum TileInfo
    {
        ROUTE,
        WALL,
        PLAYER,
        ENEMY,
        NONE
    }

    public enum LogType
    {
        NONE,
        ATTACK,
        DEAD,
    }
    
    	/// <summary>キャラクターの状態を表す</summary>
	public enum Status
	{
		STAY,
		MOVE,
        TRUN,
		ATTACK,
		REAR_GAP
	}
    
    
    class Common
    {
        /// <summary>ステージの総数</summary>
        public const int STAGE_NUM = 4;
        /// <summary>ステージの総数</summary>
        public const int ENEMY_NUM = 4;

        /// <summary>汎用エラーID</summary>
        public const int ERROR_ID = -1;




        /// <summary>設定のキー</summary>
        public const string KEY_SETTINGS = "settings";
        /// <summary>ステージ選択で選択されたステージID</summary>
        public const string KEY_SELECTED_STAGE_ID = "stage_id";

        /// <summary>フロアの最低横幅</summary>
        public const int FLOOR_WIDTH = 20;
        /// <summary>フロアの最低縦幅</summary>
        public const int FLOOR_HEIGHT = 10;
        /// <summary>最低分割幅</summary>
        public const int DIVIISION = 10;

        /// <summary>移動後の次の入力までのディレイ</summary>
        public const float REAR_GAP_TIME = 0.1f;
        /// <summary>モーションにかける時間</summary>
        public const float MOTION_SPEED = 0.1F;

		/// <summary>敵の初期出現数</summary>
		public const int ENEMY_COUNT = 3;

		public const int PULAYER_HP = 2;
		public const int ENEMY_HP = 1;

        public static  Point SetPoint(int x,int y)
        {
            return new Point(x,y);
        }

        /// <summary>
        /// プレイヤーのアニメーションの方向定数
        /// <see cref="Vector"/>とインデックスを対応させてる
        /// </summary>
        public static string[] CHARA_ANIMS_MOVE_DIR =
        {
            "MoveLeft",
            "MoveRight",
            "MoveUp",
            "MoveDown",
        };

        /// <summary>
        /// 移動終了時のプレイヤーアニメーション
        /// </summary>
        public static string[] CHARA_ANIMS_END_DIR =
        {
            "DirectionLeft",
            "DirectionRight",
            "DirectionUp",
            "DirectionDown",
        };

        public static string[] CHARA_ANIMS_ATTACK_DIR =
        {
            "AttackLeft",
            "AttackRight",
            "AttackUp",
            "AttackDown",
        };


        // シーン名
        public const string SCENE_TITLE = "Title";
        public const string SCENE_SELECT = "StageSelect";
        public const string SCENE_GAME = "Game";

        //public static 

        /// <summary>
        /// シーン切り替え用関数
        /// </summary>
        /// <param name="_sceneName">読み込むシーン名</param>
        public static void LoadScene(string _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
        }

        /// <summary>
        /// ゲーム終了時の処理
        /// </summary>
        public static void GameQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// 列挙体の要素数をカンタンに取得する関数
        /// </summary>
        /// <typeparam name="T">列挙体の型</typeparam>
        /// <returns>列挙体の</returns>
        public static int GetEnumLength<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Length;
        }


        /// <summary>
        /// 0～1に正規化された値(_val)を_min～_maxに正規化して返す関数
        /// </summary>
        /// <param name="_val">0～1に正規化された値</param>
        /// <param name="_min">正規化する値の最小値</param>
        /// <param name="_max">正規化する値の最大値</param>
        /// <returns></returns>
        public static float NormalizedFunc(float _val, float _min, float _max)
        {
            return _val * (_max - _min) + _min;
        }

        /// <summary>
        /// _min1～_max1に正規化された値(_val)を_min2～_max2に正規化して返す関数
        /// </summary>
        /// <param name="_val">正規化したい値</param>
        /// <param name="_min1">_valがとる値の範囲の最小値</param>
        /// <param name="_max1">_valがとる値の範囲の最大値</param>
        /// <param name="_min2">正規化する値の最小値</param>
        /// <param name="_max2">正規化する値の最大値</param>
        /// <returns></returns>
        public static float NormalizedFunc(float _val, float _min1, float _max1, float _min2, float _max2)
        {
            // _valを0～1で正規化
            _val = (_val - _min1) / (_max1 - _min1);
            //_min2～_max2で正規化された値を返す
            return NormalizedFunc(_val, _min2, _max2);
        }
    }
    


}