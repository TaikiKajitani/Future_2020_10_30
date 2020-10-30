using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Kajitani
{
    [CanEditMultipleObjects]
    //Planetのカスタムエディタ(inspectorの表示に関わる)
    [CustomEditor(typeof(UVauto))]
    public class UVautoEditer : Editor
    {
        UVauto obj = null;

        void OnEnable()
        {
            // AnyClassNameコンポーネントを取得
            obj = target as UVauto;
        }

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();
            if (GUILayout.Button("作成"))
            {
                obj.SetUV();
            }

        }
    }
}