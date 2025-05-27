using UnityEngine;
using MonsterData;

public class MonsterController : MonoBehaviour
{
    private Monster monster;
    private Transform player;
    private float attackTimer;
    private const float defenceConstant = 1000f; // ��� ���
    private GameObject bullet;
    private Transform firePoint;

    void Start()
    {
        monster = GetComponent<Monster>();
        player = GameObject.FindWithTag("Player").transform;
        if(monster.stats.monsterType == "Range")
        {
            firePoint = transform.Find("FirePoint");
            if (firePoint == null) Debug.LogWarning("There is no Firepoint");
        }
        bullet = Resources.Load<GameObject>("Prefab/Bullet");
        if (bullet == null) Debug.LogWarning("No bullet");
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
            if (attackTimer >= 1f / monster.stats.monsterDef) 
            {
                float damage = CalculateDamage(); // ������ ���
                if(monster.stats.monsterType == "Range")
                {
                    ShootProjectile(damage);
                }
                else
                {
                    AttackPlayer(damage);
                }
                attackTimer = 0;
            }
        }
    }
    float CalculateDamage()
    {
        // ���� �ҷ�����
        var playerStats = PlayerData.Stats.LoadStats(GameManager.currentSlot);
        // ����� ���
        float defRate = playerStats.defence / (playerStats.defence + defenceConstant);
        defRate *= (1f - (monster.stats.monsterDefencePenetration / 100f));
        // ġ�� ����
        bool isCritical = (monster.stats.monsterCritChance >= 100f) || (Random.value < (monster.stats.monsterCritChance / 100f));
        float critMultiplier = isCritical ? (1f + (monster.stats.monsterCritDmg / 100f)) : 1f;
        // ���� ������ ���
        float damage = monster.stats.monsterAtk * (1f - defRate) * critMultiplier;
        return damage;        
    }
    void AttackPlayer(float damage)
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null) return;
        PlayerController playerController = playerObj.GetComponent<PlayerController>();
        if(playerController == null) return;
        playerController.TakeDamage(damage);
    }
    void ShootProjectile(float damage)
    {
        if (firePoint == null) firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint not found");
            return;
        }
        GameObject projectile = Instantiate(bullet, firePoint.position, Quaternion.identity);
        Vector2 direction = (transform.position - player.position).normalized;
        projectile.GetComponent<Projectile>().Init(direction, damage);
    }
}
