using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    [Tooltip("スティック"), SerializeField]
    private InputAction inputMover;
    [Tooltip("歩く速度"), SerializeField]
    private float speed = 0.1f;

    /// <summary>
    ///オブジェクトがアクティブになった時に呼ばれるイベント
    /// </summary>
    private void OnDisable()
    {
        inputMover.Disable();
    }

    /// <summary>
    /// オブジェクトが非表示になった時に呼ばれるイベント
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
        //スティックのベクトルを取得
        var moveVec = inputMover.ReadValue<Vector2>();

        //プレイヤを移動
        transform.position += new Vector3(
            moveVec.x * speed,
            moveVec.y * speed,
            0);
    }
}
