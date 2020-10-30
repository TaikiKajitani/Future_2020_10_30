using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class DrawLine2 : MonoBehaviour
    {
        public SpriteRenderer circle;

        List<Vector3> points = new List<Vector3>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject obj = Instantiate(circle.gameObject);
                Vector3 cameraPosition = Input.mousePosition;
                cameraPosition.z = 10.0f;

                obj.transform.position = Camera.main.ScreenToWorldPoint(cameraPosition);
                points.Add(obj.transform.position);
            }
        }
    }
}