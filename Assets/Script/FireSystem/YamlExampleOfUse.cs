using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class YamlExampleOfUse : MonoBehaviour
    {
        [System.Serializable]
        public struct Jdata
        {
            public int aaa;
            public string bbb;
        }
        public bool b = true;
        private void Start()
        {
            if (b) {
                Save();
                    }
            else
            {
                Load();
            }


        }
        //セーブの例
        bool Load()
        {

            YamlLoader yaml = new YamlLoader();
            //このファイルから読み込み
            yaml.Load("TestData");

            Jdata data;
            //このキーのデータを取得
            string str = yaml.GetMassage("key.data1");
            //読み込んだデータをJsonとして取得
            if (str != "")
            {
                try
                {
                    data = JsonUtility.FromJson<Jdata>(str);
                    Debug.Log(data.aaa);
                    Debug.Log(data.bbb);
                    return true;
                }
                catch
                {
                    return false;
                };
            }
            return false;
        }
        //ロードの例
        bool Save()
        {

            YamlWriter yaml = new YamlWriter();

            Jdata data;
            data.aaa = 432;
            data.bbb = "aaa";
            yaml.SetMassage("key.data1", JsonUtility.ToJson(data));
            //このファイルに書き込み
            yaml.Save("TestData");
            return true;
        }
    }
}