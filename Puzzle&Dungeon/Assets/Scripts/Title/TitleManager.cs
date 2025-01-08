using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Common;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [Tooltip("�I�𒆂̃��j���[")]
    SelectedMenu selectedMenu;
    [Tooltip("�I���ł���UI�̔z��")]
    Image[] selectableImages;
    [Tooltip("�I���ł���UI�̐e�I�u�W�F�N�g"), SerializeField]
    Transform selectableParent;

    //�I�u�W�F�N�g
    [Tooltip("�X�e�B�b�N"), SerializeField]
    private InputAction inputMover;
    [Tooltip("�X�e�B�b�N"), SerializeField]
    private InputAction inputSelect;
    [Tooltip("�X�e�B�b�N"), SerializeField]
    private InputActionAsset inputActions;

    bool isInput = false;

    /// <summary>
    /// �I���\UI�̗�
    /// </summary>
    enum SelectedMenu
    {
        START,
        OPTION,
        QUIT,
    }



    // Start is called before the first frame update
    void Start()
    {
        Init();

        inputMover = inputActions.FindAction("UI/Move");
        inputSelect = new InputAction("AButton", binding: "<Gamepad>/buttonEast");

        inputMover.Enable();
        inputSelect.Enable();
    }

    /// <summary>
    /// ����������
    /// </summary>
    void Init()
    {
        // �J�[�\������ԏ�ŏ�����
        selectedMenu = 0;

        // �I���ł���UI�����ׂĎ擾
        selectableImages = selectableParent.GetComponentsInChildren<Image>();
        // �J�[�\���ʒu�̏�����
        MainCursorDisplay();

        //// �I�v�V�������\��
        //optionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // �I�v�V�������샂�[�h�łȂ��Ȃ�
        if (!OptionManager.Instance.isOptionOpened)
        {
            // ����{�^�������ꂽ��J�[�\���ʒu�ɉ����ď�������
            if (PushSubmit())
            {
                SoundManager.Instance.PlaySE(Sounds.SEType.Submit);
                return;
            }
            // �㉺���͂���������J�[�\����ύX���\��
            if (SelectMainCursor())
            {
                MainCursorDisplay();
                SoundManager.Instance.PlaySE(Sounds.SEType.CursorMove);
            }
        }


    }

    /// <summary>
    /// ����{�^���������̏���
    /// </summary>
    /// <returns></returns>
    bool PushSubmit()
    {
        bool ret = false;

        float buttonValue = inputSelect.ReadValue<float>();

        if (buttonValue > 0.5f)
        {
            // ���͂�������
            ret = true;

            // �I�𒆂�UI�ŕ���
            switch (selectedMenu)
            {
                // �Q�[���J�n
                case SelectedMenu.START:
                    PushStart();
                    break;
                // �I�v�V�������J��
                case SelectedMenu.OPTION:
                    PushOption();
                    break;
                // �Q�[���I��
                case SelectedMenu.QUIT:
                    PushQuit();
                    break;

                default:
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���͂�������
            ret = true;

            // �I�𒆂�UI�ŕ���
            switch (selectedMenu)
            {
                // �Q�[���J�n
                case SelectedMenu.START:
                    PushStart();
                    break;
                // �I�v�V�������J��
                case SelectedMenu.OPTION:
                    PushOption();
                    break;
                // �Q�[���I��
                case SelectedMenu.QUIT:
                    PushQuit();
                    break;

                default:
                    break;
            }
        }

        return ret;
    }

    /// <summary>
    /// �v���C���[�̓��͂�selectedNum�𑝌�������
    /// </summary>
    /// <returns>���͂���������true</returns>
    bool SelectMainCursor()
    {
        // �߂�l�̏�����
        bool ret = false;

        if (inputMover.ReadValue<Vector2>() == new Vector2(0, 0))
        {
            isInput = false;
        }

        // ������̓���
        if (inputMover.ReadValue<Vector2>().y == 1.0f && isInput == false)
        {
            // ���̓A��
            ret = true;
            isInput = true;
            selectedMenu--;
            // �ŉ��[�ɍs�������ɖ߂�
            if (selectedMenu < 0)
            {
                selectedMenu = (SelectedMenu)selectableImages.Length - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ���̓A��
            ret = true;
            selectedMenu--;
            // �ŉ��[�ɍs�������ɖ߂�
            if (selectedMenu < 0)
            {
                selectedMenu = (SelectedMenu)selectableImages.Length - 1;
            }
        }

        // �������̓���
        if (inputMover.ReadValue<Vector2>().y == -1.0f && isInput == false)
        {
            // ���̓A��
            ret = true;
            isInput = true;
            selectedMenu++;
            // �ŏ�[�ɍs�����牺�ɖ߂�
            if (selectedMenu > (SelectedMenu)selectableImages.Length - 1)
            {
                selectedMenu = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            // ���̓A��
            ret = true;
            selectedMenu++;
            // �ŏ�[�ɍs�����牺�ɖ߂�
            if (selectedMenu > (SelectedMenu)selectableImages.Length - 1)
            {
                selectedMenu = 0;
            }
        }

        return ret;

    }

    /// <summary>
    /// �I�𒆂̔ԍ��ɂ����
    /// </summary>
    void MainCursorDisplay()
    {
        for (int i = 0; i < selectableImages.Length; i++)
        {
            if (i == (int)selectedMenu)
            {
                selectableImages[i].transform.localScale = Vector3.one * 1.1f;
                selectableImages[i].color = Color.white;
            }
            // ��I�𒆂̓O���[�A�E�g
            else
            {
                selectableImages[i].transform.localScale = Vector3.one;
                selectableImages[i].color = Color.gray;
            }
        }
    }

    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    void PushStart()
    {
        print("�V�[���J��");
        //
        Common.Common.LoadScene(Common.Common.SCENE_SELECT);
    }


    /// <summary>
    /// �I�v�V�����̕\��
    /// </summary>
    void PushOption()
    {
        // OptionManager�̏������Ăяo��
        OptionManager.Instance.OpenOption();

        //print("�I�v�V�����\��");
    }


    /// <summary>
    /// �Q�[���I��
    /// </summary>
    void PushQuit()
    {
        print("�I��");
        Common.Common.GameQuit();
    }
}
