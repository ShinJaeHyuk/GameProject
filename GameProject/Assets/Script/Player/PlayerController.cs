using UnityEngine;
using PlayerData;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
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
        stats = Stats.LoadStats(GameManager.CurrentSlot);
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
    float CalculateDamage(float atk, Stats enemy)
    {
        if (enemy == null) return atk;
        // ����� ���
        float defRate = enemy.defence / (enemy.defence + defenceConstant);
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
        // ���� ü�� �ҷ�����
        //float damage = CalculateDamage(stats.attackPower, enemyStats); // ������ ���
        Debug.LogFormat("Distance : {0}, Basic attack on {1}", distanceToTarget, target.name);        
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
    
}