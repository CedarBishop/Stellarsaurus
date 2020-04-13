using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void StartShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }


    private IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float originalZRotation = transform.rotation.z;

        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            float x = Random.Range(-1.0f, 1.0f) * magnitude;
            float y = Random.Range(-1.0f, 1.0f) * magnitude;
            float Z = Random.Range(-1.0f, 1.0f) * magnitude;

            transform.localPosition = new Vector3(x,y, originalPosition.z);
            transform.localRotation = Quaternion.Euler(transform.localRotation.x,transform.localRotation.y, Z);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, originalZRotation);
        transform.localPosition = originalPosition;
    }
}
