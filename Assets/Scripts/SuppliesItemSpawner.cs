using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BuffsScript;
using BuffsScript.UsingEffect;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;


public class PointInPolygon
{
    private Transform[] _spawnAreaPoints;
    private float _heightOfPoints;
    private (Vector3, Vector3, Vector3, Vector3) _corners;

    public PointInPolygon(Transform[] spawnAreaPoints, float heightOfPoints)
    {
        _spawnAreaPoints = spawnAreaPoints;
        _heightOfPoints = heightOfPoints;
        _corners = FindCorners(_spawnAreaPoints);
    }


    public Vector3 GetPoint()
    {
        var randPoint = RandomPoint(_corners);

        while (!IsPointInPolygon4(VectorPoints(_spawnAreaPoints), randPoint))
        {
            randPoint = RandomPoint(_corners);
        }

        return randPoint;
    }

    private Vector3[] VectorPoints(Transform[] points)
    {
        return points.Select(x => x.position).ToArray();
    }

    private Vector3 RandomPoint((Vector3 leftDown, Vector3 leftUp, Vector3 rightUp, Vector3 rightDown) corners)
    {
        var horizontal = Random.Range(corners.leftDown.x, corners.rightDown.x);
        var vertical = Random.Range(corners.leftDown.z, corners.leftUp.z);
        return new Vector3(horizontal, _heightOfPoints, vertical);
    }

    private static bool IsPointInPolygon4(Vector3[] polygon, Vector3 testPoint)
    {
        bool result = false;
        int j = polygon.Count() - 1;
        for (int i = 0; i < polygon.Count(); i++)
        {
            if (polygon[i].z < testPoint.z && polygon[j].z >= testPoint.z ||
                polygon[j].z < testPoint.z && polygon[i].z >= testPoint.z)
            {
                if (polygon[i].x + (testPoint.z - polygon[i].z) / (polygon[j].z - polygon[i].z) *
                    (polygon[j].x - polygon[i].x) < testPoint.x)
                {
                    result = !result;
                }
            }

            j = i;
        }

        return result;
    }


    private (Vector3, Vector3, Vector3, Vector3) FindCorners(Transform[] points)
    {
        float left = points[0].position.x;
        float up = points[0].position.y;
        float right = points[0].position.x;
        float down = points[0].position.y;

        for (int i = 1; i < points.Length; i++)
        {
            if (points[i].position.z > up)
                up = points[i].position.z;

            if (points[i].position.z < down)
                down = points[i].position.z;

            if (points[i].position.x > right)
                right = points[i].position.x;

            if (points[i].position.x < left)
                left = points[i].position.x;
        }

        return (
            new Vector3(left, _heightOfPoints, down),
            new Vector3(left, _heightOfPoints, up),
            new Vector3(right, _heightOfPoints, up),
            new Vector3(right, _heightOfPoints, down)
        );
    }
}

public class SuppliesItemSpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Точки области спавна")]
    private Transform[] _spawnAreaPoints;


    // [SerializeField]
    private float _heightOfPoints => transform.position.y;

    [SerializeField]
    [Tooltip("Случайное значение периода спавна припасов")]
    private Vector2 _randomSpawnSuppliesPeriod;

    [SerializeField]
    [Tooltip("Случайное значение периода спавна бафов")]
    private Vector2 _randomSpawnBuffPeriod;

    [SerializeField]
    [Tooltip("Ссылка на префаб припасов")]
    private SuppliesItem _suppliesItemPrefab;
    
    [SerializeField]
    [Tooltip("Случайное значение количества припасов в мешке")]
    private Vector2 _randomSuppliesItemValue;

    [SerializeField]
    [Tooltip("Ссылка на префабы бафов")]
    private Buff[] _buffsPrefab;

    [SerializeField]
    [Tooltip("Ссылка на слой земли чтобы припасы спавнялись на ней")]
    private LayerMask _groundMask;

    private PointInPolygon _pointInPolygon;

    private float _itemsYPosition;

    void Start()
    {
        _pointInPolygon = new PointInPolygon(_spawnAreaPoints, _heightOfPoints);
        StartCoroutine(SpawningSupplies());
        StartCoroutine(SpawningBuffs());

        if (Physics.Raycast(transform.position, Vector3.down, out var hit, Mathf.Infinity, _groundMask))
        {
            _itemsYPosition = hit.point.y;
        }
    }

    private IEnumerator SpawningSupplies()
    {
        while (true)
        {
            var randomPoint = _pointInPolygon.GetPoint();
            randomPoint.y = _itemsYPosition + _suppliesItemPrefab.Size.y / 2;
            var newSupplies = Instantiate(_suppliesItemPrefab, randomPoint,
                Quaternion.identity);
            newSupplies.SetAmount(Random.Range(_randomSuppliesItemValue.x,_randomSuppliesItemValue.y));
            yield return new WaitForSeconds(Random.Range(_randomSpawnSuppliesPeriod.x,_randomSpawnSuppliesPeriod.y));
        }
    }

    private IEnumerator SpawningBuffs()
    {
        while (true)
        {
            var randomPrefab = _buffsPrefab[Random.Range(0, _buffsPrefab.Length)];
            var randomPoint = _pointInPolygon.GetPoint();
            randomPoint.y = _itemsYPosition + 1;
            
            Instantiate(randomPrefab, randomPoint,
                Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(_randomSpawnBuffPeriod.x,_randomSpawnBuffPeriod.y));
        }
    }


    private void OnDrawGizmos()
    {
        if (_spawnAreaPoints == null)
            return;
        for (int i = 1; i < _spawnAreaPoints.Length; i++)
        {
            if (_spawnAreaPoints[i] == null)
                return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(ConvertedAreaPoint(_spawnAreaPoints[i - 1].position),
                ConvertedAreaPoint(_spawnAreaPoints[i].position));
        }

        Gizmos.DrawLine(ConvertedAreaPoint(_spawnAreaPoints[_spawnAreaPoints.Length - 1].position),
            ConvertedAreaPoint(_spawnAreaPoints[0].position));
    }

    private Vector3 ConvertedAreaPoint(Vector3 point)
    {
        return new Vector3(point.x, _heightOfPoints, point.z);
    }
}