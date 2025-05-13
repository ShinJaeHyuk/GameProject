using UnityEngine;
using PlayerData;

public class Skill : MonoBehaviour
{
    private Stats stats;
    void Start()
    {
        stats = Stats.LoadStats(GameManager.CurrentSlot);
    }
    public void UseDashSkill(Transform target)
    {
        if (target == null) return;
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)direction * 3f; // ���� �Ÿ�
        Debug.Log("Dash skill used!");
    }
    public void UseBladeSkill()
    {
        Debug.Log("Blade skill activated!");
        // Į ȸ�� ��ų ���� �߰� ����
    }
}