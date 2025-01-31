using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>�L���̏��</summary>
public class Aisle
{
    //�I�u�W�F�N�g
    /// <summary>�\�������ׂ����̂�</summary>
    private bool isActive;
    /// <summary>�Ǘ���</summary>
    private AisleManager cAm;
    /// <summary>�ʘH�̍��W</summary>
    private List<GameObject> cAisleTiles;
    /// <summary>���g��ID</summary>
    private int cAisleId;
    /// <summary>���E�̕���ID</summary>
    private RoomJoint[] cRoomJoints;
    /// <summary>�ʒu���</summary>
    private Lurd[] cLurds;

    //�����o�ϐ�

    //�����o�֐�
    /// <summary>�L���̃|�C���g�̒ǉ�</summary>
    public void AddTilePoints()
    {

    }
    /// <summary>�L���̕\��</summary>
    public bool OpenAllTiles()
    {
        if(isActive)
        {
            return true;
        }

        for(int i = 0; i < cAisleTiles.Count;i++)
        {
            cAisleTiles[i].SetActive(true);
        }
        isActive = true;

        return false;
    }

    //Set�֐�
    //
    public void SetTiles(List<GameObject> tgos)
    {
        for(int i = 0;i < tgos.Count;i++)
        {
            cAisleTiles.Add(tgos[i]);
        }
    }

    //Get�֐�
    public int GetLength()
    {
        return cLurds.Length;
    }
    //
    public Lurd GetLurd(int num)
    {
        return cLurds[num];
    }
    /// <summary>�ڑ�������Ԃ�</summary>
    public RoomJoint[] GetRoomJoint()
    {
        return cRoomJoints;
    }

    //�R���X�g���N�^
    public Aisle(Lurd[] tlurds, RoomJoint[] trj,int num,AisleManager tam) 
    {
        cLurds = tlurds;
        
        cAisleTiles= new List<GameObject>();
        cAisleId = num;
        cRoomJoints = trj;
        cRoomJoints[0].SetAisleId(num);
        cRoomJoints[1].SetAisleId(num);


        cAm = tam;
    }


}
