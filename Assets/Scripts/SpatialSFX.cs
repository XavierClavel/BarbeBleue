using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpatialSFX : MonoBehaviour
{
    private AudioSource audioSource;

    private Camera _camera;
    private float prevDistance = Mathf.NegativeInfinity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        float newDistance = transform.position.x - _camera.transform.position.x;
        if (newDistance <= 0 && prevDistance > 0)
        {
            audioSource.Play();
            enabled = false;
        }
        prevDistance = newDistance;
    }
}
