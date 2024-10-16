using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float Hp;
    public float Damage;
    public float AttackSpeed;

    public float SuperDamage;
    public float SuperAttackSpeed;

    public float AttackRange = 2;

    [SerializeField] private Animator AnimatorController;
    [SerializeField] private Image PlayerHpIndicator;

    [SerializeField] private Image attackCooldownIndicator;
    [SerializeField] private Image superCooldownIndicator;
    [SerializeField] private Button superButton;
    [SerializeField] private ParticleSystem ps;

    private float lastAttackTime = 0;
    private float lastSuperTime = 0;

    private Coroutine _attackCooldown;
    private Coroutine _superCooldown;

    private Enemy closestEnemy;

    private bool isDead = false;
    public bool IsDead => isDead;

    private bool canMove = true;
    public bool CanMove => canMove;

    private float maxHp;

    private void Start()
    {
        maxHp = Hp;
    }

    public void Regen(float value)
    {
        if (isDead) return;
        
        Hp += value;
        if (Hp > maxHp)
            Hp = maxHp;

        UpdatePlayerHpIndicator();
    }

    public void ReceiveDamage(float value)
    {
        if (isDead) return;

        Hp -= value;

        if (Hp <= 0)
        {
            Hp = 0;
            Die();
        }

        UpdatePlayerHpIndicator();
    }

    private void UpdatePlayerHpIndicator()
    {
        PlayerHpIndicator.fillAmount = Hp / maxHp;
    }

    private void Update()
    {
        if (isDead)
            return;

        UpdateClosestEnemy();
        superButton.interactable = closestEnemy != null;
    }

    public void OnAttackClick()
    {
        if (_attackCooldown == null && Time.time - lastAttackTime > AttackSpeed)
        {
            lastAttackTime = Time.time;
            AnimatorController.SetTrigger("Attack");

            StartCoroutine(Attack(Damage));
            _attackCooldown = StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        float value;
        do
        {
            value = (Time.time - lastAttackTime) / AttackSpeed;
            attackCooldownIndicator.fillAmount = (Time.time - lastAttackTime) / AttackSpeed;
            yield return null;
        } while (value < 1f);
        _attackCooldown = null;
    }

    public void OnSuperClick()
    {
        if (_superCooldown == null && Time.time - lastSuperTime > SuperAttackSpeed)
        {
            lastSuperTime = Time.time;
            AnimatorController.SetTrigger("Super");

            StartCoroutine(Attack(SuperDamage, true));
            _superCooldown = StartCoroutine(SuperCooldown());
        }
    }

    private IEnumerator SuperCooldown()
    {
        float value;
        do
        {
            value = (Time.time - lastSuperTime) / SuperAttackSpeed;
            superCooldownIndicator.fillAmount = (Time.time - lastSuperTime) / SuperAttackSpeed;
            yield return null;
        } while (value < 1);
        _superCooldown = null;
    }

    private void UpdateClosestEnemy()
    {
        var enemies = SceneManager.Instance.Enemies;

        var closest = enemies
            .Where(e => e != null)
            .Select(e => new { Enemy = e, Distance = Vector3.Distance(transform.position, e.transform.position) })
            .OrderBy(x => x.Distance)
            .FirstOrDefault();

        closestEnemy = closest != null && closest.Distance <= AttackRange ? closest.Enemy : null;
    }

    private IEnumerator Attack(float damage, bool useCameraShake = false)
    {
        canMove = false;
        yield return new WaitForSeconds(0.2f);
        if (closestEnemy == null)
        {
            canMove = true;
            yield break;
        }

        transform.rotation = Quaternion.LookRotation(closestEnemy.transform.position - transform.position);
        closestEnemy.ReceiveDamage(damage);
        ps.Play();
        if (useCameraShake) CameraShake.Instance.StartShake();

        yield return new WaitForSeconds(0.2f);

        canMove = true;
    }

    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }

}
