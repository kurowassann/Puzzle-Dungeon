using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Common
{
    //4����
    public enum Value
    {
        LEFT, RIGHT, TOP, BOTTOM
    }

    /// <summary>�l���̃}�X�ڂ�������</summary>
    public struct Lurd
    {
        private int left;
        private int top;
        private int right;
        private int bottom;

        public Lurd Set(int tl, int tt, int tr, int tb)
        {
            left = tl;
            top = tt;
            right = tr;
            bottom = tb;
            return this;
        }

        public void SetValue(Value tvalue,int tnum)
        {

            switch (tvalue)
            {
                case Value.LEFT:
                    left = tnum;
                    break;
                case Value.RIGHT:
                    right = tnum;
                    break;
                case Value.TOP:
                    top = tnum;
                    break;
                case Value.BOTTOM:
                    bottom = tnum;
                    break;
                default:
                    break;
            }
        }


        public int GetValue(Value tvalue)
        {
            int val;
            switch (tvalue)
            {
                case Value.LEFT:
                    val = left;
                    break;
                case Value.RIGHT:
                    val = right;
                    break;
                case Value.TOP:
                    val = top;
                    break;
                case Value.BOTTOM:
                    val = bottom;
                    break;
                    default:
                    val = 0;
                    break;
            }
            return val;
        }

        public Lurd(int tl, int tt, int tr, int tb)
        {
            this.left = tl;
            this.top = tt;
            this.right = tr;
            this.bottom = tb;
        }
    }

    //�G���A�������
    public enum ArDi
    {
        AREA,
        DIVIDION,
    }

    /// <summary>1�G���A�ƕ������̎l��</summary>
    public struct AreaDiv
    {
        private Lurd area;
        private Lurd division;
        private Direction dir;

        public void Set(Lurd tArea, Lurd tDiv)
        {
            area = tArea;
            division = tDiv;
        }

        public void SetLurd(ArDi val,Lurd tlurd)
        {
            switch (val)
            {
                case ArDi.AREA:
                   area = tlurd;
                    break;
                case ArDi.DIVIDION:
                    division = tlurd;
                    break;
                default:
                    break;
            }
        }

        public void SetDir(Direction tdir)
        {
            dir = tdir;
        }


        public Lurd GetLurd(ArDi val)
        {
            Lurd lurd;
            switch (val)
            {
                case ArDi.AREA:
                    lurd = area;
                    break;
                case ArDi.DIVIDION:
                    lurd = division;
                    break;
                default:
                    lurd = area;
                    break;
            }
            return lurd;
        }

        /// <summary>�R���X�g���N�^</summary>
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

    /// <summary>�c������</summary>
    public enum Direction
    {
        /// <summary>��</summary>
        HOR,
        /// <summary>�c</summary>
        VER
    };


    public enum Abc
    {
        PLAYER_HP,
        PLAYER_MP,
    }

	//�ړ�����
    public enum Vector
    {
    	LEFT,
        RIGHT,
        UP,
        DOWN,
        NONE
    };
    
   	//�^�C���̏��
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
    
    	/// <summary>�L�����N�^�[�̏�Ԃ�\��</summary>
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
        /// <summary>�X�e�[�W�̑���</summary>
        public const int STAGE_NUM = 4;
        /// <summary>�X�e�[�W�̑���</summary>
        public const int ENEMY_NUM = 4;

        /// <summary>�ėp�G���[ID</summary>
        public const int ERROR_ID = -1;




        /// <summary>�ݒ�̃L�[</summary>
        public const string KEY_SETTINGS = "settings";
        /// <summary>�X�e�[�W�I���őI�����ꂽ�X�e�[�WID</summary>
        public const string KEY_SELECTED_STAGE_ID = "stage_id";

        /// <summary>�t���A�̍Œቡ��</summary>
        public const int FLOOR_WIDTH = 20;
        /// <summary>�t���A�̍Œ�c��</summary>
        public const int FLOOR_HEIGHT = 10;
        /// <summary>�Œᕪ����</summary>
        public const int DIVIISION = 10;

        /// <summary>�ړ���̎��̓��͂܂ł̃f�B���C</summary>
        public const float REAR_GAP_TIME = 0.1f;
        /// <summary>���[�V�����ɂ����鎞��</summary>
        public const float MOTION_SPEED = 0.1F;

		/// <summary>�G�̏����o����</summary>
		public const int ENEMY_COUNT = 3;

		public const int PULAYER_HP = 2;
		public const int ENEMY_HP = 1;

        public static  Point SetPoint(int x,int y)
        {
            return new Point(x,y);
        }

        /// <summary>
        /// �v���C���[�̃A�j���[�V�����̕����萔
        /// <see cref="Vector"/>�ƃC���f�b�N�X��Ή������Ă�
        /// </summary>
        public static string[] CHARA_ANIMS_MOVE_DIR =
        {
            "MoveLeft",
            "MoveRight",
            "MoveUp",
            "MoveDown",
        };

        /// <summary>
        /// �ړ��I�����̃v���C���[�A�j���[�V����
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


        // �V�[����
        public const string SCENE_TITLE = "Title";
        public const string SCENE_SELECT = "StageSelect";
        public const string SCENE_GAME = "Game";

        //public static 

        /// <summary>
        /// �V�[���؂�ւ��p�֐�
        /// </summary>
        /// <param name="_sceneName">�ǂݍ��ރV�[����</param>
        public static void LoadScene(string _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
        }

        /// <summary>
        /// �Q�[���I�����̏���
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
        /// �񋓑̗̂v�f�����J���^���Ɏ擾����֐�
        /// </summary>
        /// <typeparam name="T">�񋓑̂̌^</typeparam>
        /// <returns>�񋓑̂�</returns>
        public static int GetEnumLength<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Length;
        }


        /// <summary>
        /// 0�`1�ɐ��K�����ꂽ�l(_val)��_min�`_max�ɐ��K�����ĕԂ��֐�
        /// </summary>
        /// <param name="_val">0�`1�ɐ��K�����ꂽ�l</param>
        /// <param name="_min">���K������l�̍ŏ��l</param>
        /// <param name="_max">���K������l�̍ő�l</param>
        /// <returns></returns>
        public static float NormalizedFunc(float _val, float _min, float _max)
        {
            return _val * (_max - _min) + _min;
        }

        /// <summary>
        /// _min1�`_max1�ɐ��K�����ꂽ�l(_val)��_min2�`_max2�ɐ��K�����ĕԂ��֐�
        /// </summary>
        /// <param name="_val">���K���������l</param>
        /// <param name="_min1">_val���Ƃ�l�͈̔͂̍ŏ��l</param>
        /// <param name="_max1">_val���Ƃ�l�͈̔͂̍ő�l</param>
        /// <param name="_min2">���K������l�̍ŏ��l</param>
        /// <param name="_max2">���K������l�̍ő�l</param>
        /// <returns></returns>
        public static float NormalizedFunc(float _val, float _min1, float _max1, float _min2, float _max2)
        {
            // _val��0�`1�Ő��K��
            _val = (_val - _min1) / (_max1 - _min1);
            //_min2�`_max2�Ő��K�����ꂽ�l��Ԃ�
            return NormalizedFunc(_val, _min2, _max2);
        }
    }
    


}