using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class Room : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if(Player.instance.inRoom != this){
                gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}