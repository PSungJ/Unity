using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Transform shakeCamera;   //��鸲 ȿ���� �� ī�޶�
    public bool shakeRotate = false; //ȸ�� ���θ� �Ǵ��ϴ� bool����
    private Vector3 originPos = Vector3.zero;  //Shake�ϰ��� ���� ��ġ�� �ǵ��� ���� ����
    private Quaternion originRot = Quaternion.identity; //Shake�ϰ��� ���� ȸ���� �ǵ��� ���� ����
    void Start()
    {
        originPos = shakeCamera.transform.position;
        originRot = shakeCamera.transform.rotation;
    }
    public IEnumerator ShakeCamera(float duration = 0.05f, 
        float magnitudePos = 0.03f, 
        float magnitudeRot = 0.1f)
    {
        float passTime = 0.0f;  // �ð��� ������ ����
        //Shake �ð����� ������ ��ȸ�ϱ� ����
        while (passTime < duration)
        {   //�ұ�Ģ�� ��ġ�� ����
            Vector3 shakePos = Random.insideUnitSphere;
            shakeCamera.transform.position = shakePos * magnitudePos;
            if (shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0f, 0f, Mathf.PerlinNoise(Time.time * magnitudeRot, 0f));
                shakeCamera.rotation = Quaternion.Euler(shakeRot);
            }
            passTime += Time.deltaTime; // Shake�ð��� ����
            yield return null;
        }
        //Shake�� ������ ���� ��ġ�� �ʱ�ȭ
        shakeCamera.position = originPos;
        shakeCamera.rotation = originRot;
    }
}
