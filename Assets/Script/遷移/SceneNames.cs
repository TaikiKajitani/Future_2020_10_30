using UnityEngine;

//全てのシーン名をここから参照できる用にしておく
static public class SceneNames
{
    //タイトルシーン
    public static string titleScene = "Start";
    //タイトルシーン
    public static string SelectScene = "SSD";//"SelectScene";
    //ステージ名
    public static string[][] sceneNames = { new string[]{ "Stage_1_1", "Stage_1_2", "Stage_1_3"},
        new string[]{ "Stage_2_1", "Stage_2_2", "Stage_2_3", "Stage_2_4", "Stage_2_5", "Stage_2_6" },
        new string[]{ "Stage_3_1", "Stage_3_2", "Stage_3_3", "Stage_3_4", "Stage_3_5", "Stage_3_6" },
        new string[]{ "Stage_4_1", "Stage_4_2", "Stage_4_3", "Stage_4_4", "Stage_4_5", "Stage_4_6" },
        new string[]{ "Stage_5_1", "Stage_5_2", "Stage_5_3", "Stage_5_4", "Stage_5_5", "Stage_5_6" },
        new string[]{ "Stage_6_1", "Stage_6_2", "Stage_6_3", "Stage_6_4", "Stage_6_5", "Stage_6_6" },
    };
    //番号からシーン名に直す
    public static string SceneNumberToName(Vector2Int stage)
    {
        //番号のシーンがないなら遷移させない
        if (stage.x >= sceneNames.Length)
        {
            return "null";
        }
        if (stage.y >= sceneNames[stage.x].Length)
        {
            return "null";
        }
        return sceneNames[stage.x][stage.y];
    }
    //シーン名から番号に直す（ステージx-y）
    public static Vector2Int SceneNameToNumber(string name)
    {
        for (int i = 0; i < sceneNames.Length; i++)
        {
            for (int j = 0; j < sceneNames[i].Length; j++)
            {
                if (sceneNames[i][j] == name)
                {
                    return new Vector2Int(i, j);
                }
            }
        }
        return new Vector2Int(10000, 10000);
    }
    //シーン名から番号に直す(ステージ1からの番号)
    public static int SceneNameToNumber1(string name)
    {
        int k = 0;
        for (int i = 0; i < sceneNames.Length; i++)
        {
            for (int j = 0; j < sceneNames[i].Length; j++)
            {
                if (sceneNames[i][j] == name)
                {
                    return k;
                }
                k++;
            }
        }
        return 10000;
    }
}
