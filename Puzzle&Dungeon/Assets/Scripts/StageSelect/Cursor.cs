using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [Tooltip("�ړ��J�n���̍��W")]
    Vector3 startPosition;
    [Tooltip("�ڕW���W")]
    Vector3 targetPos;
    [Tooltip("�ړ��o�ߎ���")]
    float elapsedTime;
    [Tooltip("�ړ��ɂ����鎞��")]
    const float DURATION = 0.3f;
    [Tooltip("�ړ��t���O")]
    public bool isMoving {  get; private set; }


    /// <summary>
    /// ����������
    /// </summary>
    public void Init(Vector3 _pos)
    {
        transform.position = _pos;
    }


    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            Move();
        }
    }

    /// <summary>
    /// �J�[�\���ړ��J�n���̏���
    /// </summary>
    /// <param name="_tPos">�ڕW�n�_</param>
    public void MoveStart(Vector3 _tPos)
    {
        isMoving = true;
        startPosition = transform.position;
        targetPos = _tPos;
        elapsedTime = 0;
    }


    /// <summary>
    /// �ړ�����.�ڕW�n�_�܂łȂ߂炩�ɓ���
    /// </summary>
    void Move()
    {
        // �o�ߎ���
        elapsedTime += Time.deltaTime;

        // �ړ��̐i�s�x
        var progress = Mathf.Clamp01(elapsedTime / DURATION);

        // �C�[�W���O����
        float smooth = Mathf.SmoothStep(0, 1, progress);

        // ���X�Ɉړ�
        transform.position = Vector3.Lerp(startPosition, targetPos, smooth);

        // �i�s�x��1�Ŋ���
        if(progress >= 1)
        {
            // �O�̂��߂����␳���ďI��
            transform.position = targetPos;
            isMoving = false;
        }
    }
}
