using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float Hp;
    [SerializeField] private float Damage;
    [SerializeField] private float AtackSpeed;
    [SerializeField] private float AttackRange;
    [SerializeField] private float regenPlayerHp;

    [SerializeField] private Animator AnimatorController;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Collider collider;

    private float lastAttackTime = 0;
    private bool isDead = false;


    public void Init()
    {
        SceneManager.Instance.AddEnemy(this);
        Agent.SetDestination(SceneManager.Instance.Player.transform.position);
    }

    private void Update()
    {
        if (isDead) return;

        if (Hp <= 0)
        {
            Die();
            return;
        }

        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);
        bool inRange = distance <= AttackRange;
        Agent.isStopped = inRange;

        if (inRange)
        {
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
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
        yield return new WaitForSeconds(0.2f);
        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);
        bool inRange = distance <= AttackRange;
        if (!inRange) yield break;

        SceneManager.Instance.Player.ReceiveDamage(Damage);
    }


    protected virtual void Die()
    {
        SceneManager.Instance.Player.Regen(regenPlayerHp);
        SceneManager.Instance.RemoveEnemy(this);
        TakedownControllerUI.Instance.SpawnTakedown(gameObject.name);
        isDead = true;
        AnimatorController.SetTrigger("Die");
        collider.enabled = false;
        Agent.isStopped = true;
    }

}
