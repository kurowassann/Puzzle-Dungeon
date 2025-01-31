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
    public int  AddAisle(Lurd[] tlurds,RoomJoint[] trjs)
    {
        cAisles.Add(new Aisle(tlurds, trjs,mAisleCount,this));
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
                //Debug.Log($"�ڑ�������{Rjs[j].GetPos()}�ł�");
            }
        }
    }
    /// <summary>�L���̕\��</summary>
    public bool OpenOneAisle(int num)
    {
       return cAisles[num].OpenAllTiles();
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
