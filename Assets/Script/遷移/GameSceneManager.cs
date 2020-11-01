using UnityEngine;
using UnityEngine.SceneManagement;


namespace Kajitani
{
    //ゲームシーンの遷移を管理する
    public class GameSceneManager : MonoBehaviour
    {
        static GameSceneManager myInstance = null;

        [Tooltip("フェードアウト・インするオブジェクト")]
        public Fade fadeObject;

        //シーンが変わるまでのフレーム数(ポーズしている可能性があるので固定フレームで)
        public float changeFrame =1;

        float start_time = 0;

        //セレクトシーンから変わるシーンの名前
        string scnenName = "";

        static public GameSceneManager GetInstance()
        {
            return myInstance;
        }

        public enum ChangeSceneName
        {
            e_TitleScene,//タイトルに戻る
            e_SelectScene,//セレクトに戻る
            e_NextScene,//次のシーンへ
            e_ReStart,//現在のシーンを最初から
            e_SelectToGame,//セレクトシーンからゲームシーンへ
            e_Null
        }
        //変わるのが確定したシーン
        private ChangeSceneName csn = ChangeSceneName.e_Null;

        public bool ChangeScene(ChangeSceneName l_csn)
        {
            //まだ遷移が確定していなければ
            if (csn == ChangeSceneName.e_Null)
            {
                //ポーズをやめる
                Time.timeScale = 1.0f;
                //フェードアウトさせる
                if (fadeObject)
                {
                    fadeObject.SetFadeOut(changeFrame);
                }
                //遷移を確定する
                csn = l_csn;
                //1秒後に遷移する
                Invoke("GoScene",1);
                return true;
            }
            return false;
        }
        //セレクトシーンからゲームシーンに遷移する
        //public bool ChangeScene(int world, int stage)
        //{
        //    //まだ遷移が確定していなければ
        //    if (csn == ChangeSceneName.e_Null)
        //    {
        //        scnenName = SceneNames.SceneNumberToName(new Vector2Int(world, stage));
        //        //番号に対応するシーンがなければ遷移させない
        //        if (scnenName == "null")
        //        {
        //            return false;
        //        }
        //        //フェードアウトさせる
        //        if (fadeObject)
        //        {
        //            fadeObject.SetFadeOut(changeFrame);
        //        }
        //        //遷移を確定する
        //        csn = ChangeSceneName.e_SelectToGame;
        //        return true;
        //    }
        //    return false;
        //}
        //確定したシーンに遷移する
        private void GoScene()
        {
            //bgmとseをすべて止める
            //if (SoundManager.Instance)
            //{
            //    SoundManager.Instance.StopBgm();
            //    SoundManager.Instance.StopSe();
            //}
            string str;
            switch (csn)
            {
                case ChangeSceneName.e_TitleScene:
                    //タイトルシーンに遷移する
                    SceneManager.LoadScene(SceneNames.titleScene);
                    break;
                case ChangeSceneName.e_SelectScene:
                    //セレクトシーンに遷移する
                    SceneManager.LoadScene(SceneNames.SelectScene);
                    break;
                //シーンの再開
                case ChangeSceneName.e_ReStart:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                ////次のシーンに遷移する
                //case ChangeSceneName.e_NextScene:
                //    str = GetNextStage(SceneManager.GetActiveScene().name);
                //    if (str != "null")
                //    {
                //        SceneManager.LoadScene(str);
                //        break;
                //    }
                //    //次のシーンがなければセレクトシーンに遷移する
                //    SceneManager.LoadScene(SceneNames.SelectScene);
                //    break;
                //遷移したいシーンが存在していれば
                case ChangeSceneName.e_SelectToGame:
                    SceneManager.LoadScene(scnenName);
                    break;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            myInstance = this;

            //フェードインさせる
            if (fadeObject)
            {
                fadeObject.SetFadeIn(changeFrame);
            }
        }
        
        //タイトルに遷移する
        public void ChangeTitle()
        {
            ChangeScene(ChangeSceneName.e_TitleScene);
        }
        //セレクトに遷移する
        public void ChangeSelect()
        {
            ChangeScene(ChangeSceneName.e_SelectScene);
        }
        //次のシーンに遷移する
        public void ChangeNext()
        {
            ChangeScene(ChangeSceneName.e_NextScene);
        }
        //このシーンを最初からやり直す
        public void RestartScene()
        {
            ChangeScene(ChangeSceneName.e_ReStart);
        }
        //シーンの遷移が確定していなければtrue
        public bool noChange()
        {
            return csn == ChangeSceneName.e_Null;
        }
        ////次のシーン名を取得
        ////引数 調べたいシーン名
        ////返り値　調べたい次のシーン名
        ////なければnullが帰ってくる
        //public static string GetNextStage(string now_scene)
        //{
        //    for (int i = 0; i < SceneNames.sceneNames.Length; i++)
        //    {
        //        for (int j = 0; j < SceneNames.sceneNames[i].Length - 1; j++)
        //        {
        //            //次のシーン名があったら
        //            if (now_scene == SceneNames.sceneNames[i][j])
        //            {
        //                return SceneNames.sceneNames[i][j + 1];
        //            }
        //        }
        //        //そのワールドの最後なら
        //        if (now_scene == SceneNames.sceneNames[i][SceneNames.sceneNames[i].Length - 1])
        //        {
        //            //次のステージの1へ
        //            //次のワールドが存在し
        //            if (i + 1 < SceneNames.sceneNames.Length)
        //            {
        //                //次のワールドにシーンがあれば
        //                if (SceneNames.sceneNames[i + 1].Length != 0)
        //                {
        //                    return SceneNames.sceneNames[i + 1][0];
        //                }
        //            }
        //        }
        //    }
        //    return "null";
        //}
    }
}