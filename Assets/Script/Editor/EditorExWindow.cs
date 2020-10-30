using UnityEngine;
using UnityEditor; // エディタ拡張関連はUnityEditor名前空間に定義されているのでusingしておく。
using System.Collections;
using System.Linq;


namespace Kajitani
{
    // エディタに独自のウィンドウを作成する
    public class EditorExWindow : EditorWindow
    {
        public glass_Creater glass;
        // メニューのWindowにEditorExという項目を追加。
        [MenuItem("Window/EditorEx")]
        static void Open()
        {
            // メニューのWindow/EditorExを選択するとOpen()が呼ばれる。
            // 表示させたいウィンドウは基本的にGetWindow()で表示＆取得する。
            EditorWindow.GetWindow<EditorExWindow>("EditorEx"); // タイトル名を"EditorEx"に指定（後からでも変えられるけど）
        }

        // Windowのクライアント領域のGUI処理を記述
        void OnGUI()
        {
            // 試しにラベルを表示
            EditorGUILayout.LabelField("ようこそ！　Unityエディタ拡張の沼へ！");
            //Graphics.DrawLine.DrawLine(pointA, pointB, color, width);


            //GUI.li

            ////LineRenderer lineRenderer;
            //// Lineオブジェクトの生成
            //GameObject beam = Instantiate(BeamPrefab,
            //                    new Vector3(0, 0, 0),
            //                    Quaternion.identity) as GameObject;

            //LineRenderer lineRenderer;

            //lineRenderer.st

            //EditorGUILayout.Separator;

        }
    }
}