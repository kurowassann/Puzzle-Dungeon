using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
	//�����o�ϐ�
	[Tooltip("�N���A�p�l��"), SerializeField]
	private GameObject mClearPanel;
	[Tooltip("�Q�[���I�[�o�[�p�l��"),SerializeField]
	private GameObject mGameOverPanel;

	/// <summary>�N�����t���O</summary>
	private bool isActive;

	//�����o�֐�
	/// <summary>�N���A�p�l�����I�[�v��</summary>
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
		//�X�y�[�X�Ń^�C�g����
		if (isActive) 
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				SceneManager.LoadScene("Title");
			}
		}
    }
}
