using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 仮想的用の弾処理
/// </summary>
public class DefaultBullet : BulletBase
{
    Vector3 _velocity;

    public override void Bang()
    {
        _velocity = Logic.GetRandomTargetVec(this);
    }

    private void Update()
    {
        this.transform.position += _velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        Logic.DamageToTarget(this.gameObject, collision.gameObject);
    }
}
