using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]GameObject deathVFX;
    [SerializeField] AudioData[] deathSFX;
    [Header("--------血量--------")]
    
    [SerializeField]protected float maxHealth;
    protected float health;
    [SerializeField] StatsBar onHeadHealthBar;
    [SerializeField] bool showOnHeadHealthBar = true;


    protected virtual void OnEnable()
    {
        health = maxHealth;

        if (showOnHeadHealthBar)
        {
            ShowOnHeadHealthBar();
        }
        else
        {
            HideOnHeadHealthBar();
        }
    }
    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialized(health, maxHealth);
    }
    public void HideOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (showOnHeadHealthBar&&gameObject.activeSelf)
        {
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }
        if (health <= 0f)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        health = 0f;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX, transform.position);
        gameObject.SetActive(false);
    }
    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth) return;
        //health += value;
        //health = Mathf.Clamp(health, 0f, maxHealth);
        health = Mathf.Clamp(health + value, 0f, maxHealth);//选定角色恢复血量区域

        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }
    }

    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime,float percent)
    {
        while (health < maxHealth&&health>0f)
        {
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    }
    //protected IEnumerator DamageOvertimeCoroutine(WaitForSeconds waitTime, float percent)
    //{
    //    while (health>0f)
    //    {
    //        yield return waitTime;

    //        RestoreHealth(maxHealth * percent);
    //    }
    //}
}
