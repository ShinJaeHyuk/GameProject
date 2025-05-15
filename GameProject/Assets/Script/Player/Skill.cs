using UnityEngine;
using System.Collections.Generic;
using PlayerData;
public class Skill : MonoBehaviour 
{
    /*
    private Dictionary<int, Skill> skillDictionary;
    private Dictionary<int, float> skillCooldownTimers = new Dictionary<int, float>();
    private Stats playerStats;
    private Transform playerTransform;

    void Start()
    {
        skillDictionary = Stats.LoadSkills();  // ��ų ������ �ε�
        playerStats = Stats.LoadStats(GameManager.CurrentSlot);
        playerTransform = transform;

        // ��� ��ų�� ��Ÿ�� �ʱ�ȭ
        foreach (int skillID in skillDictionary.Keys)
        {
            skillCooldownTimers[skillID] = 0f;
        }
    }
    void Update()
    {
        // ��Ÿ�� ���� ó��
        List<int> skillIDs = new List<int>(skillCooldownTimers.Keys);
        foreach (int skillID in skillIDs)
        {
            if (skillCooldownTimers[skillID] > 0f)
            {
                skillCooldownTimers[skillID] -= Time.deltaTime;
            }
        }
    }
    public void UseSkill(int skillID)
    {
        if (!skillDictionary.ContainsKey(skillID)) return;
        Skill skill = skillDictionary[skillID];

        // ��ų ��� ���� ���� Ȯ��
        if (skillCooldownTimers[skillID] > 0f || !skill.unlocked) return;

        // ��ų Ÿ�Կ� ���� ó�� �б�
        switch (skillID)
        {
            case 1:  // Dash Strike
                UseDashStrike(skill);
                break;
            case 2:  // Spinning Blades
                UseSpinningBlades(skill);
                break;
            default:
                Debug.LogWarning($"Skill ID {skillID} is not defined.");
                break;
        }
        // ��ų ��Ÿ�� �ʱ�ȭ
        skillCooldownTimers[skillID] = skill.cooldown;
    }
    private void UseDashStrike(Skill skill)
    {
        // ���� ����� �� ã��
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(playerTransform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        // ���� ���� ���� ���� ���� ����
        if (closestEnemy != null && closestDistance <= skill.range)
        {
            Vector2 direction = (closestEnemy.transform.position - playerTransform.position).normalized;
            playerTransform.position = closestEnemy.transform.position;
            Debug.Log($"Dash Strike used on {closestEnemy.name}, dealt {skill.damage} damage.");
        }
    }
    private void UseSpinningBlades(Skill skill)
    {
        // ���̵� ������ ���� �� ȸ�� ����
        GameObject bladePrefab = Resources.Load<GameObject>("Prefabs/SpinningBlade");
        GameObject blades = Instantiate(bladePrefab, playerTransform.position, Quaternion.identity, playerTransform);
        blades.GetComponent<SpinningBlades>().Initialize(skill, playerStats);
        Debug.Log("Spinning Blades activated.");
    }*/
}