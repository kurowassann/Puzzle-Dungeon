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
    private void Init()
    {
       cRooms = new List<Room>();
        mRoomCount = 0;
    }
    //�����̒ǉ�
    public void AddRoom(Lurd tlurd)
    {
        cRooms.Add(new Room(tlurd,mRoomCount, this));

        mRoomCount++;   
    }
    //
    public void OpenOneRoom(int num)
    {
        cRooms[num].OpenTiles();
    }

    //Set�֐�


    //Get�֐�
    /// <summary>�����_���ȕ����̃����_���ȃ|�W�V������Ԃ�</summary>
    public Point GetRandomRoom()
    {
        Point point;
        int num = Random.Range(0, cRooms.Count);
        point = cRooms[num].GetRandomPoint();


        return point;
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
    //
    public Room GetRoom(int num)
    {
        return cRooms[num].GetRoom();
    }

    public  RoomManager()
    {
        Debug.Log($"{this}�R���X�g���N�^");

        Init();
    }
    
}
