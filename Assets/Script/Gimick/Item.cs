using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kajitani
{
    //ドアの奥に移動
    public class Item : NearTarget
    {
        public GameObject other;
        [Header("UIとして生成されるカギ")]
        public GameObject prafab;
        public Key itemKey;
        // Start is called before the first frame update
        //new void Start()
        //{
        //    base.Start();
        //    nearTarget = GetComponent<NearTarget>();
        //}

        // Update is called once per frame
        protected void Start()
        {
            base.Start();
            GetComponent<SpriteRenderer>().color = itemKey.GetColor();
        }

        protected override void TargetStay()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //鍵を生成
                GameObject obj = Instantiate(prafab);
                obj.GetComponent<Image>().color = itemKey.GetColor();
                itemKey.uiKey = obj;
                PlayerKeys.Instance.keys.Add(itemKey);
                obj.transform.parent = prafab.transform.parent;
                obj.transform.position = prafab.transform.position;
                obj.SetActive(true);
                Destroy(gameObject);
            }
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