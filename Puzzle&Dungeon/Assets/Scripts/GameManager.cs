using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

/// <summary>�Q�[���𓮂���</summary>
public class GameManager : MonoBehaviour 
{ 

    //�I�u�W�F�N�g
    [Tooltip("�}�b�v�̊Ǘ��I�u�W�F�N�g"),SerializeField]
    private MapManager cMm;
    [Tooltip("�v���C���Ǘ��I�u�W�F�N�g"), SerializeField]
    private Player cPlayer;

    //�����o�ϐ�
    

    //�����o�֐�
    //private
    /// <summary>�^�[���̏I�����m�F</summary>
    private bool TurnCheck()
    {
        if(cPlayer.GetAction())
        {
            return true;
        }

        return false;
    }
    /// <summary>�^�[���̏I��</summary>
    private void TurnEnd()
    {
        cPlayer.TurnEnd();
    }

    //public
   

    //Set�֐�

    //Get�֐�

    /// <summary>�N�����ɌĂ�</summary>
    private void Start()
    {
        //�}�b�v�̐���
        cMm.Init();

        //�v���C���̐���
        //�����ʒu�̌���
        PosId posId = cMm.GetRandomPlayer();
        cPlayer.Init(this, cMm,posId,1,"p");
        cPlayer.transform.position = cMm.GetTilePos(posId.GetPos());
    }

    //
    private void Update()
    {
        if(TurnCheck())
        {
            TurnEnd();
        }


        
    }
}
