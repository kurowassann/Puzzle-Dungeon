using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [Tooltip("タイマーの最大値"), SerializeField]
    const float MAX_TIMER = 0.5f;
    [Tooltip("タイマー"), SerializeField]
    float timer;
    [Tooltip("ボタンが反応するキー"), SerializeField]
    KeyCode keyCode;
    [Tooltip("タイマーを表示するImageオブジェクト"), SerializeField]
    Image timerImage;
    [Tooltip("タイマーがたまった時に呼ばれる処理"), SerializeField]
    UnityEvent onTimerReachedEvent;

    /// <summary>
    /// キーが押されていたときの加算処理
    /// </summary>
    /// <returns>タイマーが超過したかどうか</returns>
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
    /// キーが押されていなかった時の減算処理
    /// LIMITに関わらず最長一秒でタイマーが0に戻る
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
    /// タイマーのリセット処理
    /// </summary>
    public void ResetTimer()
    {
        timer = 0;
    }

    /// <summary>
    /// キーコード取得
    /// </summary>
    /// <returns></returns>
    public KeyCode GetKeyCode() { return keyCode; }

    /// <summary>
    /// タイマーがたまった時の関数を取得
    /// </summary>
    /// <returns></returns>
    public UnityEvent GetAction() { return onTimerReachedEvent; }

    /// <summary>
    /// タイマーの表示処理
    /// </summary>
    void TimerDisplay()
    {
        timerImage.fillAmount = timer / MAX_TIMER;
    }
}
