using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    [Tooltip("�X�e�B�b�N"), SerializeField]
    private InputAction inputMover;
    [Tooltip("�������x"), SerializeField]
    private float speed = 0.1f;

    /// <summary>
    ///�I�u�W�F�N�g���A�N�e�B�u�ɂȂ������ɌĂ΂��C�x���g
    /// </summary>
    private void OnDisable()
    {
        inputMover.Disable();
    }

    /// <summary>
    /// �I�u�W�F�N�g����\���ɂȂ������ɌĂ΂��C�x���g
    /// </summary>
    private void OnEnable()
    {
        inputMover.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�X�e�B�b�N�̃x�N�g�����擾
        var moveVec = inputMover.ReadValue<Vector2>();

        //�v���C�����ړ�
        transform.position += new Vector3(
            moveVec.x * speed,
            moveVec.y * speed,
            0);
    }
}
