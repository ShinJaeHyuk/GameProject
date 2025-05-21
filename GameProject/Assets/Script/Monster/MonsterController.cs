using UnityEngine;
using MonsterData;

public class MonsterController : MonoBehaviour
{
    private Monster monster;
    private Transform player;
    private float attackTimer;
    void Start()
    {
        monster = GetComponent<Monster>();
        player = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);
        float range = monster.stats.monsterType == "range" ? 5f : 1.5f;
        if (distance > range)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * monster.stats.monsterDef * Time.deltaTime); // �̵��ӵ��� monsterDef ��ü ����
        }
        else
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= 1f / monster.stats.monsterDef) // ���ݼӵ��� monsterDef ��ü ����
            {
                AttackPlayer();
                attackTimer = 0;
            }
        }
    }
    void AttackPlayer()
    {
        float damage = monster.stats.monsterAtk;
        float critRoll = Random.value;
        if (critRoll < monster.stats.monsterCritChance) damage *= monster.stats.monsterCritDmg;
        // �÷��̾ ������ �ִ� ����
        Debug.Log($"{monster.stats.monsterName} attacked player for {damage} damage");
    }
}
