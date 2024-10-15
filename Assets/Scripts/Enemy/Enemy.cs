using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float Hp;
    [SerializeField] private float Damage;
    [SerializeField] private float AtackSpeed;
    [SerializeField] private float AttackRange;
    [SerializeField] private float regenPlayerHp;

    [SerializeField] private Animator AnimatorController;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Collider capsuleCollider;

    public event Action<float> OnHealthChanged; // in percentage

    private float _lastAttackTime = 0f;
    private bool _isDead = false;
    private float _maxHp;


    public void Init()
    {
        SceneManager.Instance.AddEnemy(this);
        _maxHp = Hp;
        Agent.SetDestination(SceneManager.Instance.Player.transform.position);
    }

    private void Update()
    {
        if (_isDead) return;

        Move();
    }

    private void Move()
    {
        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);
        bool inRange = distance <= AttackRange;
        Agent.isStopped = inRange;

        if (inRange)
        {
            if (Time.time - _lastAttackTime > AtackSpeed)
            {
                _lastAttackTime = Time.time;
                StartCoroutine(ActualDamage());
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.SetDestination(SceneManager.Instance.Player.transform.position);
        }
        AnimatorController.SetFloat("Speed", Agent.speed);
    }

    private IEnumerator ActualDamage()
    {
        yield return new WaitForSeconds(0.5f);

        if (_isDead) yield break;

        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);
        bool inRange = distance <= AttackRange;
        if (!inRange) yield break;

        SceneManager.Instance.Player.ReceiveDamage(Damage);
    }

    public void ReceiveDamage(float value)
    {
        Hp -= value;
        if (Hp <= 0)
        {
            Hp = 0;
            Die();
        }

        OnHealthChanged?.Invoke(Hp / _maxHp);
    }

    protected virtual void Die()
    {
        SceneManager.Instance.Player.Regen(regenPlayerHp);
        SceneManager.Instance.RemoveEnemy(this);
        TakedownControllerUI.Instance.SpawnTakedown(gameObject.name.Replace("(clone)",""));
        _isDead = true;
        AnimatorController.SetTrigger("Die");
        capsuleCollider.enabled = false;
        Agent.isStopped = true;
    }

    private IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(5f);

    }

}
