using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Json;


public class OptionManager : MonoBehaviour
{
    // todo:apply�̊m�F�_�C�A���O

    /// <summary>
    /// �I�v�V������ʂ̑I�����j���[
    /// </summary>
    public enum SelectableOption
    {
        SOUND,
        LIGHT,

        CLOSE_OPT,
        BACK_TITLE,
    }

    /// <summary>
    /// �ݒ荀�ڂ̒��g
    /// </summary>
    public enum SettingObject
    {
        TYPE,
        VALUE,
        SLIDER,
    }

    [Tooltip("�C���X�^���X")]
    public static OptionManager Instance { get; private set; }

    [Tooltip("�I�v�V�������J���Ă��邩�ǂ���")]
    public bool isOptionOpened { get; private set; }


    [Tooltip("�ݒ�̐e�I�u�W�F�N�g�z��"), SerializeField]
    Transform[] settingParents;
    [Tooltip("�ۑ��e�L�X�g"), SerializeField]
    Transform saveSettingText;
    [Tooltip("��ۑ��ŕ���e�L�X�g"), SerializeField]
    Transform unsaveSettingText;
    [Tooltip("�^�C�g���ɖ߂�e�L�X�g"), SerializeField]
    Transform backToTitleText;

    [Tooltip("�I�𒆂̃J�[�\��"), SerializeField]
    GameObject selectedCursor;
    [Tooltip("�I�𒆂̃��j���["), SerializeField]
    SelectableOption selectedMenu;


    [Tooltip("�`�悷��L�����o�X")]
    Canvas canvas;
    [Tooltip("�^�C�g���V�[�����ǂ���")]
    bool isTitleScene;
    [Tooltip("���C�g�p�̃L�����o�X")]
    Canvas canvasLight;


    /// <summary>
    /// �V���O���g��������
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoded;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    /// <summary>
    /// �I�u�W�F�N�g�j�����̏���
    /// </summary>
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoded;
    }

    /// <summary>
    /// �V�[�����؂�ւ�邽�тɌĂ΂�鏈��
    /// <para><seealso cref="Init"/>������</para>
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoded(Scene scene, LoadSceneMode mode)
    {
        // ������
        Init();
        if(scene.name == "Title")
        {
            isTitleScene = true;
            backToTitleText.gameObject.SetActive(false);
        }
        else
        {
            isTitleScene = false;
            backToTitleText.gameObject.SetActive(true);
        }
    }


    void Update()
    {
        // �I�v�V�������J���Ă���Ԃ̂ݏ������s��
        if (isOptionOpened)
        {
            // ���͂���������
            if (OptionInput())
            {
                // �ύX��UI�ɔ��f����
                OptionDisplay();
            }
        }

    }


    /// <summary>
    /// ����������
    /// <para>�L�����o�X�̃J�������Đݒ�</para>
    /// <para>�L�����o�X�̔�\��</para>
    /// </summary>
    void Init()
    {
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        canvasLight = transform.GetChild(1).GetComponent<Canvas>();
        // �L�����o�X�̃J�����ݒ�
        // ���C���J�������Đݒ�
        if (Camera.main != null)
        {
            canvas.worldCamera = Camera.main;
            canvasLight.worldCamera = Camera.main;
        }
        else
        {
            print("�J����������܂���");
            return;
        }
        // �t���O�̏�����
        isOptionOpened = false;
        // ���׌y���̂���canvas�̓I�t��
        canvas.gameObject.SetActive(false);

        var set = LoadSettings();
        if(set != null)
        {
            var soundSlider = settingParents[(int)SelectableOption.SOUND].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
            var lightSlider = settingParents[(int)SelectableOption.LIGHT].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
            soundSlider.value = set.sound_volume;
            lightSlider.value = set.screen_light;
        }
        // �ݒ蔽�f
        SetVolume();
        SetLight();
    }

    /// <summary>
    /// ���͏���
    /// </summary>
    bool OptionInput()
    {

        // �J�[�\���ړ��̓���
        if (Cursorinput())
        {
            return true;
        }

        // �ݒ�l�ύX�̓���
        if (Volumeinput())
        {
            return true;
        }

        // �X�y�[�X�������Ƃ��̏���
        SubmitInput();


        return false;
    }

    /// <summary>
    /// �J�[�\�����ړ���������͏���
    /// </summary>
    /// <returns></returns>
    bool Cursorinput()
    {
        bool ret = false;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedMenu--;
            if (selectedMenu < 0)
            {
                selectedMenu = 0;
            }
            ret = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {


            selectedMenu++;

            if (isTitleScene && selectedMenu == SelectableOption.BACK_TITLE)
            {
                selectedMenu--;
            }


            if (selectedMenu > (SelectableOption)Common.Common.GetEnumLength<SelectableOption>() - 1)
            {
                selectedMenu = (SelectableOption)Common.Common.GetEnumLength<SelectableOption>() - 1;

            }
            ret = true;
        }

        return ret;
    }

    /// <summary>
    /// �ݒ�l��ύX������͏���
    /// </summary>
    /// <returns></returns>
    bool Volumeinput()
    {
        // �ύX����ꏊ

        bool ret = false;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            switch (selectedMenu)
            {
                case SelectableOption.SOUND:
                case SelectableOption.LIGHT:
                    var slider = settingParents[(int)selectedMenu].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
                    slider.value -= 1;
                    // ���ʂ��Z�b�g
                    SetVolume();

                    // ���邳�̃Z�b�g
                    SetLight();
                    break;
            }
            ret = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            switch (selectedMenu)
            {
                case SelectableOption.SOUND:
                case SelectableOption.LIGHT:
                    var slider = settingParents[(int)selectedMenu].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
                    slider.value += 1;
                    // ���ʂ��Z�b�g
                    SetVolume();

                    // ���邳�̃Z�b�g
                    SetLight();
                    break;
            }
            ret = true;
        }

        return ret;
    }

    /// <summary>
    /// ����{�^�������������̏���
    /// </summary>
    /// <returns></returns>
    void SubmitInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (selectedMenu)
            {
                case SelectableOption.CLOSE_OPT:
                    // �ݒ�̕ۑ�
                    SaveSettings();
                    break;
                case SelectableOption.BACK_TITLE:

                    // �m�F�_�C�A���O���o��
                    Common.Common.LoadScene("TItle");
                    break;
            }
        }
    }


    /// <summary>
    /// ���͂�`�悷�鏈��
    /// </summary>
    void OptionDisplay()
    {
        // �J�[�\���̍��W�𔽉f
        CursorDisplay();

        // �ݒ�l��UI�ɔ��f
        SettingsDisplay();
    }

    /// <summary>
    /// �J�[�\���̍��W���X�V���鏈��
    /// </summary>
    void CursorDisplay()
    {
        var pos = selectedCursor.transform.position;
        // �J�[�\���̍��W�X�V
        switch (selectedMenu)
        {
            case SelectableOption.SOUND:
            case SelectableOption.LIGHT:
                pos = settingParents[(int)selectedMenu].GetChild((int)SettingObject.TYPE).position;
                break;
            case SelectableOption.CLOSE_OPT:
                pos = unsaveSettingText.position;
                break;
            case SelectableOption.BACK_TITLE:
                pos = backToTitleText.position;
                break;

        }
        selectedCursor.transform.position = pos;
    }

    /// <summary>
    /// �X���C�_�[�𐔒lUI�ɔ��f���鏈��
    /// </summary>
    void SettingsDisplay()
    {
        // ���ʂƖ��邳�Ń��[�v
        for (int i = 0; i < settingParents.Length; i++)
        {
            // todo:���͂�UI�ɔ��f
            var textVal = settingParents[i].GetChild((int)SettingObject.VALUE).GetComponent<Text>();
            var slider = settingParents[i].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
            textVal.text = slider.value.ToString();
        }
    }


    /// <summary>
    /// �I�v�V�������J�������A�e�}�l�[�W���[����Ăяo��
    /// </summary>
    public void OpenOption()
    {
        //print("�I�v�V�������J��");
        // todo:�I�v�V������ʂ̏�����.�ݒ�̓ǂݍ��݂�UI�ɔ��f
        var settings = LoadSettings();



        if (settings != null)
        {
            var soundSlider = settingParents[(int)SelectableOption.SOUND].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
            var lightSlider = settingParents[(int)SelectableOption.LIGHT].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
            soundSlider.value = settings.sound_volume;
            lightSlider.value = settings.screen_light;
            SettingsDisplay();
        }

        selectedMenu = 0;
        CursorDisplay();
        isOptionOpened = true;
        canvas.gameObject.SetActive(true);
        // �J�[�\���ʒu�̏�����
        selectedCursor.transform.position = settingParents[(int)SelectableOption.SOUND].GetChild((int)SettingObject.TYPE).position;
    }

    /// <summary>
    /// �I�v�V��������鏈��
    /// </summary>
    void CloseOption()
    {
        canvas.gameObject.SetActive(false);
        isOptionOpened = false;
        //print("�I�v�V���������");
        //todo:�ēx�ǂݍ��񂾃f�[�^�Œl��ύX
    }

    /// <summary>
    /// �ݒ��ۑ�
    /// </summary>
    void SaveSettings()
    {
        var settings = new JsonSettings();
        var soundSlider = settingParents[(int)SelectableOption.SOUND].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
        var lightSlider = settingParents[(int)SelectableOption.LIGHT].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
        settings.sound_volume = soundSlider.value;
        settings.screen_light = lightSlider.value;

        var txt = JsonUtility.ToJson(settings);
        print("�ۑ��O�f�[�^:" + txt);
        PlayerPrefs.SetString(Common.Common.KEY_SETTINGS, txt);
        PlayerPrefs.Save();

        // �ۑ����ďI��
        Invoke("CloseOption", 0.1f);
    }

    /// <summary>
    /// ���ʂ̐ݒ�
    /// </summary>
    public void SetVolume()
    {
        // SoundManager�̉��ʂ��Z�b�g����
        var soundSlider = settingParents[(int)SelectableOption.SOUND].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
        var normaliazedVol = Common.Common.NormalizedFunc(soundSlider.value, soundSlider.minValue, soundSlider.maxValue, 0, 1);
        SoundManager.Instance?.SetVolumeRate(normaliazedVol);
		//SoundManager.Instance?.SetSoundsVolume(normaliazedVol);
        //SoundManager.Instance?.SetSEVolume(normaliazedVol);
    }

    public void SetLight()
    {
        var img = canvasLight.transform.GetChild(0).GetComponent<Image>();
        var col = img.color;
        var lightSlider = settingParents[(int)SelectableOption.LIGHT].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
        var normVal = Common.Common.NormalizedFunc(lightSlider.value, lightSlider.minValue, lightSlider.maxValue, 0.2f, 0.8f);
        col.a = 1 - normVal;
        img.color = col;
    }

    /// <summary>
    /// �ݒ�t�@�C���ǂݍ���
    /// </summary>
    /// <returns></returns>
    JsonSettings LoadSettings()
    {
        var txt = PlayerPrefs.GetString(Common.Common.KEY_SETTINGS, null);
        print("�ǂݍ��񂾐ݒ�f�[�^:" + txt);
        if (txt != null)
        {
            return JsonUtility.FromJson<JsonSettings>(txt);
        }
        else
        {
            print("�ǂݍ��ݎ��s");
            return null;
        }
    }
}
