using UnityEngine;
using Debug = UnityEngine.Debug;
using Stopwatch = System.Diagnostics.Stopwatch;
using Type = System.Type;

public sealed class Benchmark : MonoBehaviour
{
    [SerializeField] int _instanceCount = 4096;
    [SerializeField] int _iterationCount = 128;

    static readonly Type[] ComponentTypes = 
      { typeof(TestComponent1), typeof(TestComponent2),
        typeof(TestComponent3), typeof(TestComponent4),
        typeof(TestComponent5), typeof(TestComponent6),
        typeof(TestComponent7), typeof(TestComponent8) };

    void Start()
    {
        var salt = Random.Range(0, _instanceCount - 1);
        for (var i = 0; i < _instanceCount; i++) CreateGameObject(i ^ salt);
        for (var i = 0; i < 8; i++) RunBenchmark(ComponentTypes[i]);
    }

    void CreateGameObject(int index)
    {
        var go = new GameObject("Instance");
        for (var i = 0; i < 8; i++)
            if ((index & ((1 << i) - 1)) == 0)
                go.AddComponent(ComponentTypes[i]);
    }

    void RunBenchmark(Type type)
    {
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

        var num = FindObjectsOfType(type).Length;
        var ratio = 1 - time2 / time1;

        Debug.Log($"{type} ({num}): {time1:F2} -> {time2:F2} ({ratio:P})");
    }
}
