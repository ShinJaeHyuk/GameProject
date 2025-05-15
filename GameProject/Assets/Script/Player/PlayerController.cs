using UnityEngine;
using PlayerData;

public class PlayerController : MonoBehaviour
{
    private Stats stats;
    private Transform target;
    private float attackTimer = 0f;
    private bool autoSkillEnabled = true;
    private Skill[] skills = new Skill[2];
    void Start()
    {
        stats = Stats.LoadStats(GameManager.CurrentSlot);        
        
    }
    void Update()
    {
        FindClosestTarget();
        HandleMovement();
        HandleAttack();
    }    
    void FindClosestTarget()
    {
        // 가까운 적 찾기
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
        // 적에게 이동
        if (target == null) return;
        Vector2 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * stats.moveSpeed * Time.deltaTime);
    }
    void HandleAttack()
    {
        // 스킬 또는 일반공격
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            if (autoSkillEnabled)
            {
                UseSkill();
            }
            else
            {
                BasicAttack();
            }
            attackTimer = 1f / stats.attackSpeed;
        }
    }
    void BasicAttack()
    {
        if (target == null) return;
        Debug.Log("Basic attack on " + target.name);
        // 기본 공격 로직 추가
    }
    public void ToggleAutoSkill()
    {
        autoSkillEnabled = !autoSkillEnabled;
        Debug.Log("Auto Skill: " + (autoSkillEnabled ? "Enabled" : "Disabled"));
    }
    void UseSkill()
    {
       
    }
}