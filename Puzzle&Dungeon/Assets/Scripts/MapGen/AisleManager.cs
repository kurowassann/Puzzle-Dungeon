using Common;
using System.Collections;
using System.Collections.Generic;
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
    }
    /// <summary>�L���̒ǉ�����</summary>
    public void AddAisle()
    {
        cAisles.Add(new Aisle(mAisleCount));
    }
    /// <summary>�L���^�C���̒ǉ�</summary>
    public void AddAisleTiles()
    {

    }

    //Set�֐�

    //Get�֐�


    //�R���X�g���N�^
    public AisleManager() 
    {
        Init();
    }
}
