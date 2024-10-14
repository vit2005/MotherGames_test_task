using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.TextCore.Text;

public class DoubledEnemy : Enemy
{
    [SerializeField] private List<Enemy> EnemiesToSpawn = new List<Enemy>();


    protected override void Die()
    {
        foreach (Enemy e in EnemiesToSpawn)
        {
            Vector3 pos = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
            Instantiate(e, transform.position + pos, Quaternion.identity).Init();
        }

        base.Die();
    }
}
