using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxMaster : MonoBehaviour
{
    [SerializeField] private BoxScript _boxPrefab;
    [SerializeField] private Transform _targetPrefab;

    [SerializeField] private GameObject _victoryScreen;

    private static int _currentLevel;

    private readonly string[] _levels = new[]
    {
        "1:0;0:-1;-1:0;0:2;1:1;-1:1", // tutorial
        "0:-1;1:1;-1:1;0:3;-1:3;1:3;2:3;3:2;3:1;3:0;2:-1;1:-1;-2:3;-3:2;-3:1;-2:-1;-3:0;-1:-1", // tutorial 2
        "0:-1;-1:0;-2:1;0:5;1:5;-3:2;-4:3;-3:5;-4:4;-1:6;-2:6;2:6;3:6;4:5;2:0;1:-1;3:1;4:2;5:3;5:4", // heart
    };

    private Dictionary<Vector2Int, BoxScript> _boxes = new Dictionary<Vector2Int, BoxScript>();

    private HashSet<Vector2Int> target;

    private void Awake()
    {
        SpawnBox(Vector2Int.zero);
        if (_currentLevel >= _levels.Length)
        {
            _currentLevel = 0;
            _victoryScreen.SetActive(true);
            return;
        }

        SpawnLevel(_levels[_currentLevel]);
    }

    private void SpawnLevel(string level)
    {
        target = new HashSet<Vector2Int>();
        var positions = level.Split(";");
        foreach (var pos in positions)
        {
            var posSplit = pos.Split(":");
            var x = int.Parse(posSplit[0]);
            var y = int.Parse(posSplit[1]);

            Instantiate(_targetPrefab, new Vector3(x, y, -1), Quaternion.identity);
            target.Add(new Vector2Int(x, y));
        }
    }

    private void SpawnBox(Vector2Int position)
    {
        var box = Instantiate(_boxPrefab, transform);
        box.transform.position = (Vector2)position;
        _boxes.Add(position, box);

        box.OnClick = () => Clicked(position);
    }

    private void Clicked(Vector2Int pos)
    {
        _boxes[pos].gameObject.SetActive(false);

        foreach (var delta in new[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left })
        {
            var newPos = pos + delta;
            if (_boxes.ContainsKey(newPos)) continue;
            SpawnBox(newPos);
        }

        if (target.SetEquals(_boxes.Where(pair => pair.Value.gameObject.activeSelf).Select(pair => pair.Key)))
        {

        }
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.P)) return;
        var keys = string.Join(";", .Select(pair => $"{pair.Key.x}:{pair.Key.y}"));
        Debug.Log(keys);
    }
}
