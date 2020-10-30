using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kajitani
{
    //血
    public class Blood : MonoBehaviour
    {
        Image image;

        float a_time = 2.0f;
        float b_time;
        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<Image>();
            Player.instance.blood = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (b_time < Time.time)
            {
                image.enabled = false;
            }
            else
            {
                image.enabled = true;
                Color color = image.color;
                color.a = (b_time - Time.time) / a_time;
                image.color = color;
            }
        }
        public void SetBlood()
        {
            b_time = Time.time + a_time;
        }
    }
}