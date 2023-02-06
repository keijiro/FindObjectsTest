using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stopwatch = System.Diagnostics.Stopwatch;
using Type = System.Type;

public sealed class RandomBenchmark : MonoBehaviour
{
    [SerializeField] int _iterationCount = 128;

    static readonly Type[] ComponentTypes = 
      { typeof(TestComponent1), typeof(TestComponent2),
        typeof(TestComponent3), typeof(TestComponent4),
        typeof(TestComponent5), typeof(TestComponent6),
        typeof(TestComponent7), typeof(TestComponent8) };

    List<GameObject> _instances = new List<GameObject>();

    IEnumerator Start()
    {
        for (var i = 0; i < 128 * 32; i++)
        {
            for (var j = 0; j < 128 / 32; j++)
            {
                CreateObjectRandom();
                CreateObjectRandom();
                DestroyObjectRandom();
            }
            yield return null;
        }

        var sheet = Enumerable.Range(0, 8)
          .Select(i => RunBenchmark(ComponentTypes[i]))
          .Aggregate((txt, line) => txt + "\n" + line);

        Debug.Log(sheet);

        Camera.main.backgroundColor = Color.red;
    }

    void CreateObjectRandom()
    {
        var go = new GameObject();
        for (var i = 0; i < ComponentTypes.Length; i++)
            if (Random.value < (i + 1.0f) / ComponentTypes.Length) go.AddComponent(ComponentTypes[i]);
        _instances.Add(go);
    }

    void DestroyObjectRandom()
    {
        var index = Random.Range(0, _instances.Count);
        Destroy(_instances[index]);
        _instances.RemoveAt(index);
    }

    string RunBenchmark(Type type)
    {
        var num = FindObjectsOfType(type).Length;

        var sw = new Stopwatch();

        sw.Start();
        for (var i = 0; i < _iterationCount; i++)
            FindObjectsOfType(type);
        sw.Stop();

        var time1 = sw.Elapsed.TotalMilliseconds;

        sw.Reset();
        sw.Start();
        for (var i = 0; i < _iterationCount; i++)
            FindObjectsByType(type, FindObjectsSortMode.None);
        sw.Stop();

        var time2 = sw.Elapsed.TotalMilliseconds;

        return $"{type},{num},{time1:F2},{time2:F2}";
    }
}
