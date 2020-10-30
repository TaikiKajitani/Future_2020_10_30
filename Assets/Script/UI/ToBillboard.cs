using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    //常に画面の方向を向く
    public class ToBillboard : MonoBehaviour
    {
        void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }
}