using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int deathEnergyBonus = 3;
    [SerializeField]int ScorePoint = 100;
    public override void Die()
    {
        ScoreManager.Instance.AddScore(ScorePoint);
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        EnemyManager.Instance.RemoveFromList(gameObject);
        base.Die();
    }
}
