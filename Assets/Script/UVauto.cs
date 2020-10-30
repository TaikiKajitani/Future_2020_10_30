using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class UVauto : MonoBehaviour
    {
        Vector2 Division(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }
        Vector3 Division(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        public enum Ziku
        {
            X, Y, Z
        }

        public Vector3 zikuX = new Vector3(1, 0, 0), zikuY = new Vector3(0, 1, 0);

        MeshFilter filter;
        // Start is called before the first frame update
       public void SetUV()
        {
            filter = GetComponent<MeshFilter>();
             Mesh cmesh = filter.sharedMesh;
            Mesh mesh = new Mesh();
            mesh.name = gameObject.name;
            mesh.vertices = cmesh.vertices;
            mesh.normals = cmesh.normals;
            mesh.triangles = cmesh.triangles;

            List<Vector2> uvs = new List<Vector2>();

            for (int i = 0; i < cmesh.uv.Length; i++)
            {
                uvs.Add(new Vector2(Vector3.Dot(transform.TransformPoint(mesh.vertices[i]), zikuX), Vector3.Dot(transform.TransformPoint(mesh.vertices[i]), zikuY)));
            }
            mesh.uv = uvs.ToArray();
            Debug.Log(mesh.uv.Length);
            filter.mesh = mesh;
            //Vector3 avPos;
            //Vector2 avUV;
            //for (int i = 0; i < mesh.triangles.Length/3; i++)
            //{
            //    avPos = filter.mesh.vertices[mesh.triangles[i * 3]] + filter.mesh.vertices[mesh.triangles[i * 3 + 1]] + filter.mesh.vertices[mesh.triangles[i * 3 + 2]];
            //    avUV = filter.mesh.uv[mesh.triangles[i * 3]] + filter.mesh.uv[mesh.triangles[i * 3 + 1]] + filter.mesh.uv[mesh.triangles[i * 3 + 2]];
            //    mesh.uv[mesh.triangles[i * 3]] = avUV + Division(filter.mesh.uv[mesh.triangles[i * 3]].x - avUV.x, filter.mesh.vertices[mesh.triangles[i * 3]] - avPos);
            //}
        }

        // Update is called once per frame
     
    }
}