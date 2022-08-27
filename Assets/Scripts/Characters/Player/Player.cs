using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player :Character
{
    [SerializeField]StatsBar_Hud statsBar_Hud;
    [Header("------Ѫ��-------")]
    [SerializeField] bool regenerateHealth = true;
    [SerializeField]float healthRegenerateTime;
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;
    [Header("-------����------")]
    [SerializeField]PlayerInput input;
    [Header("-------�ƶ�------")]
    [SerializeField]private float moveSpeed = 10f;
    [SerializeField] private float paddingX = 0.2f;
    [SerializeField] private float paddingY = 0.2f;
    [SerializeField] private float accelerationTime = 3f;//���ټ���
    [SerializeField] private float decelerationTime = 3f;
    [SerializeField] float moveRotationAngle = 50f;
    [Header("-------����------")]
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    [SerializeField] GameObject projectileOverdrive;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleBottom;
    [SerializeField] AudioData[] projectileLanchSFX;
    [SerializeField,Range(0,2)] int weaponPower=0;
    [SerializeField] float fireInterval=0.2f;
    [Header("------����-------")]
    [SerializeField] AudioData dodgeSFX;
    [SerializeField,Range(0,100)] int dodgeEnergyCost = 25;
    [SerializeField] float maxRoll = 720f;
    [SerializeField] float rollSpeed = 360f;
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
    [Header("------��������--------")]
    [SerializeField]int overdriveDodgeFactor = 2;
    [SerializeField]float overdriveSpeedFactor = 1.2f;
    [SerializeField]float overdriveFireFactor = 1.2f;

    [SerializeField,Range(0,1)] float slowMotionDuration=1f;

    bool isDoging=false;
    bool isOverDriving = false;
    float currentRoll;
    float dodgeDuration;

    float t ;
    Vector2 previousVelocity;
    Quaternion previousRotation;

    new Rigidbody2D rigidbody;
    new Collider2D collider;
    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitHealthRegenerateTime;
    WaitForSeconds waitForOverdriveAvalible;
    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        dodgeDuration = maxRoll / rollSpeed;
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverDrive += OverDrive;

        PlayerOverDrive.on += OverDriveOn;
        PlayerOverDrive.off += OverDriveOff;
    }
    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverDrive -= OverDrive;
        PlayerOverDrive.on -= OverDriveOn;
        PlayerOverDrive.off -= OverDriveOff;
    }
    void Start()
    {
        rigidbody.gravityScale = 0f;
        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitForOverdriveAvalible = new WaitForSeconds(fireInterval / overdriveFireFactor);
        input.EnableGameplayInput();

        statsBar_Hud.Initialized(health,maxHealth);
        StartCoroutine(nameof(MovePositionLimitCoroutine));

    }
    #region MOVE�ƶ���ش���
    void Move(Vector2 moveInput)//�ƶ�
    {
        //Vector2 moveAmount = moveInput * moveSpeed;
        //rigidbody.velocity = moveInput * moveSpeed;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle*moveInput.y,Vector3.right);
        moveCoroutine= StartCoroutine(MoveCoroutine(accelerationTime,moveInput.normalized *moveSpeed,moveRotation));//���ÿ�ʼ�ƶ�
        StartCoroutine(nameof(MovePositionLimitCoroutine));//�����ƶ�����
        
    }
    void StopMove()//ֹͣ�ƶ�
    {
        //rigidbody.velocity = Vector2.zero;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine =StartCoroutine(MoveCoroutine(decelerationTime,Vector2.zero,Quaternion.identity));
       // StopCoroutine(nameof(MovePositionLimitCoroutine));
    }
    IEnumerator MovePositionLimitCoroutine()//�ƶ�����Э��
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position,paddingX,paddingY);
            yield return null;
        }
    }
    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity,Quaternion moveRotation)//�ƶ����ٵ�Э��
    {
        t = 0f;
        previousVelocity = rigidbody.velocity;
        previousRotation = transform.rotation;
        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody.velocity= Vector2.Lerp(previousVelocity, moveVelocity, t);
            transform.rotation= Quaternion.Lerp(previousRotation, moveRotation, t);
            yield return new WaitForFixedUpdate();
        }
    }
    //IEnumerator StopMoveCoroutine(Vector2 moveVelocity)//��ʼ�ƶ���Э��
    //{
    //    float t = 0f;
    //    while (t < decelerationTime)
    //    {
    //        t += Time.fixedDeltaTime / decelerationTime;
    //        Vector2.Lerp(rigidbody.velocity, moveVelocity, t / decelerationTime);
    //        yield return null;
    //    }
    //}
    #endregion
    #region FIRE������ش���
    void Fire()
    {
        //StartCoroutine(FireCoroutine());//���ʲ���������������ʹ��nameof
        StartCoroutine(nameof(FireCoroutine));
        
    }
    void StopFire()
    {
        //StopCoroutine(FireCoroutine());
        StopCoroutine(nameof(FireCoroutine));
    }
    IEnumerator FireCoroutine()
    {
        //WaitForSeconds waitForFireInterval = new WaitForSeconds(fireInterval);
        while (true)
        {
            //switch (weaponPower)
            //{
            //    case 0:
            //        Instantiate(projectile1, muzzleMiddle.position, Quaternion.identity);
            //        break;
            //    case 1:
            //        Instantiate(projectile1, muzzleTop.position, Quaternion.identity);
            //        Instantiate(projectile1, muzzleBottom.position, Quaternion.identity);
            //        break;
            //    case 2:
            //        Instantiate(projectile1, muzzleMiddle.position, Quaternion.identity);
            //        Instantiate(projectile2, muzzleBottom.position, Quaternion.identity);
            //        Instantiate(projectile3, muzzleTop.position, Quaternion.identity);
            //        break;
            //    default:
            //        break;
            //}

            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(isOverDriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(isOverDriving ? projectileOverdrive : projectile1, muzzleTop.position);
                    PoolManager.Release(isOverDriving ? projectileOverdrive : projectile1, muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(isOverDriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    PoolManager.Release(isOverDriving? projectileOverdrive : projectile2, muzzleTop.position);
                    PoolManager.Release(isOverDriving ? projectileOverdrive : projectile3, muzzleBottom.position);
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayRandomSFX(projectileLanchSFX);
            yield return isOverDriving ? waitForOverdriveAvalible : waitForFireInterval;
        }
        //GameObject ProjectilbleOverdrive()
        //{
        //    //if (isOverDriving)
        //    //{
        //    //    return projectileOverdrive;
        //    //}
        //    //else
        //    //{
        //    //    return projectile1;
        //    //}
        //    return isOverDriving ? projectileOverdrive : projectile1;
        //}
            
    }
    #endregion
    #region ����˺�/�ָ�Ѫ��/����
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        statsBar_Hud.UpdateStats(health, maxHealth);
        if (gameObject.activeSelf)
        {
            if (regenerateHealth)
            {
                if (healthRegenerateCoroutine != null) {
                    StopCoroutine(healthRegenerateCoroutine);
                }
                StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }
    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        statsBar_Hud.UpdateStats(health, maxHealth);
    }
    public override void Die()
    {
        statsBar_Hud.UpdateStats(0f, maxHealth);
        base.Die();
    }
    #endregion
    #region ����
    void Dodge()
    {
        if (isDoging||!PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        StartCoroutine(nameof(DodgeCoroutine));
        
        
        //�����������ֵ
    }
    IEnumerator DodgeCoroutine()
    {
        isDoging = true;
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        //����ֵ����
        PlayerEnergy.Instance.Use(dodgeEnergyCost);
        //��������ʱ����޵�״̬
        collider.isTrigger = true;
        //����ʱ�������X�ᷭת 
        currentRoll = 0f;
        var t1 = 0f;
        var t2 = 0f;
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation= Quaternion.AngleAxis(currentRoll, Vector3.right);

            if (currentRoll < maxRoll / 2f)
            {
                t1 += Time.deltaTime / dodgeDuration;
                transform.localScale = Vector3.Lerp(transform.localScale, dodgeScale, t1);
            }
            else
            {
                t2 += Time.deltaTime / dodgeDuration;
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t2);
            }

            yield return null;
        }


        collider.isTrigger = false;
        isDoging = false;
    }
    #endregion
    #region ��������
    void OverDrive()
    {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;
        PlayerOverDrive.on.Invoke();
    }
    void OverDriveOn()
    {
        isOverDriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
        TimeController.Instance.BulletTime(slowMotionDuration);
    }
    void OverDriveOff()
    {
        isOverDriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }
    #endregion
}
