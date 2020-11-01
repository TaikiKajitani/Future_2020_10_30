using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kajitani
{
    //  鍵を入手する
    public class ItemGet : NearTarget
    {
        public GameObject other;
        [Header("UIとして生成されるカギ")]
        public GameObject prafab;
        public Key itemKey;
        // Update is called once per frame
        protected void Start()
        {
            base.Start();
            GetComponent<SpriteRenderer>().color = itemKey.GetColor();
        }

        protected override void TargetStay()
        {
            //アイテムを入手
            if (Input.GetButtonDown("Fire1"))
            {
                if (Player.instance.CheakChangeRoom())
                {
                    ZoomCamera.Instance.Startnear(gameObject, this);
                }
                //if (Player.instance.CheakChangeRoom())
                //{
                //    //鍵を生成
                //    GameObject obj = Instantiate(prafab);
                //    obj.GetComponent<Image>().color = itemKey.GetColor();
                //    itemKey.uiKey = obj;
                //    PlayerKeys.Instance.keys.Add(itemKey);
                //    obj.transform.parent = prafab.transform.parent;
                //    obj.transform.position = prafab.transform.position;
                //    obj.SetActive(true);
                //    //鍵を入手したら自身を消す
                //    Destroy(gameObject);
                //}
            }
        }
        //アイテムを入手（カメラからSendMessage）
        public void CameraNotice()
        {
            //鍵を生成
            GameObject obj = Instantiate(prafab);
            obj.GetComponent<Image>().color = itemKey.GetColor();
            itemKey.uiKey = obj;
            PlayerKeys.Instance.keys.Add(itemKey);
            obj.transform.parent = prafab.transform.parent;
            obj.transform.position = prafab.transform.position;
            obj.SetActive(true);
            //鍵を入手したら自身を消す
            Destroy(gameObject);
        }
        protected override void TargetTrigger()
        {
            other.SetActive(true);
        }
        protected override void TargetExit()
        {
            other.SetActive(false);
        }
    }
}