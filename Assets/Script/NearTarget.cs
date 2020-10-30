using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class NearTarget : MonoBehaviour
    {
        [Header("判定軸")]
        [Tooltip("判定軸X")]
        public bool x = true;
        [Tooltip("判定軸Y")]
        public bool y = true;
        [Tooltip("判定軸Z")]
        public bool z = true;

        //
        [Header("距離を測る目標")]
        public GameObject target = null;

        //[HorizontalScope]

        [Header("しきい値")]
        public float near = 10;
        public float far = 20;

        //そのフレームに近いかどうか
        bool now = false;
         bool old = false;

        protected void Start()
        {
            //ターゲットが設定されていなければプレイヤーをターゲットにする
            if (target == null) {
                target = Player.instance.gameObject;
            }
        }

        // Update is called once per frame
       protected void Update()
        {

            float distance = 0;
            if (x)
            {
                distance += (transform.position.x - target.transform.position.x) * (transform.position.x - target.transform.position.x);
            }
            if (y)
            {
                distance += (transform.position.y - target.transform.position.y) * (transform.position.y - target.transform.position.y);
            }
            if (z)
            {
                distance += (transform.position.z - target.transform.position.z) * (transform.position.z - target.transform.position.z);
            }

            old = now;
            if (now)
            {
                if (far * far < distance)
                {
                    now = false;
                }
            }
            else
            {
                if (near * near > distance)
                {
                    now = true;
                }
            }
            if (now == true) {
                if(old == false)
                {
                    TargetTrigger();
                }
                TargetStay();
            }
            else
            {
                if (old == true)
                {
                    TargetExit();
                }
            }
        }
        protected virtual void TargetTrigger()
        {

        }
        protected virtual void TargetStay()
        {

        }
        protected virtual void TargetExit()
        {

        }
    }
}