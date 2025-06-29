using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Transform shakeCamera; //����ũ ȿ���� �� ī�޶�
    public bool shakeRotate = false; //ȸ�� �� �������� �Ǵ� �ϴ� �Һ���
    private Vector3 originPos = Vector3.zero; //����ũ �ϰ� ���� ���� ��ġ�� �ǵ��� ���� ����
    private Quaternion originRot = Quaternion.identity;//����ũ �ϰ� ���� ���� ȸ������ �ǵ��� ���ʹϾ� ����

    void Start()
    {
        originPos = shakeCamera.transform.position;
        originRot = shakeCamera.transform.rotation;
    }
    public IEnumerator ShakeCamera(float duration =0.05f
        ,float magnitudePos =0.03f
        ,float magnitudeRot =0.1f)
    {
        float passTime = 0.0f;//�ð��� ������ ���� 
        //����ũ �ð����� ������ ��ȸ �ϱ� ���� 
        while (passTime < duration)
        {   //�ұ�Ģ�� ��ġ�� ���� 
            Vector3 shakePos = Random.insideUnitSphere;
               float random =  Random.Range(0,20f);
            shakeCamera.transform.position = shakePos * magnitudePos;
            if(shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0f, 0f, Mathf.PerlinNoise(Time.time * magnitudeRot, 0f));
                shakeCamera.rotation = Quaternion.Euler(shakeRot);
            }
            passTime += Time.deltaTime; // ����ũ �ð��� ����
            yield return null;
        }
        //����ũ�� ������ ī�޶� ���� ��ġ�� ȸ������ ���� 
        shakeCamera.position = originPos;
        shakeCamera.rotation=originRot;
    }
  
    
}
