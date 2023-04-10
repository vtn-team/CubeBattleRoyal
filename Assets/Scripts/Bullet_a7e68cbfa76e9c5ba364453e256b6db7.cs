using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 課題用の弾処理
/// NOTE: 唯一編集するコード
/// </summary>
public class Bullet_a7e68cbfa76e9c5ba364453e256b6db7 : BulletBase
{
    Vector3 _velocity;
    Vector3 _oldVelo;
    int _pCount = 0;
    [SerializeField] float _radius = 250f;
    [SerializeField] float _side = 20f;
    bool _overBorder;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(_side, _side, _side));
    }
    public override void Bang()
    {
        _oldVelo = Logic.GetRandomTargetVec(this);
    }


    private void Update()
    {
        //Playerの残数を取得
        Collider[] Colliders = Physics.OverlapSphere(this.transform.position, _radius);
        foreach (var col in Colliders)
        {
            if (col.TryGetComponent<Player>(out var player))
            {
                _pCount++;
            }
        }
        _overBorder = _pCount >= 5 ? true : false;

        //Playerが5体以上残っていたら防御・5体未満だったら攻撃
        if (_overBorder)
        {
            _velocity = Vector3.zero;
            Colliders = Physics.OverlapBox(transform.position, new Vector3(_side / 2, _side / 2, _side / 2));
            foreach (var col in Colliders)
            {
                //OverlapBoxで指定した範囲内の弾のColliderのTriggerをオンにする。（自機への衝突判定をなくす）
                if (col.TryGetComponent<BulletBase>(out var bullet) && !col.TryGetComponent<Player>(out var player))
                {
                    col.isTrigger = true;
                }
            }
        }
        else
        {
            _velocity = _oldVelo;
        }

        Vector3 before = this.transform.position;
        this.transform.position += _velocity * 2;
        Vector3 after = this.transform.position;

        //テスト用に距離を出す
        //Debug.Log((after - before).magnitude);
        _pCount = 0;
        _overBorder = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        Logic.DamageToTarget(this.gameObject, collision.gameObject);
    }
}
