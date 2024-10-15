using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthVisualizer : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private float _initialScale;
    private Quaternion _initialRotation;
    public Camera _cameraToLookAt;

    private void Start()
    {
        enemy.OnHealthChanged += OnHealthChanged;
        _initialScale = transform.localScale.x;
        _initialRotation = transform.rotation;
        _cameraToLookAt = Camera.main; 
    }

    private void Update()
    {
        transform.rotation = _initialRotation;
    }

    private void OnHealthChanged(float obj)
    {
        transform.localScale = new Vector3(_initialScale * obj, transform.localScale.y, transform.localScale.z);
    }
}
