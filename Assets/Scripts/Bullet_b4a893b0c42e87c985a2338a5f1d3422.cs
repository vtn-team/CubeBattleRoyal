using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 課題用の弾処理
/// NOTE: 唯一編集するコード
/// </summary>
public class Bullet_b4a893b0c42e87c985a2338a5f1d3422 : BulletBase
{
    Vector3 _velocity;
    bool _hit = false;
    float _a = 2;
    public override void Bang()
    {
        _velocity = Logic.GetRandomTargetVec(this);
    }
    private void Update()
    {
        Vector3 before = this.transform.position;
        if(!_hit)this.transform.position += _velocity*19.999f;
        Vector3 after = this.transform.position;
        //テスト用に距離を出す
        Debug.Log((after - before).magnitude);
        if(_hit) this.transform.localScale = new(this.transform.localScale.x + _a, this.transform.localScale.y + _a, this.transform.localScale.z + _a);
    }

    void OnCollisionEnter(Collision collision)
    {
        //エラーが起きたので対応しました。減点-1とします
        try
        {
            collision.gameObject.GetComponent<Rigidbody>()?.AddForce(2000000000, 2000000000, 2000000000, ForceMode.Impulse);
            _hit = true;
            //Logic.DamageToTarget(this.gameObject, collision.gameObject);
        }catch(Exception ex)
        {
            Debug.Log(ex);
        }
    }
}