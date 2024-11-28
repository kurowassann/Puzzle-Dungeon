using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
	//メンバ変数
	[Tooltip("クリアパネル"), SerializeField]
	private GameObject mClearPanel;
	[Tooltip("ゲームオーバーパネル"),SerializeField]
	private GameObject mGameOverPanel;

	/// <summary>起動中フラグ</summary>
	private bool isActive;

	//メンバ関数
	/// <summary>クリアパネルをオープン</summary>
	public void OpenClear()
	{
		mClearPanel.SetActive(true);
		isActive = true;
	}

	//
	public void OpenOverPanel()
	{
		mGameOverPanel.SetActive(true);
		isActive = true;
	}


    // Start is called before the first frame update
    void Start()
    {
		isActive = false;
		mClearPanel.SetActive(false);
		mGameOverPanel.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
		//スペースでタイトルへ
		if (isActive) 
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				SceneManager.LoadScene("Title");
			}
		}
    }
}
