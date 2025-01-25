using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RoomManager 
{
    //�I�u�W�F�N�g
    /// <summary>�����̊Ǘ�</summary>
    private List<Room> cRooms;

    //�����o�ϐ�
    //���݂̕�����
    private int mRoomCount;

    //�����o�֐�
    /// <summary>����������</summary>
    private void Init()
    {
       cRooms = new List<Room>();
        mRoomCount = 0;
    }
    //�����̒ǉ�
    public int AddRoom(Lurd tlurd)
    {
        cRooms.Add(new Room(tlurd,mRoomCount, this));

        mRoomCount++; 
        return mRoomCount-1;
    }
    /// <summary>�w�肵���������Ƃ炷</summary>
    public bool OpenOneRoom(int num)
    {
        return cRooms[num].OpenTiles();
    }

    //Set�֐�


    //Get�֐�
    /// <summary>�w�肳�ꂽ�����̃����_���ȃ|�W�V������Ԃ�</summary>
    public PosId GetRandomPos(int num)
    {
        PosId posId = new PosId();
        posId.SetPos(cRooms[num].GetRandomPoint());
        posId.SetId(num, RoomAisle.ROOM);


        return posId;
    }
    /// <summary>�v���C���̐��������̕\����ON��</summary>
    public Point GetRandomRoom(int tnum)
    {
        Point point;
        int num = Random.Range(0, cRooms.Count);
        point = cRooms[num].GetRandomPoint();

        cRooms[num].OpenTiles();

        return point;
    }
    /// <summary>���[���̐���Ԃ�</summary>
    public int GetRoomCount()
    {
        return cRooms.Count;
    }
    /// <summary>�w�肳�ꂽ������Ԃ�</summary>
    public Room GetRoom(int num)
    {
        return cRooms[num].GetRoom();
    }

    public  RoomManager()
    {
        //Debug.Log($"{this}�R���X�g���N�^");

        Init();
    }
    
}
