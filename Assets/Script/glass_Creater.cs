using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class glass_Creater : MonoBehaviour
    {
        MeshFilter filter;
        MeshRenderer renderer;

        public Vector2 depth = new Vector2(1.0f, -1.0f);

        public Shape[] shapes;

        public Color color1 = Color.white;
        public Color color2 = Color.black;

        public void MeshCreate()
        {
            filter = GetComponent<MeshFilter>();
            renderer = GetComponent<MeshRenderer>();


            GameObject child = new GameObject("child");
            child.transform.position = transform.position;
            child.transform.parent = transform;

            Mesh mesh;


            //頂点
            List<Vector3> vector3 = new List<Vector3>();
            //法線
            Vector3 normal;
            List<int> index = new List<int>();
            List<Color> colos = new List<Color>();

            int count = 0;

            for (int i = 0; i < shapes.Length; i++)
            {
                mesh = new Mesh();
                mesh.name = "neoMesh";
                vector3.Clear();

                if (Vector3.Dot(new Vector3(0, 0, depth.x - depth.y),
                    Vector3.Cross(shapes[i].lines[1] - shapes[i].lines[0], shapes[i].lines[2] - shapes[i].lines[0]))
                    >= 0.0f)
                {
                    //面1
                    for (int j = 0; j < shapes[i].lines.Length; j++)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.x));
                    }

                    for (int j = 0, k = 1; j < shapes[i].lines.Length; j++, k = (k + 1) % shapes[i].lines.Length)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.x));
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.y));
                        vector3.Add(new Vector3(shapes[i].lines[k].x, shapes[i].lines[k].y, depth.x));
                        vector3.Add(new Vector3(shapes[i].lines[k].x, shapes[i].lines[k].y, depth.y));
                    }

                    for (int j = 0; j < shapes[i].lines.Length; j++)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.y));
                    }
                }
                else
                {
                    //面1
                    for (int j = shapes[i].lines.Length - 1; j >= 0; j--)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.x));
                    }

                    // for (int j = 0, k = 1; j < shapes[i].lines.Length; j++, k = (k + 1) % shapes[i].lines.Length)

                    for (int j = shapes[i].lines.Length - 1, k = j - 1; j >= 0; j--, k = (k + shapes[i].lines.Length - 1) % shapes[i].lines.Length)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.x));
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.y));
                        vector3.Add(new Vector3(shapes[i].lines[k].x, shapes[i].lines[k].y, depth.x));
                        vector3.Add(new Vector3(shapes[i].lines[k].x, shapes[i].lines[k].y, depth.y));
                    }

                    for (int j = shapes[i].lines.Length - 1; j >= 0; j--)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.y));
                    }
                }
                Vector3 center = Vector3.zero;

                for (int j = 0; j < vector3.Count; j++)
                {
                    center += vector3[i];
                }
                center *= 1.0f / vector3.Count;

                for (int j = 0; j < vector3.Count; j++)
                {

                    vector3[j] -= center;
                    //vector3[j] *= 0.99f;
                }

                mesh.vertices = vector3.ToArray();

                //法線
                vector3.Clear();

                normal = Vector3.forward;
                //面1 
                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    vector3.Add(normal);
                }
                count = shapes[i].lines.Length;

                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    normal = Vector3.Cross(mesh.vertices[count + 1] - mesh.vertices[count], mesh.vertices[count + 2] - mesh.vertices[count]);
                    normal.Normalize();
                    vector3.Add(normal);
                    vector3.Add(normal);
                    vector3.Add(normal);
                    vector3.Add(normal);
                    count += 4;
                }
                normal = Vector3.back;
                //面1 
                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    vector3.Add(normal);
                }


                mesh.normals = vector3.ToArray();

                //色
                colos.Clear();

                //面1 
                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    colos.Add(color1);
                }
                count = shapes[i].lines.Length;

                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    colos.Add(color2);
                    colos.Add(color2);
                    colos.Add(color2);
                    colos.Add(color2);
                }
                normal = Vector3.back;
                //面1 
                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    colos.Add(color1);
                }


                mesh.colors = colos.ToArray();


                //インデックス
                index.Clear();
                count = 0;
                for (int j = 0; j < shapes[i].lines.Length - 2; j++)
                {
                    index.Add(0);
                    index.Add(j + 1);
                    index.Add(j + 2);

                }

                count = shapes[i].lines.Length;

                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    index.Add(count);
                    index.Add(count + 1);
                    index.Add(count + 2);
                    index.Add(count + 3);
                    index.Add(count + 2);
                    index.Add(count + 1);

                    count += 4;
                }

                for (int j = shapes[i].lines.Length - 1; j >= 2; j--)
                {
                    index.Add(count + shapes[i].lines.Length - 1);
                    index.Add(count + j - 1);
                    index.Add(count + j - 2);
                }


                Debug.Log(index.Count);
                mesh.triangles = index.ToArray();

                //filter.mesh = mesh;
                GameObject obj = new GameObject("glass_" + i.ToString());

                obj.transform.position = transform.position + center;

                MeshFilter nfilter = obj.AddComponent<MeshFilter>();
                nfilter.mesh = mesh;

                Rigidbody nrigidbody = obj.AddComponent<Rigidbody>();
                nrigidbody.useGravity = true;
                obj.transform.parent = child.transform;


                MeshCollider ncollider = obj.AddComponent<MeshCollider>();
                ncollider.convex = true;
                ncollider.sharedMesh = mesh;

                MeshRenderer nrenderer = obj.AddComponent<MeshRenderer>();
                nrenderer.sharedMaterial = renderer.material;
            }

        }
        public void MeshCreateOri()
        {
            filter = GetComponent<MeshFilter>();
            Mesh mesh = new Mesh();

            mesh.name = "neoMesh";

            //頂点
            List<Vector3> vector3 = new List<Vector3>();

            for (int i = 0; i < shapes.Length; i++)
            {
                if (Vector3.Dot(new Vector3(0, 0, depth.x - depth.y),
                    Vector3.Cross(shapes[i].lines[1] - shapes[i].lines[0], shapes[i].lines[2] - shapes[i].lines[0]))
                    >= 0.0f)
                {
                    //面1
                    for (int j = 0; j < shapes[i].lines.Length; j++)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.x));
                    }

                    for (int j = 0, k = 1; j < shapes[i].lines.Length; j++, k = (k + 1) % shapes[i].lines.Length)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.x));
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.y));
                        vector3.Add(new Vector3(shapes[i].lines[k].x, shapes[i].lines[k].y, depth.x));
                        vector3.Add(new Vector3(shapes[i].lines[k].x, shapes[i].lines[k].y, depth.y));
                    }

                    for (int j = 0; j < shapes[i].lines.Length; j++)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.y));
                    }
                }
                else
                {
                    //面1
                    for (int j = shapes[i].lines.Length - 1; j >= 0; j--)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.x));
                    }

                    // for (int j = 0, k = 1; j < shapes[i].lines.Length; j++, k = (k + 1) % shapes[i].lines.Length)

                    for (int j = shapes[i].lines.Length - 1, k = j - 1; j >= 0; j--, k = (k + shapes[i].lines.Length - 1) % shapes[i].lines.Length)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.x));
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.y));
                        vector3.Add(new Vector3(shapes[i].lines[k].x, shapes[i].lines[k].y, depth.x));
                        vector3.Add(new Vector3(shapes[i].lines[k].x, shapes[i].lines[k].y, depth.y));
                    }

                    for (int j = shapes[i].lines.Length - 1; j >= 0; j--)
                    {
                        vector3.Add(new Vector3(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.y));
                    }
                }
            }

            mesh.vertices = vector3.ToArray();
            //法線
            int count = 0;
            Vector3 normal;

            vector3.Clear();
            for (int i = 0; i < shapes.Length; i++)
            {
                normal = Vector3.forward;
                //面1 
                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    vector3.Add(normal);
                }

                count += shapes[i].lines.Length;

                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    normal = Vector3.Cross(mesh.vertices[count + 1] - mesh.vertices[count], mesh.vertices[count + 2] - mesh.vertices[count]);
                    vector3.Add(normal);
                    vector3.Add(normal);
                    vector3.Add(normal);
                    vector3.Add(normal);
                    count += 4;
                }
                normal = Vector3.back;
                //面1 
                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    vector3.Add(normal);
                }

                count += shapes[i].lines.Length;

            }
            mesh.normals = vector3.ToArray();
            //インデックス
            count = 0;
            List<int> index = new List<int>();
            for (int i = 0; i < shapes.Length; i++)
            {
                for (int j = 0; j < shapes[i].lines.Length - 2; j++)
                {
                    index.Add(count);
                    index.Add(count + j + 1);
                    index.Add(count + j + 2);
                }
                count += shapes[i].lines.Length;

                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    index.Add(count);
                    index.Add(count + 1);
                    index.Add(count + 2);
                    index.Add(count + 3);
                    index.Add(count + 2);
                    index.Add(count + 1);
                    count += 4;
                }

                for (int j = shapes[i].lines.Length - 1; j >= 2; j--)
                {
                    index.Add(count + shapes[i].lines.Length - 1);
                    index.Add(count + j - 1);
                    index.Add(count + j - 2);
                }
                count += shapes[i].lines.Length;
            }

            Debug.Log(count);
            mesh.triangles = index.ToArray();

            filter.mesh = mesh;
        }
        public void DrawLines()
        {
            List<Vector3> fPoints = new List<Vector3>();
            List<Vector3> bPoints = new List<Vector3>();

            for (int i = 0; i < shapes.Length; i++)
            {
                for (int j = 0; j < shapes[i].lines.Length; j++)
                {
                    fPoints.Add(transform.TransformPoint(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.x));
                    bPoints.Add(transform.TransformPoint(shapes[i].lines[j].x, shapes[i].lines[j].y, depth.y));
                }
                //面1
                for (int j = 0, k = 1; j < shapes[i].lines.Length; j++, k = (k + 1) % shapes[i].lines.Length)
                {
                    Debug.DrawLine(fPoints[j], fPoints[k], Color.red);
                    Debug.DrawLine(fPoints[j], bPoints[j], Color.red + Color.blue);
                    Debug.DrawLine(bPoints[j], bPoints[k], Color.blue);
                }
                fPoints.Clear();
                bPoints.Clear();
            }
        }
    }
}