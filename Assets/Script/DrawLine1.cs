using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class DrawLine1 : MonoBehaviour
    {
        LineRenderer renderer;
        Vector3 startPos;
        Vector3 pos;
        List<Vector3> rendererPositions = new List<Vector3>();


        bool sts = false;

        void Start()
        {
            renderer = gameObject.GetComponent<LineRenderer>();
            // 線の幅
            renderer.startWidth = 0.1f;
            renderer.endWidth = 0.1f;
            //色
            Color colorStrat;
            ColorUtility.TryParseHtmlString("#" + "FF0000", out colorStrat);
            renderer.startColor = colorStrat;
            Color colorEnd;
            ColorUtility.TryParseHtmlString("#" + "FF0000", out colorEnd);
            renderer.endColor = colorEnd;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //タッチ位置取得
                Vector3 cameraPosition = Input.mousePosition;
                cameraPosition.z = 10.0f;
                startPos = Camera.main.ScreenToWorldPoint(cameraPosition);

                //ラインの起点設定
                renderer.SetPosition(0, startPos);
            }
            else if (Input.GetMouseButton(0))
            {
                //タッチ位置取得
                Vector3 cameraPosition = Input.mousePosition;
                cameraPosition.z = 10.0f;
                pos = Camera.main.ScreenToWorldPoint(cameraPosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Debug.Log(rendererPositions.Count);
                renderer.positionCount = 0;
                rendererPositions.Clear();
            }
            // ラインレンダラーに座標を設定し線を描画
            if (!rendererPositions.Contains(pos))
            {
                rendererPositions.Add(pos);
                renderer.positionCount = rendererPositions.Count;
                renderer.SetPosition(renderer.positionCount - 1, pos);
            }
        }
    }
}