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
