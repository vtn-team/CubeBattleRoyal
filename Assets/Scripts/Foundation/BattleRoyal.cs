using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// 起動時に実行されるコード
/// </summary>
public class BattleRoyal : MonoBehaviour
{
    [Serializable]
    class BulletEntry
    {
        public string Name;
        public BulletBase Bullet;
    }

    [SerializeField] GameObject _playerBase;
    [SerializeField] GameObject _namePlate;
    [SerializeField] GameObject _canvas;
    [SerializeField] UnityEngine.UI.Text _nameList;
    [SerializeField] Cinemachine.CinemachineTargetGroup _targetGroup;
    [SerializeField] List<BulletEntry> _entry;

    const int SHOT_INTERVAL = 10;
    int _shotCount = 0;

    private void Awake()
    {
        //諸々設定
        Application.targetFrameRate = 60;
        Time.timeScale = 1.0f;
    }

    void Start()
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
                GameObject namePlate = GameObject.Instantiate(_namePlate, _canvas.transform);
                NamePlate np = namePlate.GetComponent<NamePlate>();
                np.Setup(playerObj, _entry[i].Name);

                player.AssignBullet(_entry[i].Name, _entry[i].Bullet.GetType());
            }
            else
            {
                GameObject namePlate = GameObject.Instantiate(_namePlate, _canvas.transform);
                NamePlate np = namePlate.GetComponent<NamePlate>();
                np.Setup(playerObj, "NPC");

                player.AssignBullet("NPC", typeof(DefaultBullet));
            }
        }
    }

    private void Update()
    {
        _shotCount++;

        string nameList = "";
        if (_shotCount >= SHOT_INTERVAL)
        {
            var list = GameObject.FindObjectsOfType<Player>();
            foreach (var p in list)
            {
                p.Shot();
            }
            _shotCount = 0;
        }

        var plList = GameObject.FindObjectsOfType<Player>();
        foreach (var p in plList)
        {
            var entry = _entry.Where(b => b.Bullet.GetType().ToString() == p.name);
            if(entry.Count() > 0)
            {
                nameList += entry.Single().Name + "\n";
            }
            else
            {
                nameList += p.name + "\n";
            }
        }
        _nameList.text = nameList;
    }
}

