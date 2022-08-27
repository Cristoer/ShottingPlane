using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy :Singleton<PlayerEnergy>
{
    [SerializeField] EnergyBar energyBar;
    [SerializeField] int newEnergy = 50;
    [SerializeField] float overdriveInterval = 0.1f;
    public const int MAX = 100;
    public const int PERCENT = 1;
    bool availble = true;

    int energy;
    WaitForSeconds waitForOverDriveInterval;

    protected override void Awake()
    {
        base.Awake();
        waitForOverDriveInterval = new WaitForSeconds(overdriveInterval);
    }
    private void OnEnable()
    {
        PlayerOverDrive.on += PlayerOverDriveOn;
        PlayerOverDrive.off += PlayerOverDriveOff;
    }

    private void Start()
    {
        energyBar.Initialized(energy, MAX);
        Obtain(newEnergy);
    }
    public void Obtain(int value)
    {
        if (energy == MAX||!availble||!gameObject.activeSelf) return;
        //energy += value;
        energy = Mathf.Clamp(energy+value, 0, MAX);
        energyBar.UpdateStats(energy, MAX);

    }
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);
        if (energy == 0 && !availble)
        {
            PlayerOverDrive.off.Invoke();
        }
    }
    public bool IsEnough(int value)
    {
        return energy >= value;
    }
    void PlayerOverDriveOn()
    {
        availble = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }
    void PlayerOverDriveOff()
    {
        availble = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }

    IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf &&energy > 0)
        {
            yield return waitForOverDriveInterval;

            Use(PERCENT);

        }
    }
}
