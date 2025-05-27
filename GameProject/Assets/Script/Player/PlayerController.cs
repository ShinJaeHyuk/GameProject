using UnityEngine;
using PlayerData;
using System.Collections.Generic;
using MonsterData;

public class PlayerController : MonoBehaviour
{
    private Monster monster;
    private Stats stats;
    private Transform target;
    private float attackTimer = 0f;
    private bool autoSkillEnabled = true;
    private Skill[] skills = new Skill[2];
    private const float defenceConstant = 1000f; // ��� ���
    private float attackRange = 1.0f; // ���� ����
    public float distanceToTarget;
    void Start()
    {
        stats = Stats.LoadStats(GameManager.currentSlot);
        // ��ų �ʱ�ȭ
        
    }
    void Update()
    {
        FindClosestTarget();
        HandleMovement();
        HandleAttack();
    }    
    void FindClosestTarget()
    {
        // ����� �� ã��
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");
        float closestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy.transform;
            }
        }
    }
    void HandleMovement()
    {
        // ������ �̵�
        if (target == null) return;
        Vector2 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * stats.moveSpeed * Time.deltaTime);
    }
    void HandleAttack()
    {
        // ��ų �Ǵ� �Ϲݰ���
        attackTimer -= Time.deltaTime;        
        // ���� ���� Ȯ��
        if(target == null) return;
        distanceToTarget = Vector2.Distance(transform.position, target.position);        
        if (distanceToTarget > attackRange) return;
        if (attackTimer <= 0f)
        {
            BasicAttack();
            //if (autoSkillEnabled)
            //{
            //    UseSkill();
            //}
            //else
            //{
            //    BasicAttack();
            //}
            attackTimer = 1f / stats.attackSpeed;
        }
    }
    float CalculateDamage(float atk, MonsterStats enemy)
    {
        if (enemy == null) return atk;
        // ����� ���
        float defRate = enemy.monsterDef / (enemy.monsterDef + defenceConstant);
        defRate *= (1f - (stats.defencePenetration / 100f));
        // ġ��Ÿ ����
        // 100%�� ��� �ݵ�� ġ��Ÿ
        bool isCritical = (stats.critChance >= 100f) || (Random.value < (stats.critChance / 100f));
        float critMultiplier = isCritical ? (1f + (stats.critDmg / 100f)) : 1f;
        // ���� ������
        return atk * (1f - defRate) * critMultiplier;
    }
    void BasicAttack()
    {
        // �⺻ ���� ����
        if (target == null) return;
        Monster monsterComponent = target.GetComponent<Monster>();
        if(monsterComponent == null) return;
        // ���� ü�� �ҷ�����
        MonsterStats enemyStats = monsterComponent.stats;
        // ������ ���
        float damage = CalculateDamage(stats.attackPower, enemyStats);
        // ���� ������ ����
        monsterComponent.TakeDamage(damage);
        Debug.LogFormat($"{target.name}���� {damage}�� ������ ����");        
    }
    public void ToggleAutoSkill()
    {
        autoSkillEnabled = !autoSkillEnabled;
        Debug.Log("Auto Skill: " + (autoSkillEnabled ? "Enabled" : "Disabled"));
    }
    void UseSkill()
    {
        for(int i = 0; i < skills.Length; i++)
        {
            if(Time.time >= skills[i].lastUsedTime + skills[i].cooldown)
            {
                skills[i].Use(transform, target);
                skills[i].lastUsedTime = Time.time;
                break;
            }
        }           
    }
    public void TakeDamage(float damage)
    {
        stats.currentHp -= damage;
        //Debug.Log($"player took {damage} damage. HP left : {stats.currentHp}");
        if(stats.currentHp <= 0)
        {
            //Debug.Log($"player died");
        }
    }
}