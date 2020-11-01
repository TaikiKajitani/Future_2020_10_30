using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

#if DEBUG
        float infinite = 10000;
        GameObject obj1, obj2;
#endif

        protected void Start()
        {
            //ターゲットが設定されていなければプレイヤーをターゲットにする
            if (target == null)
            {
                target = Player.instance.gameObject;
            }
#if DEBUG
            int ziku = 0;
            if (x)
            {
                ziku++;
            }
            if (y)
            {
                ziku++;
            }
            if (z)
            {
                ziku++;
            }
            obj1 = new GameObject(gameObject.name + "FarArea", typeof(MeshFilter), typeof(MeshRenderer));
            MeshFilter filter = obj1.GetComponent<MeshFilter>();
            MeshRenderer renderer = obj1.GetComponent<MeshRenderer>();
            renderer.material = DebugMesh.Instance.FarMaterial;
            renderer.receiveShadows = false;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            obj1.transform.position = transform.position;

            obj2 = new GameObject(gameObject.name + "NearArea", typeof(MeshFilter), typeof(MeshRenderer));
            MeshFilter filter2 = obj2.GetComponent<MeshFilter>();
            MeshRenderer renderer2 = obj2.GetComponent<MeshRenderer>();
            renderer2.material = DebugMesh.Instance.nearMaterial;
            renderer2.receiveShadows = false;
            renderer2.shadowCastingMode = ShadowCastingMode.Off;
            obj2.transform.position = transform.position;
            switch (ziku)
            {
                case 3:
                    filter.mesh = DebugMesh.Instance.Sphere;
                    filter2.mesh = DebugMesh.Instance.Sphere;
                    obj1.transform.localScale = new Vector3( far * 2,  far * 2,  far * 2);
                    obj2.transform.localScale = new Vector3(near * 2, near * 2, near * 2);
                    break;
                case 2:
                    filter.mesh = DebugMesh.Instance.Cylinder;
                    filter2.mesh = DebugMesh.Instance.Cylinder;
                    obj1.transform.localScale = new Vector3( far * 2, infinite,  far * 2);
                    obj2.transform.localScale = new Vector3(near * 2, infinite, near * 2);
                    if (!x)
                    {
                        obj1.transform.Rotate(0, 0, 90);
                        obj2.transform.Rotate(0, 0, 90);
                    }
                    if (!y)
                    {
                    }
                    if (!z)
                    {
                        obj1.transform.Rotate(90, 0, 0);
                        obj2.transform.Rotate(90, 0, 0);
                    }
                    break;
                case 1:
                    filter.mesh = DebugMesh.Instance.Cube;
                    filter2.mesh = DebugMesh.Instance.Cube;
                    if (x)
                    {
                        obj1.transform.localScale = new Vector3( far * 2, infinite, infinite);
                        obj2.transform.localScale = new Vector3(near * 2, infinite, infinite);
                    }
                    if (y)
                    {
                        obj1.transform.localScale = new Vector3(infinite,  far * 2, infinite);
                        obj2.transform.localScale = new Vector3(infinite, near * 2, infinite);
                    }
                    if (z)
                    {
                        obj1.transform.localScale = new Vector3(infinite, infinite,  far * 2);
                        obj2.transform.localScale = new Vector3(infinite, infinite, near * 2);
                    }
                    break;
                default:
                    Destroy(obj1);
                    Destroy(obj2);
                    return;
            }
            obj1.transform.parent = transform;
            obj2.transform.parent = transform;
#endif
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

#if DEBUG
            if (DebugMesh.Instance.near)
            {
                obj1.SetActive(true);
            }
            else
            {
                obj1.SetActive(false);
            }
            if (DebugMesh.Instance.far)
            {
                obj2.SetActive(true);
            }
            else
            {
                obj2.SetActive(false);
            }
#endif
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