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
    /// <summary>�v���C���̏����ʒu�����߂�</summary>
    private PosId SelectPlayerFirstPos()
    {
        PosId posId = cMm.GetRandomPlayer();

        return posId;
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
        PosId posId = SelectPlayerFirstPos();
        cPlayer.Init(this,posId,1,"p");
        cPlayer.transform.position = cMm.GetTilePos(posId.GetPos());


    }

    //
    private void Update()
    {
        
    }
}
