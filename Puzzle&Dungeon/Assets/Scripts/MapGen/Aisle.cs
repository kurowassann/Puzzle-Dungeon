using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>�L���̏��</summary>
public class Aisle
{
    //�I�u�W�F�N�g
    /// <summary>�ʘH�̍��W</summary>
    private List<GameObject> cAisleTiles;
    /// <summary>���g��ID</summary>
    private int cAisleId;
    /// <summary>���E�̕���ID</summary>
    private int[] cRoomId;

    //�����o�ϐ�

    //�����o�֐�
    /// <summary>�L���̃|�C���g�̒ǉ�</summary>
    public void AddTilePoints()
    {

    }


    //�R���X�g���N�^
    public Aisle(int num) 
    {
        cAisleTiles= new List<GameObject>();
        cAisleId = num;
        cRoomId = new int[2];
    }


}
