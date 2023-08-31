using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int catchNumber;

    public int playerID;
    public float xLimit = 9;
    public float yLimit = 2.6f;
    public GameObject prefab;
    public Text displayText;

    public AudioSource audioSource;

    public AudioClip fashe;  // 音频剪辑1，可以在Inspector中关联
    public AudioClip zhuazhu;

    private void Start()
    {
        originalPosition = transform.position;
        AdjustStartTimeForSwinging();
    }

    private void Update()
    {
        displayText.text = "Player" + playerID.ToString()+ ": " + catchNumber.ToString();
        if (!isThrowing && !isRecovering)
        {
            
            float rotationOffset = swingAmplitude * Mathf.Sin((Time.time - startTime) * swingFrequency);
            transform.eulerAngles = new Vector3(0, 0, rotationOffset);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isThrowing && !isRecovering && playerID == 1)
        {
            isThrowing = true;
            throwDirection = -transform.up;
            audioSource.PlayOneShot(fashe);
        }
        if (Input.GetKey(KeyCode.Alpha2) && !isThrowing && !isRecovering && playerID == 1)
        {
            swingFrequency = 2.0f;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2) && !isThrowing && !isRecovering && playerID == 1)
        {
            swingFrequency = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !isThrowing && !isRecovering && playerID == 2)
        {
            isThrowing = true;
            throwDirection = -transform.up;
            audioSource.PlayOneShot(fashe);
        }
        if (Input.GetKey(KeyCode.RightArrow) && !isThrowing && !isRecovering && playerID == 2)
        {
            swingFrequency = 2.0f;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) && !isThrowing && !isRecovering && playerID == 2)
        {
            swingFrequency = 1.0f;
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
        if (isThrowing && other.gameObject.CompareTag("gold"))
        {
            StartRecovery();
            Destroy(other.gameObject);
            audioSource.PlayOneShot(zhuazhu);
            catchNumber++;
            // 随机生成一个位置
            Vector3 randomPosition = new Vector3(Random.Range(-xLimit, xLimit), Random.Range(-yLimit, yLimit), 0);

            // 使用随机位置来实例化prefab
            Instantiate(prefab, randomPosition, Quaternion.identity);
        }
    }

    private void StartRecovery()
    {
        isThrowing = false;
        isRecovering = true;
    }
}
