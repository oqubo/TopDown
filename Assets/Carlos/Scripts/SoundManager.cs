using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private AudioClip fxPerderVida, fxGanarPuntos;
    private AudioSource audioSource;
    
    private void OnEnable()
    {
        pc.eventoQuitarVida += PlaySoundPerderVida;
        pc.eventoAumentarPuntos += PlaySoundGanarPuntos;
        
    }

    private void OnDisable()
    {
        pc.eventoQuitarVida -= PlaySoundPerderVida;
        pc.eventoAumentarPuntos -= PlaySoundGanarPuntos;
    }

    private void PlaySoundPerderVida()
    {
        audioSource.PlayOneShot(fxPerderVida);
    }

    private void PlaySoundGanarPuntos()
    {
        audioSource.PlayOneShot(fxGanarPuntos);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
