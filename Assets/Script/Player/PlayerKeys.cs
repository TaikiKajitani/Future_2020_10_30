using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    [System.Serializable]
    public enum KeyColor
    {
        Free,
        Red,
        Green,
        Blue,
        Yellow,
        Cyan,
        Magenta,
        Black
    }
    [System.Serializable]
    public class Item
    {

    }

    //鍵
        [System.Serializable]
    public class Key : Item
    {
        //鍵の色
        public GameObject uiKey;
        public KeyColor color;
        public Key(KeyColor c)
        {
            uiKey = null;
            color = c;
        }
        public Color GetColor()
        {
            switch (color) {
                case KeyColor.Free:
                    return Color.white;
                case KeyColor.Red:
                    return Color.red;
                case KeyColor.Green:
                    return Color.green;
                case KeyColor.Blue:
                    return Color.blue;
                case KeyColor.Yellow:
                    return Color.yellow;
                case KeyColor.Cyan:
                    return Color.cyan;
                case KeyColor.Magenta:
                    return Color.magenta;
                case KeyColor.Black:
                    return Color.black;
            }
            return Color.white;
        }
        public static Color GetColor(KeyColor color1)
        {
            switch (color1)
            {
                case KeyColor.Free:
                    return Color.white;
                case KeyColor.Red:
                    return Color.red;
                case KeyColor.Green:
                    return Color.green;
                case KeyColor.Blue:
                    return Color.blue;
                case KeyColor.Yellow:
                    return Color.yellow;
                case KeyColor.Cyan:
                    return Color.cyan;
                case KeyColor.Magenta:
                    return Color.magenta;
                case KeyColor.Black:
                    return Color.black;
            }
            return Color.white;

        }
    }

    public class Report:Item
    {

    }


    public class PlayerKeys : SingletonMonoBehaviour<PlayerKeys>
    {
        [System.NonSerialized]
        public List<Key> keys = new List<Key>();
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
