using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeMove : MonoBehaviour
{
    public float speed = 20f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SphereCollider s_col;
    [SerializeField] private Transform tr;
    public AudioSource source;
    public AudioClip exp_Clip;
    public GameObject exp_Eff;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        s_col = GetComponent<SphereCollider>();
        tr = GetComponent<Transform>();
        source = GetComponent<AudioSource>();
        exp_Clip = Resources.Load<AudioClip>("Sounds/missile_explosion");
    }

    private void OnEnable()
    {
        rb.AddForce(tr.forward * speed, ForceMode.Impulse);
        StartCoroutine(Explosion());
        Destroy(exp_Eff, 1f);
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(5f);
        source.PlayOneShot(exp_Clip);
        Instantiate(exp_Eff, transform);
    }
}
