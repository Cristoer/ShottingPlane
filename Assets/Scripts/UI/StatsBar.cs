using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField]Image fillImageBack;
    [SerializeField]Image fillImageFont;
    [SerializeField] bool delayFill = true;
    [SerializeField] float fillDelay = 0.5f;
    [SerializeField] float fillSpeed = 0.1f;

    float previouscurrentFillAmount;
    float currentFillAmount;
    protected float targetFillAmount;
    float t;
    Canvas canvas;
    WaitForSeconds waitForDelayFill;
    Coroutine bufferedFillingCoroutine;

    private void Awake()
    {
        //canvas = GetComponent<Canvas>();
        if(TryGetComponent<Canvas>(out Canvas canvas)) 
        { 
            canvas.worldCamera = Camera.main;
        }
        

        waitForDelayFill = new WaitForSeconds(fillDelay);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public virtual void Initialized(float currentValue,float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFont.fillAmount = currentFillAmount;
    }
    public void UpdateStats(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue /maxValue;

        if (bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }
        //if 状态减少
        if (currentFillAmount > targetFillAmount)
        {
            //前面图片血量填充值=目标填充值
            fillImageFont.fillAmount = targetFillAmount;
            //血量慢慢减少后面图片填充值
            bufferedFillingCoroutine=StartCoroutine(BufferedFillingCoroutine(fillImageBack));

            return;
        }
        //if状态增加
        if (currentFillAmount < targetFillAmount)
        {
            //血量填充值=目标填充值  后面填充值
            fillImageBack.fillAmount = targetFillAmount;
            //缓慢增加前面填充值
            bufferedFillingCoroutine= StartCoroutine(BufferedFillingCoroutine(fillImageFont));
        }

    }
    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (delayFill)
        {
            yield return waitForDelayFill;
        }
        previouscurrentFillAmount = currentFillAmount;
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(previouscurrentFillAmount, targetFillAmount,t);
            image.fillAmount = currentFillAmount;

            yield return null;
        }
    }
}
