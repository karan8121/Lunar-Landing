using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    private Lander lander;

    [SerializeField] private AudioSource thrusterAudioSource;

    private void Awake()
    {
        lander = GetComponent<Lander>();
    }

    private void Start()
    {
        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnLeftForce += Lander_OnLeftForce;

        thrusterAudioSource.Pause();
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        thrusterAudioSource.Play();
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        thrusterAudioSource.Play();
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        thrusterAudioSource.Play();
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        thrusterAudioSource.Pause();
    }
}