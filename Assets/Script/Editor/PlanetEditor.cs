using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Kajitani
{
    [CanEditMultipleObjects]
    //Planetのカスタムエディタ(inspectorの表示に関わる)
    [CustomEditor(typeof(glass_Creater))]
    public class PlanetEditor : Editor
    {
        glass_Creater obj = null;

        void OnEnable()
        {
            // AnyClassNameコンポーネントを取得
            obj = target as glass_Creater;
        }

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();
            if (GUILayout.Button("メッシュを作成"))
            {
                obj.MeshCreate();
            }

            if (GUILayout.Button("線を描画"))
            {

                obj.DrawLines();
                // EditorExWindow window = EditorWindow.GetWindow<EditorExWindow>("EditorEx");
                //window.glass = obj;
            }

        }

        //// メニューのWindowにEditorExという項目を追加。
        //[MenuItem("Window/EditorEx")]
        //static void Open()
        //{
        //    // メニューのWindow/EditorExを選択するとOpen()が呼ばれる。
        //    // 表示させたいウィンドウは基本的にGetWindow()で表示＆取得する。
        //    EditorWindow.GetWindow<EditorWindow>("EditorEx"); // タイトル名を"EditorEx"に指定（後からでも変えられるけど）
        //}


        //// Windowのクライアント領域のGUI処理を記述
        //void OnGUI()
        //{
        //    // 試しにラベルを表示
        //    EditorGUILayout.LabelField("ようこそ！　Unityエディタ拡張の沼へ！");
        //}
    }
}