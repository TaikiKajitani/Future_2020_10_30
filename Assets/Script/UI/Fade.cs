using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kajitani
{
    public class Fade : SingletonMonoBehaviour<Fade>
    {
        Image renderer;

        public Material monoTone;

        public bool natural = true;

        public bool m_debug;
        [Range(0.0f, 1.0f)]
        public float rad = 0;
        
        // Start is called before the first frame update
        void Start()
        {
            renderer = GetComponent<Image>();
            SetAlpha(1);
        }
        float alpha_prm = 0;

        //遷移する時間
        float fadeSpeed = 1;

        enum FadeType
        {
            FadeNone,
            FadeIn,
            FadeOut
        }

        FadeType fadeType = FadeType.FadeNone;

        // Update is called once per frame
        void Update()
        {
            if (fadeType == FadeType.FadeOut)
            {
                alpha_prm += Time.deltaTime * 1.2f;
                if (alpha_prm >= fadeSpeed)
                {
                    fadeType = FadeType.FadeNone;
                }
                SetAlpha(Mathf.Min(alpha_prm / fadeSpeed));
            }
            if (fadeType == FadeType.FadeIn)
            {
                alpha_prm -= Time.deltaTime;
                if (alpha_prm <= 0)
                {
                    fadeType = FadeType.FadeNone;
                    renderer.enabled = false;
                }
                SetAlpha(Mathf.Max(0, alpha_prm / fadeSpeed));
            }
            if (m_debug)
            {
                SetAlpha(rad);
            }
        }
        //数値を適応
        private void SetAlpha(float prm)
        {
            if (monoTone)
            {
                monoTone.SetFloat("_Range", 1.0f - prm);
            }
        }
        //明かりを半分にする
        public void SetFadeHalf()
        {
            renderer.enabled = true;
            fadeType = FadeType.FadeNone;
            SetAlpha(0.5f);
        }
        //フェードイン開始
        public void SetFadeIn(float wantime)
        {
            fadeSpeed = wantime;
            renderer.enabled = true;
            fadeType = FadeType.FadeIn;
            alpha_prm = fadeSpeed;
            SetAlpha(1.0f);
        }
        //フェードイン開始
        public void SetFadeIn()
        {
            fadeSpeed = 0.5f;
            renderer.enabled = true;
            fadeType = FadeType.FadeIn;
            alpha_prm = fadeSpeed;
            SetAlpha(1.0f);
        }
        //フェードアウト開始
        public void SetFadeOut(float wantime)
        {
            fadeSpeed = wantime;
            renderer.enabled = true;
            fadeType = FadeType.FadeOut;
            alpha_prm = 0.0f;
            SetAlpha(alpha_prm);
        }
        public void FadeNoActive()
        {
            renderer.enabled = false;
        }
    }
}