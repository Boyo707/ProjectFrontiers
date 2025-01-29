using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void OnEnable()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOneShot(AudioClip clip, bool isPitched)
    {
        if (isPitched)
        {
            audioSource.pitch = Random.Range(0.4f, 1.4f);
        }
        audioSource.PlayOneShot(clip);
    }


    private void OnDisable()
    {
        
    }
}
