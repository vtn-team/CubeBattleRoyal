using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 起動時に実行されるコード
/// </summary>
public class BattleRoyal : MonoBehaviour
{
    [SerializeField] GameObject _playerBase;
    [SerializeField] Cinemachine.CinemachineTargetGroup _targetGroup;
    [SerializeField] List<BulletBase> _entry;

    private void Awake()
    {
        //諸々設定
        Application.targetFrameRate = 60;
        Time.timeScale = 1.0f;
    }

    public void Start()
    {
        int maxDiv = Mathf.Max(32, _entry.Count);
        for (int i=0; i< maxDiv; ++i)
        {
            Vector3 position = Quaternion.AngleAxis(360 / maxDiv * i, Vector3.up) * new Vector3(0,0,-100);
            Quaternion rotation = Quaternion.identity;
            
            GameObject playerObj = GameObject.Instantiate(_playerBase, position, rotation);
            Player player = playerObj.GetComponent<Player>();
            playerObj.transform.LookAt(Vector3.zero);

            _targetGroup.AddMember(player.transform, 1, 20);

            if (i < _entry.Count)
            {
                player.AssignBullet(_entry[i]);
            }
            else
            {
                player.AssignBullet(new DefaultBullet());
            }
        }
    }
}

