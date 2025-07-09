using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ũ ü�� �� �����̰� �ϱ�
public class TrackAni : MonoBehaviour
{
    private float _scrollSpeed = 1.0f;
    private MeshRenderer _meshRenderer;
    private TankInput input;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        input = GetComponentInParent<TankInput>();
    }

    void Update()
    {
        var offset = Time.time * _scrollSpeed * input.axisRaw;
        _meshRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, offset)); // _MainTex : �Ϲ� Base Texture
        _meshRenderer.material.SetTextureOffset("_BumpMap", new Vector2(0f, offset)); // _BumpMap : NormalMap Texture
    }
}
