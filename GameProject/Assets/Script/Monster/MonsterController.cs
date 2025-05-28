using UnityEngine;
using MonsterData;

public class MonsterController : MonoBehaviour
{
    private Monster monster;
    private Transform player;
    private float attackTimer;
    private const float defenceConstant = 1000f; // 방어 상수
    private GameObject bullet;
    private Transform firePoint;
    private float moveSpeed = 2.0f;
    private float spawnDelay = 1.5f;
    private float spawnTimer = 0f;
    private bool canMove = false;
    void Start()
    {
        monster = GetComponent<Monster>();        
        player = GameObject.FindWithTag("Player").transform;        
        if (monster.stats.monsterType == "Range")
        {
            firePoint = transform.Find("FirePoint");
            if (firePoint == null) Debug.LogWarning("There is no Firepoint");
        }
        bullet = Resources.Load<GameObject>("Prefab/Bullet");
        if (bullet == null) Debug.LogWarning("No bullet");
    }
    void Update()
    {
        if (!canMove)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnDelay)
            {
                canMove = true;
            }
            return;
        }
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);        
        float range = monster.stats.monsterType == "range" ? 5f : 1.5f;
        if (distance > range)
        {            
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime); // 이동속도는 moveSpeed로 고정                        
        }
        else
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= 2f) 
            {
                int damage = CalculateDamage(); // 데미지 계산
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
    int CalculateDamage()
    {
        // 스탯 불러오기
        var playerStats = PlayerData.Stats.LoadStats(GameManager.currentSlot);
        // 방어율 계산
        float defRate = playerStats.defence / (playerStats.defence + defenceConstant);
        defRate *= (1f - (monster.stats.monsterDefencePenetration / 100f));
        // 치명 판정
        bool isCritical = (monster.stats.monsterCritChance >= 100f) || (Random.value < (monster.stats.monsterCritChance / 100f));
        float critMultiplier = isCritical ? (1f + (monster.stats.monsterCritDmg / 100f)) : 1f;
        // 최종 데미지 계산
        int damage = (int)(monster.stats.monsterAtk * (1f - defRate) * critMultiplier);
        return damage;        
    }
    void AttackPlayer(int damage)
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null) return;
        PlayerController playerController = playerObj.GetComponent<PlayerController>();
        if(playerController == null) return;
        playerController.TakeDamage(damage);
    }
    void ShootProjectile(int damage)
    {
        if (firePoint == null) firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint not found");
            return;
        }
        GameObject projectile = Instantiate(bullet, firePoint.position, Quaternion.identity);        
        projectile.GetComponent<Projectile>().Init(player, damage);
    }
}
