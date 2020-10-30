using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class Stairs : MonoBehaviour
    {
        //階段を上る/降りる
        public GameObject other;
        NearTarget nearTarget;
        // Start is called before the first frame update
        //void Awake()
        //{
        //    nearTarget = GetComponent<NearTarget>();
        //}

        //// Update is called once per frame
        //void Update()
        //{
        //    if (nearTarget.sts)
        //    {
        //        if (Input.GetKeyDown(KeyCode.Q))
        //        {
        //            Vector3 tar = transform.position + Vector3.up* 5;
        //            tar.y = Player.instance.transform.position.y;
        //            Player.instance.GoToMove(tar);
        //        }
        //    }
        //    if (nearTarget.sts == true && nearTarget.sts != false)
        //    {
        //        other.SetActive(true);
        //    }
        //    else if (nearTarget.sts == false && nearTarget.sts != true)
        //    {
        //        other.SetActive(false);
        //    }
        //}
    }
}