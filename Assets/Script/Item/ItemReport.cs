using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class ItemReport : NearTarget
    {
        public GameObject other;
        public Report report;

        protected void Start()
        {
            base.Start();
        }

        protected override void TargetStay()
        {
            //アイテムを入手
            if (Input.GetButtonDown("Fire1"))
            {
                
                //鍵を入手したら自身を消す
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