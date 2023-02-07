using UnityEngine;
using UnityEditor;
using Stopwatch = System.Diagnostics.Stopwatch;
using Type = System.Type;

static class BenchmarkTool
{
    const int IterationCount = 50;

    static long TestOldAPI<T>() where T : Object
    {
        var sw = new Stopwatch();
        System.GC.Collect();
        sw.Start();
        Object.FindObjectsOfType<T>();
        sw.Stop();
        return sw.Elapsed.Ticks;
    }

    static long TestNewAPI<T>() where T : Object
    {
        var sw = new Stopwatch();
        System.GC.Collect();
        sw.Start();
        Object.FindObjectsByType<T>(FindObjectsSortMode.None);
        sw.Stop();
        return sw.Elapsed.Ticks;
    }

    static string RunBenchmark<T>() where T : Object
    {
        var num = Object.FindObjectsByType<T>(FindObjectsSortMode.None).Length;
        var acc = (x:0L, y:0L);

        for (var i = 0; i < IterationCount; i++)
        {
            acc.x += TestOldAPI<T>();
            acc.y += TestNewAPI<T>();
        }

        acc.x /= IterationCount;
        acc.y /= IterationCount;

        const double div = System.TimeSpan.TicksPerMillisecond;
        return $"{typeof(T)},{num},{acc.x / div},{acc.y / div}";
    }

    [MenuItem("Benchmark/Run")]
    static void MenuItemRun()
    {
        var text = "";
        text += RunBenchmark<TestComponent1>() + "\n";
        text += RunBenchmark<TestComponent2>() + "\n";
        text += RunBenchmark<TestComponent3>() + "\n";
        text += RunBenchmark<TestComponent4>() + "\n";
        text += RunBenchmark<TestComponent5>() + "\n";
        text += RunBenchmark<TestComponent6>() + "\n";
        text += RunBenchmark<TestComponent7>() + "\n";
        text += RunBenchmark<TestComponent8>();
        Debug.Log(text);
    }
}
