using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class AutoRotation : MonoBehaviour
    {
        public Vector3 rotVec;
        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotVec,Time.deltaTime*60);
        }
    }
}