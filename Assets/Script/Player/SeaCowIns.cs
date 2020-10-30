using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class SeaCowIns : MonoBehaviour
    {
        public float lostTime = 10;
        float fstTime;
        MeshRenderer renderer;
        MaterialPropertyBlock props;
        // Start is called before the first frame update
        void Start()
        {
            fstTime = Time.time;
            props = new MaterialPropertyBlock();
            renderer = GetComponent<MeshRenderer>();
            props.SetFloat("_Alpha", 1);
            renderer.SetPropertyBlock(props);
        }

        // Update is called once per frame
        void Update()
        {
            float alpha = 1 - (Time.time - fstTime) / lostTime;// * (Time.time - fstTime) / lostTime;
            props.SetFloat("_Alpha", alpha);
            renderer.SetPropertyBlock(props);
            if (lostTime + fstTime < Time.time) {
                Destroy(gameObject);
            }
        }
    }
}