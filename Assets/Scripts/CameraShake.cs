using UnityEngine;
using Cinemachine;
using System.Collections;
using Unity.VisualScripting;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    public CinemachineVirtualCamera virtualCamera;
    public float shakeDuration = 0.5f; // ��������� �����
    public float shakeAmplitude = 0.5f; // �������� �����

    private CinemachineTransposer transposer;
    private Vector3 originalOffset;
    private bool isShaking = false;

    void Start()
    {
        Instance = this;
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        originalOffset = transposer.m_FollowOffset;
    }

    public void StartShake()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }

    private IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            // ��������� ����������� ������� �� Y
            float yOffset = Random.Range(-shakeAmplitude, shakeAmplitude);
            Vector3 newOffset = new Vector3(originalOffset.x, originalOffset.y + yOffset, originalOffset.z);

            transposer.m_FollowOffset = newOffset;

            yield return null;
        }

        // ��������� ��������� �������� FollowOffset
        transposer.m_FollowOffset = originalOffset;
        isShaking = false;
    }
}
