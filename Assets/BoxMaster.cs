using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxMaster : MonoBehaviour
{
    [SerializeField] private BoxScript _boxPrefab;
    [SerializeField] private Transform _targetPrefab;

    [SerializeField] private GameObject _victoryScreen;

    private static int _currentLevel;

    private readonly string[] _levels = new[]
    {
        "0:2;1:0;0:-2;-1:0;1:1;1:-1;-1:1;-1:-1", // Tutorial 1
        "-1:2;0:2;1:0;0:0;1:3;2:2;0:-1;-1:3;1:2;-2:2;-1:0", // Heart
    };

    private Dictionary<Vector2Int, BoxScript> _boxes = new Dictionary<Vector2Int, BoxScript>();

    private HashSet<Vector2Int> target;
    private bool stopped;
    private float t;

    private void Awake()
    {
        if (_currentLevel >= _levels.Length)
        {
            _currentLevel = 0;
            _victoryScreen.SetActive(true);
            return;
        }

        SpawnLevel(_levels[_currentLevel]);
        SpawnBox(Vector2Int.zero);
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
        if (stopped) return;
        Destroy(_boxes[pos].gameObject);
        _boxes.Remove(pos);

        foreach (var delta in new[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left })
        {
            var newPos = pos + delta;

            if (_boxes.ContainsKey(newPos))
            {
                Destroy(_boxes[newPos].gameObject);
                _boxes.Remove(newPos);
            }
            else
            {
                SpawnBox(newPos);
            }
        }

        if (target.SetEquals(_boxes.Where(pair => pair.Value.gameObject.activeSelf).Select(pair => pair.Key)))
        {
            _currentLevel++;
            stopped = true;
            t = 1;
        }
    }

    private void Update()
    {
        if (t > 0)
        {
            t -= Time.deltaTime;
            if (t > 0) return;
            SceneManager.LoadScene(0);
            return;
        }

        if (!Input.GetKeyDown(KeyCode.P)) return;
        var keys = string.Join(";", _boxes.Where(pair => pair.Value.gameObject.activeSelf).Select(pair => $"{pair.Key.x}:{pair.Key.y}"));
        Debug.Log(keys);
    }
}
