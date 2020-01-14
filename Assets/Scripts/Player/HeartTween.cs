using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeartTween : MonoBehaviour
{
    [SerializeField]
    private Image heartFG;

    [SerializeField]
    private Image heartBG;

    [SerializeField]
    private Image flash;

    private bool animating;

    public void ResetFlash()
    {
        this.animating = false;
        StopAllCoroutines();
    }

    public void SetHeartFG(bool enable)
    {
        if (this.animating)
        {
            return;
        }
        if (!this.heartFG.enabled && enable)
        {
            this.ScaleOutFG(false);
        }
        this.heartFG.enabled = enable;
    }

    public void SetHeartBG(bool enable)
    {
        if (!this.heartBG.enabled && enable && base.gameObject.activeInHierarchy)
        {
            base.StartCoroutine(this.BGEffect());
        }
        this.heartBG.enabled = enable;
    }

    private void ScaleOutFG(bool enable)
    {
        if (enable)
        {
            //big
        }
        else
        {
            //normal
        }
    }

    public void Flash(Color c)
    {
        if (this.heartBG.enabled)
        {
            this.ResetFlash();
            this.FlashOnly(c, this.heartFG);
        }
    }

    private void FlashOnly(Color c, Image _parent)
    {
        GameObject f = Instantiate(flash.gameObject);
        f.transform.parent = _parent.transform;
        f.transform.localPosition = Vector3.zero;
        f.transform.localScale = Vector3.one;
        Image component = f.GetComponent<Image>();
        c.a = 0;
        component.color = c;
        component.DOFade(1,0.15f).OnComplete(()=> {
            component.color = Color.clear;
            Destroy(f);
        });
        f.transform.localScale = _parent.transform.localScale;
    }

    public void Hit()
    {
        if (this.heartBG.enabled)
        {
            this.ResetFlash();
            base.StartCoroutine(this.HitEffect());
        }
    }

    protected IEnumerator BGEffect()
    {
        this.FlashOnly(Color.white, this.heartBG);
        yield return base.StartCoroutine(PauseForSecs(0.15f));
        yield break;
    }

    protected IEnumerator HitEffect()
    {
        this.animating = true;
        this.ScaleOutFG(true);
        yield return base.StartCoroutine(PauseForSecs(0.15f));
        this.FlashOnly(Color.white, this.heartFG);
        yield return base.StartCoroutine(PauseForSecs(0.15f));
        this.animating = false;
        this.SetHeartFG(false);
        yield break;
    }
    public IEnumerator PauseForSecs(float numberOfSeconds)
    {
        float t = Time.realtimeSinceStartup;
        while (t + numberOfSeconds > Time.realtimeSinceStartup)
        {
            yield return null;
        }
        yield break;
    }
}
