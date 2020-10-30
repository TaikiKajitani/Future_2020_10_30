using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Kajitani
{
    //線分
    [System.Serializable]
    public struct Line
    {
        public Vector2 startPoint;
        public Vector2 endPoint;
        //public Line()
        //{
        //    startPoint = Vector2.zero;
        //    endPoint =Vector2.zero;
        //}
        public Line(Vector2 s, Vector2 e)
        {
            startPoint = s;
            endPoint = e;
        }
    }

    [System.Serializable]
    public struct Shape
    {
        public Vector2[] lines;
    }

    //線を引いてそこから出来上がる図形を抽出する
    public class ShapeSearch : MonoBehaviour
    {
        public Line[] points1 = { new Line(new Vector2(-100, -100), new Vector2(100, -100)),
        new Line(new Vector2(100, -100), new Vector2(100, 100)),
        new Line(new Vector2(100, 100), new Vector2(100, -100)),
        new Line(new Vector2(100, -100), new Vector2(-100, -100)) };

        struct MyLine
        {
            public Line point;
            public Vector2[] crossPoints;
            public int[] crossLineIndex;
        }

        List<MyLine> myLines = new List<MyLine>();

        //public Vector2[] points2; 
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < points1.Length; i++)
            {
                Debug.DrawLine(points1[i].startPoint, points1[i].endPoint, Color.red * i / points1.Length + Color.blue, 100);
                MyLine myLine = new MyLine();
                myLine.point = points1[i];
                myLines.Add(myLine);
            }
            Vector2 crossPoint;
            //交差している線の番号を記録
            List<int> indexs = new List<int>();
            List<Vector2> points = new List<Vector2>();
            for (int i = 0; i < myLines.Count; i++)
            {
                for (int j = 0; j < myLines.Count; j++)
                {
                    if (CrossLine(myLines[i].point, myLines[j].point, out crossPoint))
                    {
                        points.Add(crossPoint);
                        indexs.Add(j);
                    }
                }
                MyLine cLine = myLines[i];
                cLine.crossLineIndex = indexs.ToArray();
                cLine.crossPoints = points.ToArray();
                myLines[i] = cLine;
                indexs.Clear();
                points.Clear();
            }
            //図形を探す
            List<Line> l_lines = new List<Line>();
            MyLine myLine1;
            List<int> ins = new List<int>();
            List<Vector2> open = new List<Vector2>();
            //線を始点とする点を探す
            for (int i = 0; i < myLines.Count - 2; i++)
            {
                //交差点ごとに調べる
                for (int j = 0; j < myLines[i].crossLineIndex.Length; j++)
                {
                    myLine1 = myLines[myLines[i].crossLineIndex[j]];
                    //Debug.Log(myLines[i].crossLineIndex[j]);
                    if (Shach(myLines[i].crossLineIndex[j], -1, ins, myLines[i].crossLineIndex[j], out open))
                    {
                        string str = "";
                        for (int k = 0; k < open.Count; k++)
                        {
                            str += k.ToString() + ":" + open[k].ToString();
                        }
                        Debug.Log(str);
                    }
                    //points.Add(myLines[i].point[j]);
                }
                ins.Add(i);
            }
        }
        bool Shach(int goLine, int old, List<int> lines, int hitLine, out List<Vector2> hackout)
        {
            //交差点ごとに調べる
            for (int i = 0; i < myLines[goLine].crossLineIndex.Length; i++)
            {
                if (myLines[goLine].crossLineIndex[i] != old)
                {
                    for (int j = 0; j < lines.Count; j++)
                    {
                        if (myLines[goLine].crossLineIndex[i] == lines[j])
                        {
                            //現在のノードを消す
                            hackout = null;
                            Debug.Log(lines[j]);
                            return false;
                            //新たな図形は発見できなかった
                        }
                    }
                    if (myLines[goLine].crossLineIndex[i] == hitLine)
                    {
                        hackout = new List<Vector2>();
                        hackout.Add(myLines[goLine].crossPoints[i]);
                        //発見して探索終了
                        return true;
                    }
                    Debug.Log(myLines[goLine].crossLineIndex[i]);
                    //すでに通った線を追加
                    lines.Add(myLines[goLine].crossLineIndex[i]);
                    if (Shach(myLines[goLine].crossLineIndex[i], goLine, lines, hitLine, out hackout))
                    {
                        hackout.Add(myLines[goLine].crossPoints[i]);
                        return true;
                    }
                }
            }
            hackout = null;
            return false;
        }

        public static float Vector2_Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }
        bool CrossLine(Line line1, Line line2, out Vector2 crossPoint)
        {
            Vector2 vec1 = line1.endPoint - line1.startPoint;
            Vector2 vec2 = line2.endPoint - line2.startPoint;
            //線が互いに平行なら交差しない
            if (vec1.x * vec2.y == vec1.y * vec2.x)
            {
                crossPoint = Vector2.zero;
                return false;
            }

            //交差点を求める
            Vector2 v = line2.startPoint - line1.startPoint;
            float Crs_v1_v2 = Vector2_Cross(vec1, vec2);
            float Crs_v_v2 = Vector2_Cross(v, vec2);
            float t1 = Crs_v_v2 / Crs_v1_v2;
            crossPoint = line1.startPoint + vec1 * t1;

            //その交差点が各線分の中にあるか調べる(どちらかの軸を調べればわかる)
            if (Mathf.Min(line1.startPoint.x, line1.endPoint.x) <= crossPoint.x &&
               Mathf.Max(line1.startPoint.x, line1.endPoint.x) >= crossPoint.x &&
               Mathf.Min(line2.startPoint.x, line2.endPoint.x) <= crossPoint.x &&
               Mathf.Max(line2.startPoint.x, line2.endPoint.x) >= crossPoint.x)
            {
                return true;
            }
            //交差点はどちらかの線分外
            return false;
        }
    }
}