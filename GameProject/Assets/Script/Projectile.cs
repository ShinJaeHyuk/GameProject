using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float moveSpeed = 2.0f;
    private float damage;
    private Transform player;
    private float maxDistance = 10.0f;
    private float traveledDistance = 0.0f;
    private float hitDistance = 0.5f; // 플레이어에게 도달했다고 판단할 거리
    public void Init(Vector2 direction, float damage)
    {
        this.direction = direction.normalized;
        this.damage = damage;
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("no player");            
        }
    }    
    void Update()
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        transform.Translate(direction * moveDistance);
        traveledDistance += moveDistance;
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= hitDistance)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damage);
                }
                Destroy(gameObject);
                return;
            }
        }
        if(traveledDistance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
