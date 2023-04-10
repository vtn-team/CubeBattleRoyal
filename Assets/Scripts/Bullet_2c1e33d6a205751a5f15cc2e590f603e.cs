using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 課題用の弾処理
/// NOTE: 唯一編集するコード 
/// アイデア　最速出現＋くそでかコライダー＋Bulletクラスで出来なければ、他のクラスでやればいいじゃない
/// 
/// Bulletではsceneがロードされる前にA関数(実行順をなるべく早くするため名前はA)を実行する。
/// 箱を生成し、AAクラスのAddクラス関数を実行。
/// </summary>
public class Bullet_2c1e33d6a205751a5f15cc2e590f603e : BulletBase
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void A()
    {
        GameObject myObj = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
        myObj.GetComponent<BoxCollider>().size = new(10000, 10000, 10000);
        AA.AddClass(myObj);
    }

    public override void Bang() { }
}

/// <summary>
/// AAクラスでは360度にレイを飛ばし、当たったものがplayerクラス持ちなら
/// Logic.DamageToTarget(dummy, hit.gameObjectを実行。
/// </summary>
class AA : MonoBehaviour
{
    int _numRays = 360;                // レイの本数
    float _maxDistance = 1000.0f;       // レイの最大距離
    List<Transform> _hits = new();

    private void Awake()
    {
        Raycast();
    }

    private void Update()
    {
        Raycast();
    }

    void Raycast()
    {
        float angleIncrement = 360.0f / _numRays;  // レイの角度間隔を計算
 
        for (int i = 0; i < _numRays; i++)
        {
            float angle = i * angleIncrement;        // レイの角度を計算
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward; // レイの向きを計算
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, _maxDistance))
            {
                // レイが何かに当たった場合の処理
                Debug.DrawLine(transform.position, hit.point, Color.red);
                if (!hit.transform.GetComponent<Bullet>())
                {
                    _hits.Add(hit.transform);
                }
            }
        }
        Attack();
    }

    void Attack()
    {
        foreach (var hit in _hits)
        {
            Damage(hit);
        }
        _hits.RemoveAll(hit => hit);
    }

    /// <summary>
    /// ダメージを与える処理を発行する
    /// Hpは10なので10回実行
    /// </summary>
    /// <param name="hit"></param>
    private static void Damage(Transform hit)
    {
        
            var dummy = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
            Logic.DamageToTarget(dummy, hit.gameObject); 
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤーもしくはプレイヤーの弾なら
        if (collision.gameObject.GetComponent<Bullet>()) return;

        //敵なら
        if (collision.gameObject.GetComponent<Player>())
        {
            Damage(collision.transform);
        }
        //敵の弾なら
        else
        {
            Destroy(collision.gameObject);
        }
    }

    public static void AddClass(GameObject to)
    {
        to.AddComponent<AA>();
    }
}
