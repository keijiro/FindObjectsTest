using UnityEngine;
using Debug = UnityEngine.Debug;
using Stopwatch = System.Diagnostics.Stopwatch;
using Type = System.Type;
using System.Linq;

public sealed class Benchmark : MonoBehaviour
{
    [SerializeField] int _iterationCount = 64;

    static readonly Type[] ComponentTypes = 
      { typeof(TestComponent1), typeof(TestComponent2),
        typeof(TestComponent3), typeof(TestComponent4),
        typeof(TestComponent5), typeof(TestComponent6),
        typeof(TestComponent7), typeof(TestComponent8) };

    void Start()
    {
        var sheet = Enumerable.Range(0, 8)
          .Select(i => RunBenchmark(ComponentTypes[i]))
          .Aggregate((txt, line) => txt + "\n" + line);

        Debug.Log(sheet);
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
