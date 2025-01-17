using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>�L�����܂Ƃ߂ĊǗ�����</summary>
public class AisleManager
{
    //�I�u�W�F�N�g
    /// <summary>�L���N���X</summary>
    private List<Aisle> cAisles;
    /// <summary>�ڑ����̃��X�g</summary>
    private List<RoomJoint> cJoints;


    //�����o�ϐ�
    /// <summary>�L���̐��AID�Ɏg��</summary>
    private int mAisleCount;

    //�����o�֐�
    //private
    

    //public
    /// <summary>����������</summary>
    public void Init()
    {
        mAisleCount = 0;
        cAisles = new List<Aisle>();
        cJoints = new List<RoomJoint>();
    }
    /// <summary>�L���̒ǉ�����</summary>
    public int  AddAisle(Lurd[] tlurds, Point[] tpoints, int[] tids)
    {
        cAisles.Add(new Aisle(tlurds, tpoints, tids,mAisleCount,this));
        mAisleCount++;
        return mAisleCount - 1;
    }
    /// <summary>�L���^�C���̒ǉ�</summary>
    public void AddAisleTiles()
    {

    }
    /// <summary>�ڑ����������X�g��</summary>
    public void CreateJointList()
    {
        for(int i = 0; i < cAisles.Count; i++) 
        {
            RoomJoint[] Rjs = cAisles[i].GetRoomJoint();
            for(int j = 0;j < Rjs.Length; j++)
            {
                cJoints.Add(Rjs[j]);
                Debug.Log($"�ڑ�������{Rjs[j].GetPos()}�ł�");
            }
        }
    }
    /// <summary>�v���C�����ڑ������ɂ��邩���ׂ�</summary>
    public PosId ChangeJoint(PosId tpi)
    {
        int num = -1;

        for(int i = 0; i < cJoints.Count;i++)
        {
            if(tpi.GetPos() == cJoints[i].GetPos())
            {
                num = i; 
                break;
            }
        }

        if(num == -1)
        {
            Debug.Log("�����ω��Ȃ�");
            return tpi;
        }

        if(tpi.GetRA() == RoomAisle.ROOM)
        {
            tpi.SetId(cJoints[num].GetId(RoomAisle.AISLE),RoomAisle.AISLE);

        }
        else if(tpi.GetRA() == RoomAisle.AISLE)
        {
            tpi.SetId(cJoints[num].GetId(RoomAisle.ROOM), RoomAisle.ROOM);

        }

        return tpi;
    }
    /// <summary>�L���̕\��</summary>
    public void OpenOneAisle(int num)
    {
        cAisles[num].OpenAllTiles();
    }

    //Set�֐�

    //Get�֐�
    /// <summary>�L���̐���Ԃ�</summary>
    public int GetAisleCount()
    {
        return mAisleCount;
    }
    /// <summary>�w�肳�ꂽID�̘L����Ԃ�</summary>
    public Aisle GetAisle(int num)
    {
        return cAisles[num];
    }

    //�R���X�g���N�^
    public AisleManager() 
    {
        Init();
    }
}
