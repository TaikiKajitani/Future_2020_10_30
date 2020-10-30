using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class HitCamera : MonoBehaviour
    {
        public string targetlayer = "Ground";
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        RaycastHit GetCameraLength(Transform trans)
        {
            //プレイヤーからカメラへのレイ
            Ray ray = new Ray(trans.position, transform.rotation * -Vector3.forward);
            float far = Vector3.Distance(trans.transform.position, transform.position);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, far, 1 << LayerMask.NameToLayer(targetlayer));
            return hitInfo;
        }
    }
}
