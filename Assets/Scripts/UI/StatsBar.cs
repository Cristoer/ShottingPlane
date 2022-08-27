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
        //if ״̬����
        if (currentFillAmount > targetFillAmount)
        {
            //ǰ��ͼƬѪ�����ֵ=Ŀ�����ֵ
            fillImageFont.fillAmount = targetFillAmount;
            //Ѫ���������ٺ���ͼƬ���ֵ
            bufferedFillingCoroutine=StartCoroutine(BufferedFillingCoroutine(fillImageBack));

            return;
        }
        //if״̬����
        if (currentFillAmount < targetFillAmount)
        {
            //Ѫ�����ֵ=Ŀ�����ֵ  �������ֵ
            fillImageBack.fillAmount = targetFillAmount;
            //��������ǰ�����ֵ
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
