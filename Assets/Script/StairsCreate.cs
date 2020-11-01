using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class StairsCreate : MonoBehaviour
    {
        public int dan;
        public Vector2 scale;
        public GameObject purfub;
        public void Create()
        {
            GameObject child = new GameObject("child");
            child.transform.parent = transform;
            child.transform.localPosition = Vector3.zero;
            if (dan < 20)
            {
                for (int y = 1; y <= dan; y++)
                {
                    GameObject obj = Instantiate(purfub);
                    obj.transform.localScale = new Vector3(y * purfub.transform.localScale.x, 1, 1);
                    obj.transform.position = transform.position + new Vector3(y * 0.5f* purfub.transform.localScale.x, (dan - y) * purfub.transform.localScale.y, 0);
                    obj.transform.parent = child.transform;
                }
            }
        }
    }
}