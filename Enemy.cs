using UnityEngine;


public class Enemy : MonoBehaviour
{
   
    

    [SerializeField]
    Transform exit;
    [SerializeField]
    Transform[] wayPoints;
    [SerializeField]
    float navigation;
    [SerializeField]
    int health;
    [SerializeField]
    int rewardAmount;

    bool isDead = false;
    Collider2D enemyCollider;
    Animator anim;
    int target = 0;
    Transform enemy;
    float navigationTime = 0;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }



    private void Start()
    {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        Manager.Instance.RegisterEnemy(this);
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        if (wayPoints != null && isDead == false)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigation)
            {
                if (target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);

                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, navigationTime);

                }
                navigationTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MovingPoints")
        {
            target++;
        }

        else if(collision.tag == "Finish")
        {
            Manager.Instance.RoundEscaped++;
            Manager.Instance.TotalEscaped++;
            Manager.Instance.UnregisterEnemy(this);
            Manager.Instance.IsWaveOver();
        }

        else if (collision.tag == "Projectile")
        {
            Projectile newP = collision.gameObject.GetComponent<Projectile>();
            EnemyHit(newP.AttackDamage);
            Destroy(collision.gameObject);
        }
    }

    public void EnemyHit(int hitPoints)
    {
        if (health - hitPoints > 0)
        {
            // Getting damage
            health -= hitPoints;
            anim.Play("Hurt");
            Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
        }
        else
        {
            // Death of an enemy
            anim.SetTrigger("didDie");
            Death();
            
        }
        
    }

    public void Death ()
    {
        isDead = true;
        enemyCollider.enabled = false;
        Manager.Instance.TotalKilled++;
        Manager.Instance.IsWaveOver();
        Manager.Instance.addMoney(rewardAmount);
        Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);

    }
    
   
}