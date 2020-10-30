using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Kajitani
{
    [CanEditMultipleObjects]
    //Planetのカスタムエディタ(inspectorの表示に関わる)
    [CustomEditor(typeof(StairsCreate))]
    public class StairsEditer : Editor
    {
        StairsCreate obj = null;

        void OnEnable()
        {
            // AnyClassNameコンポーネントを取得
            obj = target as StairsCreate;
        }

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();
            if (GUILayout.Button("作成"))
            {
                obj.Create();
            }

        }
    }
}