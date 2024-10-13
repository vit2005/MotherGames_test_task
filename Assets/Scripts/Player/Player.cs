using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;

    private float lastAttackTime = 0;
    private bool isDead = false;
    public Animator AnimatorController;

    private void Update()
    {
        if (isDead)
            return;

        if (Hp <= 0)
        {
            Die();
            return;
        }

        // AutoAttack();
    }

    public void OnAttackClick()
    {
        if (Time.time - lastAttackTime > AtackSpeed)
        {
            lastAttackTime = Time.time;
            AnimatorController.SetTrigger("Attack");

            Attack();
        }
    }

    private void Attack()
    {
        var enemies = SceneManager.Instance.Enemies;

        var closest = enemies
            .Where(e => e != null)
            .Select(e => new { Enemie = e, Distance = Vector3.Distance(transform.position, e.transform.position) })
            .OrderBy(x => x.Distance)
            .FirstOrDefault();

        if (closest != null)
        {
            if (closest.Distance <= AttackRange)
                closest.Enemie.Hp -= Damage;
        }
    }

    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }


}
