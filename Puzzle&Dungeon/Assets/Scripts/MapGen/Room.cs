using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UIElements;
using System.Drawing;
using UnityEngine.EventSystems;
/*
�ւ₪�����Ă��Ăق������
�l��(����)
�������
�������̃L�����N�^�[
�L���ւ̈ʒu
�Ȃ����Ă��镔��
*/


public class Room 
{
    
    //�I�u�W�F�N�g
    /// <summary>�Ǘ���</summary>
    private RoomManager cRm;
    /// <summary>�����ԍ�</summary>
    private int cRoomId;
    /// <summary>�����̒��_</summary>
    private Lurd cLurd;
    /// <summary>���̒���</summary>
    private int cWidth;
    /// <summary>�c�̒���</summary>
    private int cHeight;
    /// <summary>���g�̃^�C��</summary>
    private List<GameObject> cTiles;

    //�����o�ϐ�


    //�����o�֐�
    //private
    //public
    public void OpenTiles()
    {
        for(int i = 0;i < cTiles.Count;i++) 
        {
            cTiles[i].SetActive(true);
        }
    }

    //Set�֐�
    /// <summary>�G���A�̃I�u�W�F�N�g���擾</summary>
    public void SetTiles(List<GameObject> ttiles)
    {
        cTiles = ttiles;
    }

    //Get�֐�
    /// <summary>�����_���ȕ����̈ʒu��n��</summary>
    public Point GetRandomPoint()
    {
        Point point = new Point();

        point.X = Random.Range(cLurd.GetValue(Value.LEFT), cLurd.GetValue(Value.RIGHT));
        point.Y = Random.Range(cLurd.GetValue(Value.TOP), cLurd.GetValue(Value.BOTTOM));
        
        return point;
    }
    /// <summary>���g��n��</summary>
    public Room GetRoom()
    {
        return this;
    }
    /// <summary>������n��</summary>
    public int GetWidth()
    {
        return cWidth;
    }
    /// <summary>�c����Ԃ�</summary>
    public int Height()
    {
        return cHeight;
    }
    //
    public int GetValue(Value tvalue)
    {
        return cLurd.GetValue(tvalue);
    }



    ///<summary>�R���X�g���N�^</summary>
    public Room(Lurd tlurd,int tid, RoomManager trm)
    {
        Debug.Log($"�����������@����ID:{tid}");
        cLurd = tlurd;
        cRoomId = tid;
        cWidth = cLurd.GetValue(Value.RIGHT)-cLurd.GetValue(Value.LEFT);
        cWidth = cLurd.GetValue(Value.BOTTOM) -cLurd.GetValue(Value.TOP);
        cTiles = new List<GameObject>();
        cRm = trm;

    }

}
