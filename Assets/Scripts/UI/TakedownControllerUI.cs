using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakedownControllerUI : MonoBehaviour
{
    public static TakedownControllerUI Instance;

    [SerializeField] private TakedownUI takedownUIprefab;
    [SerializeField] private Transform parent;

    public void Awake()
    {
        Instance = this;
    }

    public void SpawnTakedown(string text)
    {
        var instance = Instantiate(takedownUIprefab, parent);
        instance.Init(text);
    }
}
