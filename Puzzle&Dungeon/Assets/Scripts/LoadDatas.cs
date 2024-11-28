using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Data
{
    public class LoadData
    {
        const string PREFIX_LOAD_DATA = "LoadDatas/";
        const string STAGE_SELECT_PATH = "StageSelectData";
        const string STAGE_DATA_PATH = "StageData";
        const string ENEMY_PATH = "EnemyData";



        public static void TestFunc()
        {
            //var jsonStageSelectData = LoadStageSelectData();
            //var jsonStageData = LoadStageData(jsonStageSelectData.stage_select_datas[0].stage_id);
            //var jsonEnemyData = LoadEnemyData();

            //Debug.Log(JsonUtility.ToJson(jsonStageSelectData));
            //Debug.Log(JsonUtility.ToJson(jsonStageData));
            //Debug.Log(JsonUtility.ToJson(jsonEnemyData));
        }







        /// <summary>
        /// ���\�[�X����e�L�X�g��ǂݍ���ŔC�ӂ̌^�ɃV���A���C�Y���ĕԂ��֐�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_path"></param>
        /// <returns></returns>
        static T LoadAssetData<T>(string _path) where T : class
        {
            var tmp = Resources.Load<TextAsset>(PREFIX_LOAD_DATA + _path);
            var json = JsonUtility.FromJson<T>(tmp.text);
            if (json != null)
            {
                return json;
            }
            return null;
        }


        /// <summary>
        /// �C�ӂ̃X�e�[�W�̃f�[�^��ǂݍ���
        /// </summary>
        /// <param name="_stageId"></param>
        /// <returns>�Ȃ�������null</returns>
        static JsonStageData LoadStageData(int _stageId)
        {
            var tmp = LoadAssetData<JsonStageMaster>(STAGE_DATA_PATH);
            return tmp.GetStageData(_stageId);
        }



        /// <summary>
        /// �X�e�[�W�������ׂēǂݍ���
        /// </summary>
        /// <returns></returns>
        public static JsonStageMaster LoadAllStageData()
        {
            return LoadAssetData<JsonStageMaster>(STAGE_DATA_PATH);
        }

        /// <summary>
        /// �G�������ׂēǂݍ���
        /// </summary>
        /// <returns></returns>
        public static JsonEnemyMaster LoadAllEnemyData()
        {
            return LoadAssetData<JsonEnemyMaster>(ENEMY_PATH);
        }



        /// <summary>
        /// �X�e�[�W�ɓo�ꂷ��G�̃f�[�^�������o��
        /// </summary>
        /// <param name="_stageId">�X�e�[�W��ID</param>
        /// <returns></returns>
        static JsonEnemyData[] LoadEnemyDatas(int _stageId)
        {
            // �G�̑S�f�[�^�擾
            var enemyMaster = LoadAssetData<JsonEnemyMaster>(ENEMY_PATH);
            // �X�e�[�W�̃f�[�^�擾
            var stageData = LoadStageData(_stageId);
            //Debug.Log("�o�Ă���G�ꗗ");
            foreach (var item in stageData.enemy_datas)
            {
                //Debug.Log($"id:{item.enemy_id}");
            }
            // �ԋp�l�̏���
            var ret = new JsonEnemyData[stageData.enemy_datas.Length];
            // �X�e�[�W�ɕK�v�ȃf�[�^�������o��
            for (int i = 0; i < stageData.enemy_datas.Length; i++)
            {
                if (stageData.enemy_datas[i].enemy_id != Common.Common.ERROR_ID)
                {
                    // �G�f�[�^�擾
                    ret[i] = enemyMaster.GetEnemyData(stageData.enemy_datas[i].enemy_id);

                    if (ret[i] == null)
                    {
                        Debug.Log($"{stageData.enemy_datas[i].enemy_id}�͓G�f�[�^���Ȃ�");
                        return null;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// �Q�[���J�n���ɕK�v�ȃf�[�^���擾
        /// </summary>
        /// <param name="_stageId"></param>
        /// <returns></returns>
        public static JsonGameData GetGameData(int _stageId)
        {
            var ret = new JsonGameData();
            ret.stage_data = LoadStageData(_stageId);
            ret.enemy_datas = LoadEnemyDatas(_stageId);

            if(ret.stage_data != null && ret.enemy_datas != null)
            {
                return ret;
            }

            return null;
        }
    }



    #region Json�N���X

    /// <summary>
    /// �X�e�[�W�ǂݍ��ݎ��Ɏ擾����f�[�^
    /// </summary>
    [Serializable]
    public class JsonStageMaster
    {
        public JsonStageData[] stage_datas;

        public JsonStageMaster()
        {
            stage_datas = new JsonStageData[Common.Common.STAGE_NUM];
        }

        /// <summary>
        /// �C�ӃX�e�[�W��Ԃ�
        /// </summary>
        /// <param name="_stageId"></param>
        /// <returns>�Ȃ����null</returns>
        public JsonStageData GetStageData(int _stageId)
        {
            for(int i = 0; i < stage_datas.Length; i++)
            {
                if(_stageId == stage_datas[i].stage_id)
                {
                    //Debug.Log("�X�e�[�W�ǂݍ��ݐ���");
                    return stage_datas[i];
                }
            }
            Debug.Log("�X�e�[�W�ǂݍ��ݎ��s");
            return null;
        }
    }

    [Serializable]
    public class JsonStageData
    {
        public int stage_id;
        public string stage_name;
        public string stage_id_name;
        public string stage_desc;
        public int floor_width;
        public int floor_height;
        /// <summary>�G���A�������̃G���A�̍ŏ���</summary>
        public int min_division;
        /// <summary>�G�̍ő吔</summary>
        public int max_enemy_num;

        public int enemy_id_1;
        public int enemy_id_2;
        public JsonStageEnemyData[] enemy_datas;

        public JsonStageData()
        {
            enemy_datas = new JsonStageEnemyData[Common.Common.ENEMY_NUM];
            stage_id = Common.Common.ERROR_ID;
        }
    }

    [Serializable]
    public class JsonStageEnemyData
    {
        public int enemy_id;
        public float spawn_rate;

        public JsonStageEnemyData()
        {
            enemy_id = Common.Common.ERROR_ID;
        }
    }

    [Serializable]
    public class JsonEnemyMaster
    {
        public JsonEnemyData[] enemy_datas;

        public JsonEnemyMaster()
        {
            enemy_datas = new JsonEnemyData[Common.Common.ENEMY_NUM];
        }

        /// <summary>
        /// �G�f�[�^�擾�p�̏���
        /// </summary>
        /// <param name="_enemyId">�C�ӂ̓G�f�[�^</param>
        /// <returns>�}�X�^�[�����݂��Ȃ�������null�A��������f�[�^</returns>
        public JsonEnemyData GetEnemyData(int _enemyId)
        {
            for (int i = 0; i < enemy_datas.Length; i++)
            {
                if (enemy_datas[i].enemy_id == _enemyId)
                {
                    return enemy_datas[i];
                }
            }
            Debug.Log("�G�f�[�^������܂���ł���");
            return null;
        }
    }

    [Serializable]
    public class JsonEnemyData
    {
        public int enemy_id;
        public string enemy_name;
        public string enemy_id_name;
        public int hp;
        public int atk;
        public int pattern;

        public JsonEnemyData()
        {
            enemy_id = Common.Common.ERROR_ID;
        }
    }

    /// <summary>
    /// �Q�[���J�n���ɕK�v�ȃf�[�^
    /// </summary>
    [Serializable]
    public class JsonGameData
    {
        public JsonStageData stage_data;
        public JsonEnemyData[] enemy_datas;

        public JsonGameData()
        {
            stage_data = new JsonStageData();
            enemy_datas = new JsonEnemyData[Common.Common.ENEMY_NUM];
        }
    }


    #endregion

}



