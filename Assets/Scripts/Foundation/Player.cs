using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �ۑ�p�v���C���[����
/// NOTE: �ҏW�͂ł��Ȃ��R�[�h
/// </summary>
public class Player : MonoBehaviour
{
    int _life = 10;
    BulletBase _bullet;

    public int Life => _life;

    void Update()
    {
        if (_life < 0)
        {
            //�N�\�d�����ǉۑ�̂���
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

    public void AssignBullet(string entryName, Type bullet)
    {
        _bullet = this.gameObject.AddComponent(bullet) as BulletBase;
        gameObject.name = entryName;
    }

    public void Damage(int damage)
    {
        _life -= damage;
    }
}
