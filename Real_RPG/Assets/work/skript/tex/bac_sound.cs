using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class bac_sound : sound
{
    private int count;
    private float vol;
    private void Start()
    {
        vol = op._sound(tipe);
    }
    void Update()
    {
        if(vol != op._sound(tipe))
        {
            _aud.Stop();
            _one_playSound(_sound[count]);
            count++;
            vol = op._sound(tipe);
        }
        if(!_aud.isPlaying)
        {
            _one_playSound(_sound[count]);
            count++;
        }
        if(count >= _sound.Count())
        {
            count = 0;
        }
    }
}
