using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kajitani
{
    public class EmissionPlayer : MonoBehaviour
    {
        public float maxPower = 10;
        public float maxTime = 4;
        public float coolTime = 4;
        //現在光っているか
        public bool isLihjt { get; private set; } = false;
        bool isCoolTime = true;
        //ウミウシのマテリアル
        public Renderer renderer;
        //プレイヤー自体の発行の明るさ
        public float bright;
        //明るさを変えるライト
        Light light;
        //通常のマテリアル
      public  Material material_def;
        //発光状態のマテリアル
       public Material material_emi;
        // Start is called before the first frame update
        void Start()
        {
            //通常のマテリアルをセット
            renderer.material = material_def;
            //発光
            light = GetComponent<Light>();
            Player.instance.emission = this;
            light.enabled = false;
        }
        public bool ToLight()
        {
            if (isCoolTime)
            {
                isCoolTime = false;
                StartCoroutine(FuncCoroutine());
                return true;
            }
            return false;
        }

        IEnumerator FuncCoroutine()
        {
            //発光開始
            light.enabled = true;
            float lagtime = 0.5f;
            float l_time = Time.time + lagtime;
            //発光のマテリアルをセット
            renderer.material = material_emi;
            while (l_time > Time.time)
            {
                float power = (1 - (l_time - Time.time) / lagtime);
                Debug.Log(power * bright / 255.0f);
                material_emi.SetColor("_EmissionColor", new Color(power * bright / 255.0f, power * bright / 255.0f, power * bright / 255.0f, 1));
                light.intensity = maxPower * power;
                yield return new WaitForSeconds(0.01f);
            }
            //発光フラグをtrue
            isLihjt = true;
            light.intensity = maxPower;
            yield return new WaitForSeconds(maxTime);
            //発光フラグをfalse
            isLihjt = false;
            l_time = Time.time + lagtime;
            //発光を終わらせる
            while (l_time > Time.time)
            {
                float power = (l_time - Time.time) / lagtime;
                Debug.Log(power * bright / 255.0f);
                material_emi.SetColor("_EmissionColor", new Color(power * bright / 255.0f, power * bright / 255.0f, power * bright / 255.0f, 1));
                light.intensity = maxPower *power;
                yield return new WaitForSeconds(0.01f);
            }
            //通常のマテリアルをセット
            renderer.material = material_def;
            light.intensity = 0;
            light.enabled = false;
            //しばらく再発光できない
            yield return new WaitForSeconds(coolTime);
            isCoolTime = true;
        }
    }
}