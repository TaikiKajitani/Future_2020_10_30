using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kajitani
{
    //窓の外を描画
    public class SeaSeter : SingletonMonoBehaviour<SeaSeter>
    {
        Vector4[] fish = {
            new Vector4(0,0,1,1),
            new Vector4(0,0,1,1),
            new Vector4(0,0,1,1),
            new Vector4(0,0,1,1),
        };
        Vector4[] moveVec = {
            new Vector4(0,0,0,0),
            new Vector4(0,0,0,0),
            new Vector4(0,0,0,0),
            new Vector4(0,0,0,0),
        };
        
        int[] Zsort ={
            0,1,2,3
        };
        string[] typeName ={
            "_FishPosition1",
            "_FishPosition2",
            "_FishPosition3",
            "_FishPosition4",
        };

        public Material material;
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                Init(i);
            }
        }
        // Update is called once per frame
        void Update()
        {

            for (int i = 0; i < 4; i++)
            {
                //移動処理
                fish[i] += moveVec[i] * Time.deltaTime;
                //画面外にいた場合座標リセット
                if (CheakPosition(fish[i]))
                {
                    Init(i);
                }
            }
            
            //手前の魚をさきに入れないとおかしくなるのでZソートしておく
            int c = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = i; j < 4; j++)
                {
                    if (Mathf.Abs(fish[Zsort[i]].z) > Mathf.Abs(fish[Zsort[j]].z))
                    {
                        c = Zsort[i];
                        Zsort[i] = Zsort[j];
                        Zsort[j] = c;
                    }
                }
            }
            //位置と大きさをセット
            for (int i = 0; i < 4; i++)
            {
                material.SetVector(typeName[Zsort[i]], fish[i]);
            }
        }
        //画面外に出たかどうか
        bool CheakPosition(Vector4 pos)
        {
            if ((pos.z * 0.5 +1< Mathf.Abs(pos.x)) || (pos.z * 0.5 +1< Mathf.Abs(pos.y)))
            {
                return true;
            }

            return false;
        }
        void Init(int i)
        {  //座標等のリセット
            //z位置をランダムで設定
            float deap = Random.Range(2.0f, 10.0f);
            
            //深度値に応じた座標を設定
            fish[i].x = deap * 0.5f+0.9f;
            fish[i].y = deap * Random.Range(-0.5f, 0.5f);
            //移動方向を設定
            moveVec[i].x = Random.Range(0.5f, 1.0f);
            if (fish[i].y < 0)
            {
                moveVec[i].y = Random.Range(0.0f, 0.25f);
            }
            else
            {
                moveVec[i].y = Random.Range(-0.25f, 0.0f);
            }
            fish[i].w = Random.Range(0.8f, 1.6f);
            //進む方向によって数値を逆にする
            if (Random.Range(0, 2) == 0)
            {
                fish[i].x *= -1;
            }
            else
            {
                fish[i].w *= -1;
                moveVec[i].x *= -1;
            }

            fish[i].z = deap;
        }

    }
}