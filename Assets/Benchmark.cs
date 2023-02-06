using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using Debug = UnityEngine.Debug;
using Stopwatch = System.Diagnostics.Stopwatch;
using Type = System.Type;

public sealed class Benchmark : MonoBehaviour
{
    [SerializeField] int _instanceCount = 1024 * 8;
    [SerializeField] int _iterationCount = 128;

    static readonly Type[] ComponentTypes = 
      { typeof(TestComponent1), typeof(TestComponent2),
        typeof(TestComponent3), typeof(TestComponent4),
        typeof(TestComponent5), typeof(TestComponent6),
        typeof(TestComponent7), typeof(TestComponent8) };

    void Start()
    {
        for (var i = 0; i < _instanceCount; i++)
            (new GameObject())
              .AddComponent(ComponentTypes[i % ComponentTypes.Length]);

        var sheet = Enumerable.Range(0, 8)
          .Select(i => RunBenchmark(ComponentTypes[i]))
          .Aggregate((txt, line) => txt + "\n" + line);

        Debug.Log(sheet);
    }

    string RunBenchmark(Type type)
    {
        var num = FindObjectsOfType(type).Length;
        var sw = new Stopwatch();

#if !UNITY_EDITOR
        GarbageCollector.GCMode = GarbageCollector.Mode.Manual;
#endif

        sw.Start();
        for (var i = 0; i < _iterationCount; i++)
            FindObjectsOfType(type);
        sw.Stop();

        var time1 = sw.Elapsed.TotalMilliseconds;

        System.GC.Collect();

        sw.Reset();
        sw.Start();
        for (var i = 0; i < _iterationCount; i++)
            FindObjectsByType(type, FindObjectsSortMode.None);
        sw.Stop();

#if !UNITY_EDITOR
        GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
#endif

        var time2 = sw.Elapsed.TotalMilliseconds;

        return $"{type},{num},{time1:F2},{time2:F2}";
    }
}
