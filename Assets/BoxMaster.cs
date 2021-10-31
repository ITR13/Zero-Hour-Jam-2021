using System.Collections.Generic;
using UnityEngine;

public class BoxMaster : MonoBehaviour
{
    [SerializeField] private BoxScript _boxPrefab;

    private Dictionary<Vector2Int, BoxScript> _boxes = new Dictionary<Vector2Int, BoxScript>();
    private void Awake()
    {
        SpawnBox(Vector2Int.zero);
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
    }
}
