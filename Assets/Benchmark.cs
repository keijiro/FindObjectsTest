using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Diagnostics;

public sealed class Benchmark : MonoBehaviour
{
    [SerializeField] int _instanceCount = 4096;
    [SerializeField] int _iterationCount = 128;

    void Start()
    {
        var components = new []
          { typeof(TestComponent1), typeof(TestComponent2),
            typeof(TestComponent3), typeof(TestComponent4),
            typeof(TestComponent5), typeof(TestComponent6),
            typeof(TestComponent7), typeof(TestComponent8) };

        for (var i = 0; i < _instanceCount; i++)
            new GameObject("Instance", components);

        var sw = new Stopwatch();

        sw.Start();
        var sum = 0;
        for (var i = 0; i < _iterationCount; i++)
            sum += FindObjectsOfType<TestComponent1>().Length;
        sw.Stop();

        var time1 = sw.Elapsed.TotalMilliseconds;

        sw.Reset();
        sw.Start();
        for (var i = 0; i < _iterationCount; i++)
            sum += FindObjectsByType<TestComponent1>(FindObjectsSortMode.None).Length;
        sw.Stop();

        var time2 = sw.Elapsed.TotalMilliseconds;

        Debug.Log($"FindObjectOfType: {time1:F2}");
        Debug.Log($"FindObjectByType: {time2:F2}");
        Debug.Log($"Gain: {1 - time2 / time1:P}");
    }
}
