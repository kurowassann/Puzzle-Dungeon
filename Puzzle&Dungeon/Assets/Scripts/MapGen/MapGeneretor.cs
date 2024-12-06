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
    //�\����
    /// <summary>�l���̃}�X�ڂ�������</summary>
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
    /// <summary>1�G���A�ƕ������̎l��</summary>
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

        /// <summary>�R���X�g���N�^</summary>
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
	/// <summary>�c������</summary>
    public enum Direction
    {
        /// <summary>��</summary>
        HOR,
        /// <summary>�c</summary>
        VER
    };

    //�I�u�W�F�N�g
    [Tooltip("1F�̑傫��"), SerializeField]
    private GameObject BaceMap;
    [Tooltip("���R�}�X�̒���"), SerializeField]
    private int width;
    [Tooltip("�^�e�}�X�̒���"), SerializeField]
    private int height;
    /// <summary>�X�e�[�W���Ƃ̍ŏ�������</summary>
    private int div;
    [Tooltip("�}�X�ڂ̏�ԊǗ�"), SerializeField]
    private string[,] strings;
	/// <summary>�^�C���I�u�W�F�N�g�i�[�p��̃I�u�W�F�N�g</summary>
	private Transform tileParent;
    [Tooltip("�G���A�I�u�W�F�N�g"), SerializeField]
    private Area area;
    [Tooltip("�^�C���q�I�u�W�F�N�g"), SerializeField]
    private GameObject[,] tiles;


    //�����o�ϐ�
    [Tooltip("���Ȑ����t���O"), SerializeField]
    private bool isStart;
	/// <summary>�������̃J�E���g</summary>
    private int mRoomNum;


    //�����o�֐�
    /// <summary>�t���A����</summary>
	public string[,] Init( int twidth,int theight, int tdiv)
    {
        //Debug.Log($"{this.name}:�t���A�����J�n");
        //�}�b�v�̑傫�������߂�
        BaceMap = GameObject.Find("MapGen");
//width = (int)BaceMap.GetComponent<RectTransform>().sizeDelta.x / 100;
        //height = (int)BaceMap.GetComponent<RectTransform>().sizeDelta.y / 100;
		
         width = twidth;
		height = theight;
        div = tdiv;
        //BaceMap.GetComponent<RectTransform>().sizeDelta = new Vector2(width * 100, height * 100);
		BaceMap.transform.position = new Vector3(0, 0, BaceMap.transform.position.z);
		
		//�v���t�@�u�i�[�p�I�u�W�F�N�g���擾
		tileParent = transform.Find("Tiles");


        tiles = new GameObject[width, height];
        //Debug.Log($"�^�C�����F{width},{height}");

        strings = new string[width, height]; //1�t���A�̑傫�����}�X�ڂɕ���
        for (int i = 0; i < width; i++)�@//�������񂷂ׂĕǂŖ��߂�
        {
            for (int j = 0; j < height; j++)
            {
                strings[i, j] = "w";
            }
        }

        mRoomNum = 0;
        //�G���A�̕���
        allRooms = new Lurd[mRoomNum];
        area = new Area(new AreaDiv(new Lurd(0, 0, width - 1, height - 1)), this);
        bool flg = area.CheckDiv();//���ȕ���
        //Debug.Log(flg);

		Debug.Log($"{this.name}:�t���A�����I��");

		//�^�C������
		int x = (int)BaceMap.transform.position.x; //+ Common.Common.FLOOR_WIDTH;
		int y = (int)BaceMap.transform.position.y; //- Common.Common.FLOOR_HEIGHT;
		MapDrawing(x, y);


		return strings;
    }        
    /// <summary>�����_���ȕ����̃����_���ȏꏊ��Ԃ�</summary>
    public Point GetRandomRoom()
    {
        Lurd lurd = allRooms[UnityEngine.Random.Range(0, allRooms.Length)];
        Point point = new Point();
        point.X = UnityEngine.Random.Range(lurd.left, lurd.right);
        point.Y = UnityEngine.Random.Range(lurd.up, lurd.down);
        return point;
    }
    /// <summary>�����ǉ�����</summary>
    public void InsertRoom(Lurd troom)
    {
        //Debug.Log($"�����ǉ��@������:{roomNum+1}");
        mRoomNum++;
        Array.Resize(ref allRooms, mRoomNum);
        allRooms[mRoomNum - 1] = troom;
    }  
    /// <summary>�X�^�[�g����S�[���܂ł̒����`��(�f�o�b�N)</summary>
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
    /// <summary>�����̎l�p�`�`��(�f�o�b�O)</summary>
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
    /// <summary>�t���A�̑傫���ɉ������^�C����~���l�߂�</summary>
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
					print("�}�b�v�����G���[");
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


    //Set�֐�


    //Get�֐�
    /// <summary>�Œᕪ������Ԃ�</summary>
    public int GetDiv()
    {
        return div;
    }
    /// <summary>���N�G�X�g���ꂽ�^�C���̍��W��Ԃ�</summary>
    public Vector3 GetTilePos(Point tpoint)
    {
        return tiles[tpoint.X,tpoint.Y].transform.position;
    }


    [Tooltip("�����Ǘ��p�z��"), SerializeField]
    private Lurd[] allRooms;


    // Start is called before the first frame update
    void Start()
    {
        if(isStart)
        { 
            //Init();
        }
    }

    //�G���A�����p�N���X
    private�@class Area
    {
        //�I�u�W�F�N�g
        /// <summary>�}�b�v�W�F�l���[�^�[</summary>
        private MapGeneretor mg;
        /// <summary>���g���番�������G���A�̏��</summary>
        private Area[] childArea;
        /// <summary>�����̏��</summary>
        private Lurd room;


        //�����o�ϐ�
        /// <summary>����</summary>
        private int mWidth;
        /// <summary>�c��</summary>
        private int mHeight;
        /// <summary>���g�̕��������</summary>
        private AreaDiv mAd;
        /// <summary>���������邩�ǂ���</summary>
        private bool isRoom;
        /// <summary>�����n�_</summary>
        private int mDiv; 


        //�����o�֐�
        /// <summary>�������\���m�F,�^�e�����R�����߂�</summary>
        public bool CheckDiv()
        {
            //Debug.Log("�����m�F");
            bool w = (mWidth - mDiv <= 0);
            bool h = (mHeight - mDiv<= 0);
            if (w && h)//�����s��
            {
                //ebug.Log("�����s��");
                CreateRoom();
                return false; 
            }
            //�����\
            else if (w && !h)
            {
                //Debug.Log("�������\");
                DivAerea(Direction.HOR); 
            }
            else if (h && !w)
            {
                //Debug.Log("�c�����\");
                DivAerea(Direction.VER);
            }
            else
            {
                //Debug.Log("�ǂ���ł������\");
                int len = Enum.GetValues(typeof(Direction)).Length;
                DivAerea((Direction)UnityEngine.Random.Range(0, len));
            }

            return true;
        }
        /// <summary> �G���A�̕���</summary>
        private void DivAerea(Direction dir)
        {
            //Debug.Log("�����J�n");
            int div;
            bool[] flg = {false,false };
            Lurd[] area = { mAd.area, mAd.area , mAd.area};
            AreaDiv ad= new AreaDiv();
            if(dir == Direction.VER)
            {
                div = UnityEngine.Random.Range(mAd.area.left + 6, mAd.area.right - 6);
                //Debug.Log($"�^�e�����_{div}");
                area[0].right = div-1;
                area[1].left = div+1;
                area[2].left = area[2].right = div;

            }
            else//dir == Direction.HOR
            {
                div = UnityEngine.Random.Range(mAd.area.up+ 6, mAd.area.down- 6);
                //Debug.Log($"���R�����_{div}");
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
        /// <summary>�G���A�̐���</summary>
        private Area CreatArea(AreaDiv tad)
        {
            //�����G���A�̐����A����
            return new Area(tad,mg);
        }
        /// <summary>�����s�\�ȃG���A�ɕ��������</summary>
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
            //Debug.Log($"����:{room.left},{room.right}");
            //Debug.Log($"�c��:{room.up},{room.down}");
            //Debug.Log($"�������F{length}");
            mg.DrawArea(room);

            //CreateRoad(this, mAd.division, mAd.dir)
            mg.InsertRoom(room);
        }
        /// <summary>����_�֌������ē���L�΂�</summary>
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

            //�L�΂��������Ȃ�
            road[2].left = road[0].right;
            road[2].up = road[0].down;
            road[2].right = road[1].right;
            road[2].down = road[1].down;

            mg.Draw(road[2]);
        }
        /// <summary>�ŉ��w�̕������擾</summary>
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


        //Set�֐�


        //Get�֐�


        /// <summary>�R���X�g���N�^</summary>
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
