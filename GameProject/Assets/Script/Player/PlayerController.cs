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
        // �⺻ ���� ���� �߰�
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