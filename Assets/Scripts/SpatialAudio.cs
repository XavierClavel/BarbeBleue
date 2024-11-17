using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpatialAudio : MonoBehaviour
{
    private AudioSource audioSource;

    private Camera _camera;
    private const float distFullVolume = 1000;
    private const float distFade = 1500;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Mathf.Abs(transform.position.x - _camera.transform.position.x);

        if(distance < distFullVolume)
        {
            audioSource.volume = 1;
        }
        else if(distance > distFullVolume + distFade)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = 1 - ((distance - distFullVolume) / distFade);
        }
    }
}
