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
        /// リソースからテキストを読み込んで任意の型にシリアライズして返す関数
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
        /// 任意のステージのデータを読み込む
        /// </summary>
        /// <param name="_stageId"></param>
        /// <returns>なかったらnull</returns>
        static JsonStageData LoadStageData(int _stageId)
        {
            var tmp = LoadAssetData<JsonStageMaster>(STAGE_DATA_PATH);
            return tmp.GetStageData(_stageId);
        }



        /// <summary>
        /// ステージ情報をすべて読み込む
        /// </summary>
        /// <returns></returns>
        public static JsonStageMaster LoadAllStageData()
        {
            return LoadAssetData<JsonStageMaster>(STAGE_DATA_PATH);
        }

        /// <summary>
        /// 敵情報をすべて読み込む
        /// </summary>
        /// <returns></returns>
        public static JsonEnemyMaster LoadAllEnemyData()
        {
            return LoadAssetData<JsonEnemyMaster>(ENEMY_PATH);
        }



        /// <summary>
        /// ステージに登場する敵のデータだけ取り出す
        /// </summary>
        /// <param name="_stageId">ステージのID</param>
        /// <returns></returns>
        static JsonEnemyData[] LoadEnemyDatas(int _stageId)
        {
            // 敵の全データ取得
            var enemyMaster = LoadAssetData<JsonEnemyMaster>(ENEMY_PATH);
            // ステージのデータ取得
            var stageData = LoadStageData(_stageId);
            //Debug.Log("出てくる敵一覧");
            foreach (var item in stageData.enemy_datas)
            {
                //Debug.Log($"id:{item.enemy_id}");
            }
            // 返却値の準備
            var ret = new JsonEnemyData[stageData.enemy_datas.Length];
            // ステージに必要なデータだけ取り出す
            for (int i = 0; i < stageData.enemy_datas.Length; i++)
            {
                if (stageData.enemy_datas[i].enemy_id != Common.Common.ERROR_ID)
                {
                    // 敵データ取得
                    ret[i] = enemyMaster.GetEnemyData(stageData.enemy_datas[i].enemy_id);

                    if (ret[i] == null)
                    {
                        Debug.Log($"{stageData.enemy_datas[i].enemy_id}は敵データがない");
                        return null;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// ゲーム開始時に必要なデータを取得
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



    #region Jsonクラス

    /// <summary>
    /// ステージ読み込み時に取得するデータ
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
        /// 任意ステージを返す
        /// </summary>
        /// <param name="_stageId"></param>
        /// <returns>なければnull</returns>
        public JsonStageData GetStageData(int _stageId)
        {
            for(int i = 0; i < stage_datas.Length; i++)
            {
                if(_stageId == stage_datas[i].stage_id)
                {
                    //Debug.Log("ステージ読み込み成功");
                    return stage_datas[i];
                }
            }
            Debug.Log("ステージ読み込み失敗");
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
        /// <summary>エリア分割時のエリアの最小幅</summary>
        public int min_division;
        /// <summary>敵の最大数</summary>
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
        /// 敵データ取得用の処理
        /// </summary>
        /// <param name="_enemyId">任意の敵データ</param>
        /// <returns>マスターが存在しなかったらnull、あったらデータ</returns>
        public JsonEnemyData GetEnemyData(int _enemyId)
        {
            for (int i = 0; i < enemy_datas.Length; i++)
            {
                if (enemy_datas[i].enemy_id == _enemyId)
                {
                    return enemy_datas[i];
                }
            }
            Debug.Log("敵データ見つかりませんでした");
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
    /// ゲーム開始時に必要なデータ
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



