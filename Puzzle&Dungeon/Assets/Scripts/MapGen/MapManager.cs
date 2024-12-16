using System.Collections;
using System.Collections.Generic;
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
    /// <summary>�}�b�v�����p�N���X</summary>
    private MapGeneretor cMg;
    /// <summary>�^�C�����</summary>
    private string[,] cTiles;
    /// <summary>�^�C���̃I�u�W�F�N�g</summary>
    private GameObject[,] cTileObjects;

    //�����o�ϐ�
    private int mNum;

    //�����o�֐�
    //private

    //public
    public void Init()
    {
        //�}�b�v�����I�u�W�F�N�g�̐���
        GameObject clone = Instantiate(mapGen, this.transform);
        cMg = clone.GetComponent<MapGeneretor>();
        if(isStart)
        {
            cMg.Init(40,40,15);
        }

        //���������}�b�v�̏����󂯎��
        cTiles = cMg.GetStrings();
        cTileObjects = cMg.GetTileObjects();
        cRm = cMg.GetRoomManager();


        mNum = 0;
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
            if(Input.GetKeyDown(KeyCode.Space) ) 
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
                cMg.TileOpen();
            }
        }
    }

}
