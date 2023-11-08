using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    public ParticleSystem particle;
    float _time;
    public float duration;
    bool _isPlay;
    private void Awake()
    {
        Stop();
    }
    // Start is called before the first frame update
    void Start()
    {
        //particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPlay) return;

        _time += Time.deltaTime;
        if(_time > duration)
        {
            Stop();
        }
    }
    public void Play()
    {
        if (!particle) return;
        particle.Play();
        _time = 0;
        _isPlay = true;
    }
    public void Stop()
    {
        if (!particle) return;
        particle.Stop();
        _time = 0;
        _isPlay = false;
    }
}
