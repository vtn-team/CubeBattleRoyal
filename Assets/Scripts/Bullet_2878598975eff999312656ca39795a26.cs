using UnityEngine;

/// <summary>
/// 課題用の弾処理
/// NOTE: 唯一編集するコード
/// </summary>
public class Bullet_2878598975eff999312656ca39795a26 : BulletBase
{
    GameObject _targerPlayer;

    GameObject _player;

    public string playerName;

    public Vector3 velocity;

    public override void Bang()
    {
        velocity = Logic.GetRandomTargetVec(this);
        playerName = this.GetType().ToString();

        BoxCollider box = GetComponent<BoxCollider>();
        box.isTrigger = true;
        box.size = new Vector3(1000, 1000, 1000);


        Bullet_2878598975eff999312656ca39795a26 bullet = Instantiate(this);
        bullet.playerName = playerName;
        bullet.velocity = -velocity * 19.9f;
    }

    private void FixedUpdate()
    {
        Vector3 before;
        Vector3 after;

        if (_player != null)
        {
            _player.transform.position = transform.position;
            before = this.transform.position;
            this.transform.position += velocity * 300f;
            after = this.transform.position;
            _player.GetComponent<BoxCollider>().enabled = false;
        }
        else 
        {
            before = this.transform.position;
            this.transform.position += velocity * 19.9f;
            after = this.transform.position;
        }

        //テスト用に距離を
        Debug.Log((after - before).magnitude);

        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (playerName != collision.gameObject.name)
        {
            _targerPlayer = collision.gameObject;
            Logic.DamageToTarget(collision.gameObject, collision.gameObject);
        }
        else
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            _player = collision.gameObject;
            velocity = new Vector3(velocity.x, 5f, velocity.z);
            _player.transform.position += velocity * 19.9f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerName != other.gameObject.name)
        {
            _targerPlayer = other.gameObject;
            Logic.DamageToTarget(other.gameObject, other.gameObject);
        }
        else
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnDestroy()
    {
        if (_targerPlayer != null)
        {
            for (int i = 0; i < 10; i++)
            {
                Logic.DamageToTarget(_targerPlayer, _targerPlayer);
            }
        }
    }
}
