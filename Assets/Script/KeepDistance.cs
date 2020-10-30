using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class KeepDistance : MonoBehaviour
    {
        public GameObject target;
        Vector3 distance;

        // Start is called before the first frame update
        void Start()
        {
            distance = transform.position - target.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = target.transform.position + distance;// Camera.main.transform.forward* distance.magnitude;
        }
    }
}