using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameVFX : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem flameVFX;

    // Start is called before the first frame update
    void Start()
    {
        flameVFX.Stop();
    }

    public void StartFireVFX()
    {
        flameVFX.Play();
    } 

    public void StopFireVFX()
    {
        flameVFX.Stop();
    }
}
