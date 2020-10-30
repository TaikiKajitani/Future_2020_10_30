using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class SeaCowCrate : MonoBehaviour
    {
        public bool ok = true;

        float lastTime = 0;
       public float coolTime = 0.05f;
        public GameObject prefab;

        // Update is called once per frame
        void Update()
        {
            if (Player.instance.moveFlg)
            {
                if (Time.time > lastTime + coolTime)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.transform.position = transform.position + new Vector3(Random.Range(-transform.lossyScale.x, transform.lossyScale.x), Random.Range(-transform.lossyScale.y, transform.lossyScale.y), Random.Range(-transform.lossyScale.z, transform.lossyScale.z));
                    obj.transform.rotation = transform.rotation;
                    obj.transform.Rotate(Vector3.up, Random.Range(0.0f, 360.0f));
                    lastTime = Time.time;
                }
            }
        }
    }
}