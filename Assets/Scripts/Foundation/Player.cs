using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 課題用プレイヤー処理
/// NOTE: 編集はできないコード
/// </summary>
public class Player : MonoBehaviour
{
    int _life = 10;

    [SerializeField] string name = "";

    BulletBase _bullet;

    public int Life => _life;

    void Update()
    {
        if (_life < 0)
        {
            //クソ重いけど課題のため
            var list = GameObject.FindObjectsOfType<Player>();
            if (list.Length > 1)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    public void Shot()
    {
        ShotQueue.Instance.Enqueue(this.gameObject, _bullet.GetType());
    }

    public void AssignBullet(Type bullet)
    {
        _bullet = this.gameObject.AddComponent(bullet) as BulletBase;
        this.name = bullet.ToString();
        gameObject.name = this.name;
    }

    public void Damage(int damage)
    {
        _life -= damage;
    }
}
