using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    //アイテム入手時のカメラ移動
    public class ZoomCamera : SingletonMonoBehaviour<ZoomCamera>
    {
        Vector3 fstpos;
        Quaternion fstrot;
        public GameObject target;
        public Vector3 distance;
        public Quaternion tarq = Quaternion.AngleAxis(60, Vector3.right);
        //カメラが回り始める距離
        public float rotLength = 2.5f;

        public GameObject gettext;

        MonoBehaviour notice;

        float s = 0;
        int runtype = 0;
        KeepDistance keepDistance;
        
        //近づき始める
        public void Startnear(GameObject targetObj,MonoBehaviour mono)
        {
            if (runtype == 0)
            {
                Player.instance.type = Player.Type.ReadMode;
                target = targetObj;
                runtype = 1;
                fstrot = transform.rotation;
                fstpos = transform.position;

                notice = mono;

                //一定距離を保つクラスとは相性が悪いので停止させる
                keepDistance = GetComponent<KeepDistance>();
                if (keepDistance)
                {
                    keepDistance.enabled = false;
                }
            }
        }
        void End()
        {

            Player.instance.type = Player.Type.UserMode;
                        transform.position = fstpos;
            runtype = 0;
            //一定距離を保つクラスを復活
            if (keepDistance)
            {
                keepDistance.enabled = true;
            }
        }
        //表示を閉じる
        public void Close()
        {

            runtype = 3;
            //決定されたことを通知
            notice.SendMessage("CameraNotice");
            gettext.SetActive(false);
        }
        // Update is called once per frame
        private void FixedUpdate()
        {
            float len;
            switch (runtype)
            {
                case 1:
                    Debug.Log("近づく");
                    len = (target.transform.position + distance - transform.position).magnitude;

                    if (len > 0.1f)
                    {
                        transform.position += (target.transform.position + distance - transform.position).normalized * 0.1f;
                    }
                    else
                    {
                        transform.position = target.transform.position + distance;
                        runtype = 2;
                        //表示を出す
                        // gettext.SetActive(true);

                        StartCoroutine(FuncCoroutine());
                    }
                    transform.rotation = Quaternion.Lerp(fstrot, tarq, 1 - Mathf.Clamp(len / rotLength, 0, 1));
                    break;
                case 2:
                    ////FixedUpdateではトリガーできないので
                    //if (Input.GetButton("Fire1"))
                    //{
                    //    Close();
                    //}
                    break;
                case 3:
                    Debug.Log("遠ざかる");
                    len = (fstpos - transform.position).magnitude;

                    if (len > 0.1f)
                    {
                        transform.position += (fstpos - transform.position).normalized * 0.1f;
                    }
                    else
                    {
                        End();
                    }
                    transform.rotation = Quaternion.Lerp(fstrot, tarq, Mathf.Clamp(len / rotLength, 0, 1));
                    break;
            }
        }

        IEnumerator FuncCoroutine()
        {
            yield return new WaitForSeconds(1.7f);
            //決定されたことを通知
            notice.SendMessage("CameraNotice");
            yield return new WaitForSeconds(0.3f);
            runtype = 3;
        }
    }
}