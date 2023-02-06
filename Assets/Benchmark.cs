using UnityEngine;
using Debug = UnityEngine.Debug;
using Stopwatch = System.Diagnostics.Stopwatch;
using Type = System.Type;
using System.Linq;
using System.Collections;

public sealed class Benchmark : MonoBehaviour
{
    [SerializeField] int _instanceCount = 1024 * 8;
    [SerializeField] int _iterationCount = 128;

    static readonly Type[] ComponentTypes = 
      { typeof(TestComponent1), typeof(TestComponent2),
        typeof(TestComponent3), typeof(TestComponent4),
        typeof(TestComponent5), typeof(TestComponent6),
        typeof(TestComponent7), typeof(TestComponent8) };

    IEnumerator Start()
    {
        var list = Enumerable.Range(0, _instanceCount)
          .Select(x => CreateFullGameObject(x)).ToList();

        var salt = Random.Range(0, _instanceCount - 1);

        for (var i = 0; i < _instanceCount; i++)
            CullComponents(list[i], i ^ salt);

        yield return null;

        var sheet = Enumerable.Range(0, 8)
          .Select(i => RunBenchmark(ComponentTypes[i]))
          .Aggregate((txt, line) => txt + "\n" + line);

        Debug.Log(sheet);
    }

    GameObject CreateFullGameObject(int index)
      => new GameObject("Instance", ComponentTypes);

    void CullComponents(GameObject go, int mask)
    {
        for (var i = 0; i < 8; i++)
            if ((mask & ((1 << i) - 1)) != 0)
                Destroy(go.GetComponent(ComponentTypes[7 - i]));
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
