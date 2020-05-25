using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraAnchor;

    public void StartShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }


    private IEnumerator Shake (float duration, float magnitude)
    {
        Vector2 originalPosition = new Vector2(cameraAnchor.position.x, cameraAnchor.position.y);
        float originalZRotation = cameraAnchor.rotation.z;

        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            float x = Random.Range(-1.0f, 1.0f) * magnitude;
            float y = Random.Range(-1.0f, 1.0f) * magnitude;
            float Z = Random.Range(-1.0f, 1.0f) * magnitude;    // Rotation

            cameraAnchor.localPosition = new Vector3(x,y, cameraAnchor.localPosition.z);
            cameraAnchor.localRotation = Quaternion.Euler(cameraAnchor.localRotation.x, cameraAnchor.localRotation.y, Z);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        cameraAnchor.localRotation = Quaternion.Euler(cameraAnchor.localRotation.x, cameraAnchor.localRotation.y, originalZRotation);
        cameraAnchor.localPosition = originalPosition;
    }
}
