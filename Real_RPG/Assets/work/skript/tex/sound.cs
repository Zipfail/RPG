using UnityEngine;

abstract public class sound : MonoBehaviour
{
    public AudioClip[] _sound;
    [SerializeField] protected int tipe;
    protected options op => GameObject.FindGameObjectWithTag("player").GetComponent<options>();
    protected AudioSource _aud => GetComponent<AudioSource>();

    public void playSound(AudioClip clip, float volume = 1f, bool destroued = false, float p1 = 0.85f, float p2 = 1.2f)
    {
        _aud.pitch = Random.Range(p1, p2);
        _aud.PlayOneShot(clip, op._sound(tipe));
    }

    public void _one_playSound(AudioClip clip, bool destroued = false, float p1 = 0.85f, float p2 = 1.2f)
    {
        if(!_aud.isPlaying)
        {
            _aud.pitch = Random.Range(p1, p2);
            _aud.PlayOneShot(clip, op._sound(tipe));
        }
    }

}
