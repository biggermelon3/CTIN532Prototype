using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objCont : MonoBehaviour
{
    public float swingAmplitude = 30f;
    public float swingFrequency = 1.0f;
    public float throwSpeed = 10.0f;
    public float recoverySpeed = 5.0f;
    public float maxThrowDistance = 10.0f;

    private bool isThrowing = false;
    private bool isRecovering = false;
    private Vector3 originalPosition;
    private Vector3 throwDirection;
    private float startTime; //记录开始摇摆的时间

    private void Start()
    {
        originalPosition = transform.position;
        AdjustStartTimeForSwinging();
    }

        private void Update()
    {
        if (!isThrowing && !isRecovering)
        {
            float rotationOffset = swingAmplitude * Mathf.Sin((Time.time - startTime) * swingFrequency);
            transform.eulerAngles = new Vector3(0, 0, rotationOffset);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isThrowing && !isRecovering)
        {
            isThrowing = true;
            throwDirection = -transform.up;
        }

        if (isThrowing)
        {
            transform.position += throwDirection * throwSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, originalPosition) >= maxThrowDistance)
            {
                StartRecovery();
            }
        }

        if (isRecovering)
        {
            Vector3 directionBack = (originalPosition - transform.position).normalized;
            transform.position += directionBack * recoverySpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, originalPosition) < 0.2f)
            {
                transform.position = originalPosition;
                isRecovering = false;
                AdjustStartTimeForSwinging();
            }
        }
    }

    private void AdjustStartTimeForSwinging()
    {
        float currentAngle = transform.eulerAngles.z;
        if (currentAngle > 180) currentAngle -= 360; // Adjust for Unity's 0-360 angle system
        float normalizedAngle = currentAngle / swingAmplitude;

        // Ensure the value is within [-1, 1] to avoid errors with Mathf.Asin
        normalizedAngle = Mathf.Clamp(normalizedAngle, -1f, 1f);

        startTime = Time.time - (Mathf.Asin(normalizedAngle) / (2 * Mathf.PI * swingFrequency));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isThrowing)
        {
            StartRecovery();
        }
    }

    private void StartRecovery()
    {
        isThrowing = false;
        isRecovering = true;
    }
}
