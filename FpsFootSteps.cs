using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sonic.FPS
{
    
    public class FpsFootSteps : MonoBehaviour
    {
        private AudioSource audioSource;
        private CharacterController characterController;

        public AudioClip RunningClip;
        public AudioClip WalkingClip;

        public float RunningVolume=1;
        public float CrouchingVolume=1;
        public float ProningVolume=1;
        public float WalkingVolume=1;

        public float StepDistance = 0.5f;

        private float accumulatedDistance;

        private float currentVolume;
        private AudioClip currentClip;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            SetVolume();
            CheckToPlayFootstep();
        }

        private void CheckToPlayFootstep()
        {
            if (!characterController.isGrounded)
            {
                return;
            }
            if (characterController.velocity.sqrMagnitude > 0)
            {
                accumulatedDistance += Time.deltaTime;

                if (accumulatedDistance > StepDistance)
                {
                    if (audioSource.isPlaying && audioSource.clip == currentClip)
                        return;

                    audioSource.volume = currentVolume;
                    audioSource.clip = currentClip;
                    audioSource.Play();
                    accumulatedDistance = 0;
                }

            }
            else
            {
                if (audioSource.isPlaying)
                    audioSource.Stop();
                accumulatedDistance = 0;
            }
        }
        void SetVolume()
        {
            switch (FpsController.FpsPlayerState)
            {
                case PlayerState.Crouching:
                    currentVolume = CrouchingVolume;
                    break;
                case PlayerState.Walking:
                    currentVolume = WalkingVolume;
                    currentClip = WalkingClip;
                    break;
                case PlayerState.Running:
                    currentVolume = RunningVolume;
                    currentClip = RunningClip;
                    break;
                case PlayerState.Proning:
                    currentVolume = ProningVolume;
                    break;
            }
        }
    }
}