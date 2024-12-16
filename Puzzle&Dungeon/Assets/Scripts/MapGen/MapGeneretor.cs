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

/// <summary>�}�b�v�̐����݂̂��s���N���X</summary>
public class MapGeneretor : MonoBehaviour
{
    //SerializeField
    [Tooltip("�f�o�b�N�\��"), SerializeField]
    private bool isDebug;
    [Tooltip("���Ȑ����t���O"), SerializeField]
    private bool isStart;
    [Tooltip("�v���t�@�u�i�[�p�I�u�W�F�N�g"), SerializeField]
    private Transform tileParent;

    //�I�u�W�F�N�g �萔
    /// <summary>�����Ǘ��N���X</summary>
    private RoomManager cRm;
    /// <summary>���R�}�X�̒���</summary>
    private int cWidth;
    /// <summary>�^�e�}�X�̒���</summary>
    private int cHeight;
    /// <summary>�X�e�[�W���Ƃ̍ŏ�������</summary>
    private int cDiv;
    /// <summary>�}�X�ڂ̏�ԊǗ�</summary>
    private string[,] cStrings;
    /// <summary>�^�C���I�u�W�F�N�g�i�[�p��̃I�u�W�F�N�g</summary>
    private Transform cTileParent;
    /// <summary>�G���A�I�u�W�F�N�g</summary>
    private Area cArea;
    /// <summary>�^�C���q�I�u�W�F�N�g</summary>
    private GameObject[,] cTiles;


    //�����o�ϐ�


    //�����o�֐�
    /// private
    /// <summary>�t���A�̑傫���ɉ������^�C����~���l�߂�</summary>
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
                    print("�}�b�v�����G���[");
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
    //���[���̃^�C���I�u�W�F�N�g���܂Ƃ߂�
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


    //public
    /// <summary>�t���A����</summary>
	public void Init(int twidth, int theight, int tdiv)
    {
        cRm = new RoomManager();

        if (isDebug)//�J�n�R�����g
        {
            Debug.Log($"{this.name}:�t���A�����J�n");
        }


        //�����T�C�Y�̊m��
        cWidth = twidth;
        cHeight = theight;
        cDiv = tdiv;
        if (isDebug)//�i�[�f�[�^�m�F
        {
            Debug.Log($"�^�C�����F{cWidth},{cHeight} �������F{cDiv}");
        }

        //�v���t�@�u�i�[�p�I�u�W�F�N�g���擾
        cTileParent = tileParent;

        cTiles = new GameObject[cWidth, cHeight];
        cStrings = new string[cWidth, cHeight]; //1�t���A�̑傫�����}�X�ڂɕ���
        //�������񂷂ׂĕǂŖ��߂�
        for (int i = 0; i < cWidth; i++)
        {
            for (int j = 0; j < cHeight; j++)
            {
                cStrings[i, j] = "w";
            }
        }

        //�G���A�̕���
        cArea = new Area(new AreaDiv(new Lurd(0, 0, cWidth - 1, cHeight - 1)), this);
        bool flg = cArea.CheckDiv();//���ȕ���


        //�^�C������
        int x = (int)transform.position.x; //+ Common.Common.FLOOR_WIDTH;
        int y = (int)transform.position.y; //- Common.Common.FLOOR_HEIGHT;
        MapDrawing(x, y);


        //���[���Ƀ^�C����R�Â�
        for(int i = 0;i < cRm.GetRoomCount();i++)
        {
            RoomSum(cRm.GetRoom(i));
        }

        if (isDebug)//�����̏I��
        {
            Debug.Log($"{this.name}:�t���A�����I��");
        }

    }
    /// <summary>�����ǉ�����</summary>
    public void InsertRoom(Lurd troom)
    {
        DrawArea(troom);

        //List<GameObject> gms = new List<GameObject>();
        /*
        for (int i = 0; i < troom.GetValue(Value.RIGHT); i++)
        {
            for (int j = 0; j < troom.GetValue(Value.BOTTOM); j++)
            {
                gms.Add(cTiles[i, j]);
            }
        }

        */


        cRm.AddRoom(troom);
    }
    /// <summary>�X�^�[�g����S�[���܂ł̒����`��(�f�o�b�N)</summary>
    public void Draw(Lurd stgo)
    {

        if (stgo.GetValue(Value.RIGHT) < stgo.GetValue(Value.LEFT))
        {
            int tmp = stgo.GetValue(Value.RIGHT);
            stgo.SetValue(Value.RIGHT, stgo.GetValue(Value.LEFT));
            stgo.SetValue(Value.LEFT, tmp);
        }
        if (stgo.GetValue(Value.BOTTOM) < stgo.GetValue(Value.TOP))
        {
            int tmp = stgo.GetValue(Value.BOTTOM);
            stgo.SetValue(Value.BOTTOM, stgo.GetValue(Value.TOP));
            stgo.SetValue(Value.TOP, tmp);
        }

        if (stgo.GetValue(Value.LEFT) == stgo.GetValue(Value.RIGHT))
        {
            stgo.SetValue(Value.BOTTOM, stgo.GetValue(Value.BOTTOM) + 1);
        }
        if (stgo.GetValue(Value.TOP) == stgo.GetValue(Value.BOTTOM))
        {
            stgo.SetValue(Value.RIGHT, stgo.GetValue(Value.RIGHT) + 1);
        }

        //Debug.Log($"{stgo.left},{stgo.right}");

        for (int i = stgo.GetValue(Value.LEFT), j = stgo.GetValue(Value.TOP); i != stgo.GetValue(Value.RIGHT) || j != stgo.GetValue(Value.BOTTOM); i++, j++)
        {
            //tiles[i, j].SetActive(false);
            cStrings[i, j] = " ";
            if (i >= stgo.GetValue(Value.RIGHT)) i--;
            if (j >= stgo.GetValue(Value.BOTTOM)) j--;
        }
    }
    /// <summary>�����̎l�p�`�`��(�f�o�b�O)</summary>
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
    /// <summary>���ׂẴ^�C����\��</summary>
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


    //Set�֐�


    //Get�֐�
    /// <summary>�Œᕪ������Ԃ�</summary>
    public int GetDiv()
    {
        return cDiv;
    }
    /// <summary>���N�G�X�g���ꂽ�^�C���̍��W��Ԃ�</summary>
    public Vector3 GetTilePos(Point tpoint)
    {
        return cTiles[tpoint.X, tpoint.Y].transform.position;
    }
    /// <summary>�����̂܂Ƃ܂��n��</summary>
    public RoomManager GetRoomManager()
    {
        return cRm;
    }
    /// <summary>�^�C���̏���n��</summary>
    public String[,] GetStrings()
    {
        return cStrings;
    }
    /// <summary>�^�C���̃I�u�W�F�N�g��n��</summary>
    /// <returns></returns>
    public GameObject[,] GetTileObjects()
    {
        return cTiles;
    }
    /// <summary>�f�o�b�N�\���̂���Ȃ�</summary>
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

    //�G���A�����p�N���X
    private class Area
    {
        //�I�u�W�F�N�g
        /// <summary>�}�b�v�W�F�l���[�^�[</summary>
        private MapGeneretor cMg;
        /// <summary>���g���番�������G���A</summary>
        private Area[] cChildArea;
        /// <summary>�����̏��</summary>
        private Lurd cRoom;


        //�����o�ϐ�
        /// <summary>����</summary>
        private int mWidth;
        /// <summary>�c��</summary>
        private int mHeight;
        /// <summary>���g�̐؂�ꂽ�ʒu</summary>
        private AreaDiv mAd;
        /// <summary>���������邩�ǂ���</summary>
        private bool isRoom;
        /// <summary>�����n�_</summary>
        private int mDiv;


        //�����o�֐�
        /// <summary>�������\�ȏꍇ�����A�����Ȃ畔���𐶐�</summary>
        public bool CheckDiv()
        {
            if(cMg.GetDebug()) Debug.Log("�����m�F");
            bool w = (mWidth - mDiv <= 0);
            bool h = (mHeight - mDiv <= 0);
            if (w && h)//�����s��
            {
                if(cMg.GetDebug()) Debug.Log("�����s��");
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
            if(cMg.GetDebug()) Debug.Log("�����J�n");
            int div;
            bool[] flg = { false, false };
            Lurd tmp = mAd.GetLurd(ArDi.AREA);
            Lurd[] area = { tmp, tmp, tmp };
            AreaDiv ad = new AreaDiv();
            if (dir == Direction.VER)
            {
                div = UnityEngine.Random.Range(tmp.GetValue(Value.LEFT) + 6, tmp.GetValue(Value.RIGHT) - 6);
                //Debug.Log($"�^�e�����_{div}");
                area[0].SetValue(Value.RIGHT, div - 1);
                area[1].SetValue(Value.LEFT, div + 1);
                area[2].SetValue(Value.LEFT, div);
                area[2].SetValue(Value.RIGHT, div);

            }
            else//dir == Direction.HOR
            {
                div = UnityEngine.Random.Range(tmp.GetValue(Value.TOP) + 6, tmp.GetValue(Value.BOTTOM) - 6);
                //Debug.Log($"���R�����_{div}");
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
        /// <summary>�G���A�̐���</summary>
        private Area CreatArea(AreaDiv tad)
        {
            //�����G���A�̐����A����
            return new Area(tad, cMg);
        }
        /// <summary>�����s�\�ȃG���A�ɕ��������</summary>
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
            cMg.InsertRoom(cRoom);

        }
        /// <summary>����_�֌������ē���L�΂�</summary>
        private void CreateRoad(Area[] tarea, Lurd tdiv, Direction tdir)
        {
            Lurd[] road = new Lurd[3];

            for (int i = 0; i < tarea.Length; i++)
            {
                if (tdir == Direction.HOR)
                {
                    int value = UnityEngine.Random.Range(tarea[i].cRoom.GetValue(Value.LEFT), tarea[i].cRoom.GetValue(Value.RIGHT));
                    road[i].SetValue(Value.LEFT, value);
                    road[i].SetValue(Value.RIGHT, value);
                    road[i].SetValue(Value.TOP, tarea[i].cRoom.GetValue(Value.TOP));
                    road[i].SetValue(Value.BOTTOM, tdiv.GetValue(Value.BOTTOM));
                }
                else
                {
                    int num = UnityEngine.Random.Range(tarea[i].cRoom.GetValue(Value.TOP), tarea[i].cRoom.GetValue(Value.BOTTOM));
                    road[i].SetValue(Value.TOP, num);
                    road[i].SetValue(Value.BOTTOM, num);
                    road[i].SetValue(Value.LEFT, tarea[i].cRoom.GetValue(Value.LEFT));
                    road[i].SetValue(Value.RIGHT, tdiv.GetValue(Value.RIGHT));
                }
                cMg.Draw(road[i]);

            }

            //�L�΂��������Ȃ�
            road[2].SetValue(Value.LEFT, road[0].GetValue(Value.RIGHT));
            road[2].SetValue(Value.TOP, road[0].GetValue(Value.BOTTOM));
            road[2].SetValue(Value.RIGHT, road[1].GetValue(Value.RIGHT));
            road[2].SetValue(Value.BOTTOM, road[1].GetValue(Value.BOTTOM));

            cMg.Draw(road[2]);
        }
        /// <summary>�ŉ��w�̕������擾</summary>
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


        //Set�֐�


        //Get�֐�


        /// <summary>�R���X�g���N�^</summary>
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
