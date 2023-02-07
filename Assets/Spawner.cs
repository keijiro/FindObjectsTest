using UnityEngine;
using Type = System.Type;

public sealed class Spawner : MonoBehaviour
{
    [SerializeField] int _instanceCount = 4096;

    static readonly Type[] ComponentTypes =
      { typeof(TestComponent1), typeof(TestComponent2),
        typeof(TestComponent3), typeof(TestComponent4),
        typeof(TestComponent5), typeof(TestComponent6),
        typeof(TestComponent7), typeof(TestComponent8) };

    void Start()
    {
        var salt = Random.Range(0, _instanceCount - 1);

        for (var i = 0; i < _instanceCount; i++)
            CreateGameObject(i ^ salt);
    }

    void CreateGameObject(int index)
    {
        var go = new GameObject("Instance");
        for (var i = 0; i < 8; i++)
            if ((index & ((1 << i) - 1)) == 0)
                go.AddComponent(ComponentTypes[7 - i]);
    }
}
