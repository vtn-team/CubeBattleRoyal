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
    const int SHOT_INTERVAL = 10;
    int _shotCount = 0;
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
    void FixedUpdate()
    {
        _shotCount++;
        if(_shotCount >= SHOT_INTERVAL)
        {
            Shot();
            _shotCount = 0;
        }
    }

    void Shot()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        BulletBase script = cube.AddComponent(_bullet.GetType()) as BulletBase;
        cube.transform.position = this.transform.position + this.transform.forward * this.transform.localScale.z;
        script.Bang();
    }

    public void AssignBullet(BulletBase bullet)
    {
        _bullet = bullet;
        name = bullet.GetType().ToString();
    }

    public void Damage(int damage)
    {
        _life -= damage;
    }
}
