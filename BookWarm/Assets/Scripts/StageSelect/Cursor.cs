using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [Tooltip("移動開始時の座標")]
    Vector3 startPosition;
    [Tooltip("目標座標")]
    Vector3 targetPos;
    [Tooltip("移動経過時間")]
    float elapsedTime;
    [Tooltip("移動にかかる時間")]
    const float DURATION = 0.3f;
    [Tooltip("移動フラグ")]
    public bool isMoving {  get; private set; }


    /// <summary>
    /// 初期化処理
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
    /// カーソル移動開始時の処理
    /// </summary>
    /// <param name="_tPos">目標地点</param>
    public void MoveStart(Vector3 _tPos)
    {
        isMoving = true;
        startPosition = transform.position;
        targetPos = _tPos;
        elapsedTime = 0;
    }


    /// <summary>
    /// 移動処理.目標地点までなめらかに動く
    /// </summary>
    void Move()
    {
        // 経過時間
        elapsedTime += Time.deltaTime;

        // 移動の進行度
        var progress = Mathf.Clamp01(elapsedTime / DURATION);

        // イージング処理
        float smooth = Mathf.SmoothStep(0, 1, progress);

        // 徐々に移動
        transform.position = Vector3.Lerp(startPosition, targetPos, smooth);

        // 進行度が1で完了
        if(progress >= 1)
        {
            // 念のためずれを補正して終了
            transform.position = targetPos;
            isMoving = false;
        }
    }
}
