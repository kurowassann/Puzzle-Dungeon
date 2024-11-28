using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [Tooltip("�^�C�}�[�̍ő�l"), SerializeField]
    const float MAX_TIMER = 0.5f;
    [Tooltip("�^�C�}�["), SerializeField]
    float timer;
    [Tooltip("�{�^������������L�["), SerializeField]
    KeyCode keyCode;
    [Tooltip("�^�C�}�[��\������Image�I�u�W�F�N�g"), SerializeField]
    Image timerImage;
    [Tooltip("�^�C�}�[�����܂������ɌĂ΂�鏈��"), SerializeField]
    UnityEvent onTimerReachedEvent;

    /// <summary>
    /// �L�[��������Ă����Ƃ��̉��Z����
    /// </summary>
    /// <returns>�^�C�}�[�����߂������ǂ���</returns>
    public bool AddTime()
    {
        timer += Time.deltaTime;
        if (timer > MAX_TIMER)
        {
            timer = MAX_TIMER;
            return true;
        }
        TimerDisplay();
        return false;
    }
    /// <summary>
    /// �L�[��������Ă��Ȃ��������̌��Z����
    /// LIMIT�Ɋւ�炸�Œ���b�Ń^�C�}�[��0�ɖ߂�
    /// </summary>
    public void SubTime()
    {
        timer -= Time.deltaTime * MAX_TIMER;
        if(timer < 0)
        {
            timer = 0;
        }
        TimerDisplay();
    }

    /// <summary>
    /// �^�C�}�[�̃��Z�b�g����
    /// </summary>
    public void ResetTimer()
    {
        timer = 0;
    }

    /// <summary>
    /// �L�[�R�[�h�擾
    /// </summary>
    /// <returns></returns>
    public KeyCode GetKeyCode() { return keyCode; }

    /// <summary>
    /// �^�C�}�[�����܂������̊֐����擾
    /// </summary>
    /// <returns></returns>
    public UnityEvent GetAction() { return onTimerReachedEvent; }

    /// <summary>
    /// �^�C�}�[�̕\������
    /// </summary>
    void TimerDisplay()
    {
        timerImage.fillAmount = timer / MAX_TIMER;
    }
}
