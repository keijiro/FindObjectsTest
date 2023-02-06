using UnityEngine;
using UnityEditor;
using Type = System.Type;

static class BenchmarkTool
{
    const int InstanceCount = 1024 * 16;

    static readonly Type[] ComponentTypes = 
      { typeof(TestComponent1), typeof(TestComponent2),
        typeof(TestComponent3), typeof(TestComponent4),
        typeof(TestComponent5), typeof(TestComponent6),
        typeof(TestComponent7), typeof(TestComponent8) };

    [MenuItem("Benchmark/Build Test Scene")]
    static void BuildTestScene()
    {
        var salt = Random.Range(0, InstanceCount - 1);

        for (var i = 0; i < InstanceCount; i++)
            CreateGameObject(i ^ salt);
    }

    static void CreateGameObject(int index)
    {
        var go = new GameObject("Instance");
        Undo.RegisterCreatedObjectUndo(go, "Create GameObject");

        for (var i = 0; i < 8; i++)
            if ((index & ((1 << i) - 1)) == 0)
                go.AddComponent(ComponentTypes[7 - i]);

        Undo.RegisterCompleteObjectUndo(go, "Add Components");
    }
}
