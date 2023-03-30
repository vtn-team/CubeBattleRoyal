using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 課題用の弾処理
/// NOTE: 唯一編集するコード
/// </summary>
public class Bullet : BulletBase
{
    Vector3 _velocity;

    public override void Bang()
    {
        _velocity = Logic.GetRandomTargetVec(this);
    }

    private void Update()
    {
        Vector3 before = this.transform.position;
        this.transform.position += _velocity*2;
        Vector3 after = this.transform.position;

        //テスト用に距離を出す
        //Debug.Log((after - before).magnitude);
    }

    void OnCollisionEnter(Collision collision)
    {
        Logic.DamageToTarget(this.gameObject, collision.gameObject);
    }
}
