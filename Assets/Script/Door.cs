using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    //ドアの奥に移動
    public class Door : NearTarget
    {
        //public enum KeyColor
        //{
        //    White,
        //    Red,
        //    Green,
        //    Blue,
        //    Yellow,
        //    Cyan,
        //    Magenta
        //}

        [Header("この扉が対応する鍵")]
        public KeyColor keyColor = KeyColor.Free;

        [Header("属する部屋")]
        public Room room;

        [Header("隣接するドア")]
        public Door LeadDoor;

        [Header("入れるときに表示されるボタン")]
        public GameObject other;
        //プレイヤーが移動する場所
        public Vector3 periodLocalPos = Vector3.forward * 2;

        [Header("ドアの動く部分")]
        public GameObject doorFuck;

        [Header("どのカギで開くかを表示")]
        public SpriteRenderer opencolor;

        //ドアの初期位置
        Vector3 pos_door;
        //ドアが動ける時間
        float fuck_time = 0;

        //その扉から入ってきてすぐかどうか
        [System.NonSerialized]
        public bool fstin = false;

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();
            //開くドアの初期位置を記録
            if (doorFuck)
            {
                pos_door = doorFuck.transform.localPosition;
            }
            //属している部屋がいなければ親から探していく
            Transform trans = transform;
            while (!room && trans.parent != null)
            {
                trans = trans.parent;
                room = trans.GetComponent<Room>();
            }
            if (room == null | LeadDoor == null)
            {
                Debug.LogError("扉の属する部屋か隣接するドアが設定されていません");
            }
            //鍵の色を表示
            if (opencolor != null)
            {
                if (keyColor == KeyColor.Free)
                {
                    opencolor.enabled = false;
                }
                else
                {
                    opencolor.enabled = true;
                    opencolor.color = Key.GetColor(keyColor);
                }
            }
        }
        // Update is called once per frame
        new void Update()
        {
            base.Update();
            //ドアを動かす処理
            if (doorFuck)
            {
                if (fuck_time > Time.time)
                {
                    Vector3 pos = doorFuck.transform.position;
                    pos += Time.deltaTime * new Vector3(2, 0, 0);
                    doorFuck.transform.position = pos;
                }
                else
                {
                    doorFuck.transform.localPosition = pos_door;
                }
            }
#if DEBUG
            Debug.Log((transform.position + periodLocalPos).ToString() + ":" + (LeadDoor.transform.position + LeadDoor.periodLocalPos).ToString());
            Debug.DrawLine(transform.position + periodLocalPos, LeadDoor.transform.position + LeadDoor.periodLocalPos, Color.red, 1, false);
#endif
        }
        //部屋に入った
        public void InRoom()
        {
            Player.instance.transform.position = transform.position + periodLocalPos;
            Debug.Log(Player.instance.transform.position);
            fstin = true;
        }

        protected override void TargetStay()
        {
            //入ってきてすぐでなければ
            if (!fstin)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    //プレイヤーがドアを開けれる状態なら
                    if (Player.instance.CheakChangeRoom())
                    {
                        InDoor();
                    }
                }
            }
        }
        void InDoor()
        {
            //プレイヤーがドアを開けれる状態なら
            if (OepnDoor())
            {
                //部屋に入る
                Vector3 tar = transform.position + periodLocalPos;
                //tar.y = Player.instance.transform.position.y;
                if (Player.instance.GoToMove(LeadDoor, tar))
                {
                    if (keyColor != KeyColor.Free)
                    {
                        //持っているキーを使用する
                        for (int i = 0; i < PlayerKeys.Instance.keys.Count; i++)
                        {
                            if (PlayerKeys.Instance.keys[i].color == keyColor)
                            {
                                Debug.Log(PlayerKeys.Instance.keys[i].uiKey);
                                //表示を消して
                                Destroy(PlayerKeys.Instance.keys[i].uiKey);
                                //所持リストから消す
                                PlayerKeys.Instance.keys.RemoveAt(i);
                                keyColor = KeyColor.Free;
                                break;
                            }
                        }
                    }
                    other.SetActive(false);
                    //引くドアを動かす
                    if (doorFuck)
                    {
                        fuck_time = Time.time + 1.5f;
                    }
                }
            }
        }
        bool OepnDoor()
        {
            //鍵がかかっていなければ
            if (keyColor == KeyColor.Free)
            {
                return true;
            }
            if (PlayerKeys.Instance.keys != null)
            {
                for (int i = 0; i < PlayerKeys.Instance.keys.Count; i++)
                {
                    if (PlayerKeys.Instance.keys[i].color == keyColor)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        protected override void TargetTrigger()
        {
            if (OepnDoor())
            {
                //入ってきてすぐでなければ
                if (!fstin)
                {
                    other.SetActive(true);
                }
            }
        }
        protected override void TargetExit()
        {
            //入ってきてすぐでなくする
            fstin = false;
            other.SetActive(false);
        }
    }
}