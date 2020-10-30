using System.IO;
using YamlDotNet;
using YamlDotNet.RepresentationModel;
using UnityEngine;

/*
 * yamlでデータを読み書きできる
 https://assetstore.unity.com/packages/tools/integration/yamldotnet-for-unity-36292
 これを入れないと使えない
     */

public class YamlLoader
{
    YamlStream yaml;
    //データの読み込み
   public bool Load(string str)
    {
        try
        {
            StreamReader inputFile = new StreamReader(Application.dataPath + "/" + str + ".yml", System.Text.Encoding.UTF8);
            yaml = new YamlStream();
            yaml.Load(inputFile);
            return true;
        }
        catch
        {
            yaml = null;
            return false;
        }
    }
    //キーのデータを取得
    public string GetMassage(string key)
    {
        if (yaml == null)
        {
            return "";
        }
        // キーをドットで分割
        string[] keys = key.Split('.');

        // キーの配列数(＝ネストレベル)取得
        int keyCount = keys.Length;

        // ルートのマッピング取得
        YamlMappingNode mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
        YamlScalarNode node = null;
        YamlScalarNode chack = null;
        for (int i = 0; i < keyCount; i++)
        {
            chack = new YamlScalarNode(keys[i]);

            //Debug.Log(keys[i]);
            if (mapping.Children.ContainsKey(chack))
            {
                // キー配列が最後の要素になった場合は ScalarNode を取得
                if (i == keyCount - 1)
                {
                    node = (YamlScalarNode)mapping.Children[chack];
                }
                else
                {
                    // キーを元に1つ深いネストのマッピングを取得
                    mapping = (YamlMappingNode)mapping.Children[chack];
                }
            }
            else
            {
                return "";
            }
        }
        return node.ToString();
    }
}

public class YamlWriter
{
    YamlStream yaml;
    public YamlWriter()
    {
        yaml = new YamlStream();
        YamlNode node = new YamlMappingNode();
        YamlDocument document = new YamlDocument(node);
        yaml.Add(document);
    }
    //セットされたデータを書き込み
    public bool Save(string str)
    {
        try
        {
            StreamWriter outputFile = new StreamWriter(Application.dataPath + "/" + str + ".yml");
            yaml.Save(outputFile);
            outputFile.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }
    //データをセット
    public void SetMassage(string key, string value)
    {
        // キーをドットで分割
        string[] keys = key.Split('.');

        // キーの配列数(＝ネストレベル)取得
        int keyCount = keys.Length;

        YamlMappingNode mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

        // ルートのマッピング取得
        YamlScalarNode node = null;
        YamlScalarNode chack = null;
        for (int i = 0; i < keyCount; i++)
        {
            chack = new YamlScalarNode(keys[i]);

            //まだ同名のキーがあれば
            if (mapping.Children.ContainsKey(chack))
            {
                // キー配列が最後の要素になった場合は ScalarNode を取得
                if (i == keyCount - 1)
                {
                    //データの上書き
                    mapping.Children[chack] = new YamlScalarNode(value);
                    node = (YamlScalarNode)mapping.Children[chack];
                }
                else
                {
                    // キーを元に1つ深いネストのマッピングを取得
                    mapping = (YamlMappingNode)mapping.Children[chack];
                }
            }
            else
            {
                // キー配列が最後の要素になった場合は ScalarNode を取得
                if (i == keyCount - 1)
                {
                    //新しいデータをを追加
                    mapping.Children[chack] = new YamlScalarNode(value);
                    node = (YamlScalarNode)mapping.Children[chack];
                }
                else
                {
                    // キーを元に1つ深いネストのマッピングを生成
                    mapping.Children[chack] = new YamlMappingNode();
                    mapping = (YamlMappingNode)mapping.Children[chack];
                }
            }
        }
    }
}