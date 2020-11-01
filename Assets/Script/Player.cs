using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class Player : MonoBehaviour
    {
        //部屋移動にかけていい最大時間
        const float waitTime = 1.0f;
        public float stratTime = 0;
        //public Door door;
        //今いる部屋
        public Room inRoom;
        //次に向かうドア
        public Door nextDoor;

        //血
        [System.NonSerialized]
        public Blood blood;

        [System.NonSerialized]
        public EmissionPlayer emission;

        Vector3 lastPostion;

        public bool moveFlg {private set; get; } = false;

        //今いる部屋を更新
        //public void SetRoom(Door neo)
        //{
        //    inRoom = neo.room;
        //    Vector3 tar = neo.transform.position + neo.periodLocalPos;
        //   // tar.y = transform.position.y;
        //    transform.position = tar;
        //    neo.fstin = true;
        //}

        public enum Type
        {
            UserMode,
            MovieMode,
            ReadMode
        }

        static public Player instance { get; private set; } = null;

        Vector3 periods;

        public Type type { get; set; } = Type.UserMode;
        UnityChan.UnityChanControlScriptWithRgidBody2 user;
        Rigidbody rb;

        // Start is called before the first frame update
        void Awake()
        {
            instance = this;
            user = GetComponent<UnityChan.UnityChanControlScriptWithRgidBody2>();
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            moveFlg = false;
            //ユーザーの動かせる状態
            if (type == Type.UserMode)
            {
             Vector3 pos=  transform.position;
                pos.z = inRoom.transform.position.z;
                transform.position = pos;
                if (Vector3.Distance(transform.position,lastPostion) > Time.fixedDeltaTime)
                {
                    moveFlg = true;
                }
                lastPostion = transform.position;
            }
            //イベントで動いている状態
            if (type == Type.MovieMode)
            {
                //目的地まで移動
                Vector3 pos = transform.position;
                transform.LookAt(periods);
                pos += (periods - pos).normalized * Time.fixedDeltaTime*user.forwardSpeed;
                user.SetSpeed(1);
                //目的地に着いたときもしくは一定時間を超えたとき
                if ((periods - pos).magnitude < user.forwardSpeed * Time.fixedDeltaTime || stratTime + waitTime < Time.time)
                {

                }
                else
                {
                    transform.position = pos;
                }
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (blood)
                {
                    blood.SetBlood();
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (emission)
                {
                    emission.ToLight();
                }
            }
        }
        //目的地まで移動開始
        public bool GoToMove(Door nr, Vector3 period)
        {
            //プレイヤーを動かせる状態なら
            if (type == Type.UserMode)
            {
                type = Type.MovieMode;
                //目的地を設定
                periods = period;
                nextDoor = nr;
                //物理をキネマティックにする
                rb.isKinematic = true;
                stratTime = Time.time;

                Fade.Instance.SetFadeOut(0.5f);
                Invoke("FadeIn", 0.8f);

                return true;
            }
            return false;
        }
        void FadeIn()
        {
            if (inRoom)
            {
                inRoom.gameObject.SetActive(false);
            }
            //新しい部屋に入る
            inRoom = nextDoor.room;
            type = Type.UserMode;
            rb.isKinematic = false;


            //目的地の部屋をアクティブにする
            nextDoor.room.gameObject.SetActive(true);
            nextDoor.InRoom();

            Fade.Instance.SetFadeIn(0.5f);

        }
        //別の部屋に移ってよいか
        public bool CheakChangeRoom()
        {
            if (user.GetMoveLevel())
            {
                return true;
            }
            return false;
        }
    }
}