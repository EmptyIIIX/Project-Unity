using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector2 knockbackToSelf = new Vector2(3f, 5f);
    [SerializeField] private Vector2 knockbackToPlayer = new Vector2(3f, 5f);
    [SerializeField] private float enemyKnockbackDuration = 1.5f;
    //[SerializeField] private float playerKnockbackDuration = 1.5f;

    public void Die()
    {
        Destroy(gameObject);
    }

    public void HitPlayer(Transform playerTransform)
    {
        int direction = GetDirection(playerTransform);
        FindAnyObjectByType<PlayerMovement2D>().KnockbackPlayer(knockbackToPlayer, direction);
        //FindAnyObjectByType<PlayerMovement2D>().KnockbackPlayer(knockbackToPlayer, direction, playerKnockbackDuration);
        GetComponent<EnemyMovement>().KnockbackEnemy(knockbackToSelf, -direction, enemyKnockbackDuration);
    }

    private int GetDirection(Transform playerTransform)
    {
        if(transform.position.x > playerTransform.position.x)
        {
            // The enemy is to the right of the player
            return -1;
        }
        else
        {
            return 1;
        }
    }
}
