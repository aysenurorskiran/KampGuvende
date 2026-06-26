using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2.7f;
    public float turnSpeed = 130f;

    [Header("Yuruyus Sesi")]
    public AudioSource walkAudio;

    private CharacterController controller;
    public bool canMove = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (walkAudio != null)
        {
            walkAudio.playOnAwake = false;
            walkAudio.loop = true;
        }
    }

    void Update()
    {
        if (!canMove)
        {
            StopWalkSound();
            return;
        }

        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        transform.Rotate(0f, turnInput * turnSpeed * Time.deltaTime, 0f);

        Vector3 move = transform.forward * moveInput;
        controller.Move(move * speed * Time.deltaTime);

        if (Mathf.Abs(moveInput) > 0.1f)
            PlayWalkSound();
        else
            StopWalkSound();
    }

    void PlayWalkSound()
    {
        if (walkAudio != null && !walkAudio.isPlaying)
            walkAudio.Play();
    }

    void StopWalkSound()
    {
        if (walkAudio != null && walkAudio.isPlaying)
            walkAudio.Stop();
    }

    public void StopPlayer()
    {
        canMove = false;
        StopWalkSound();
    }

    public void StartPlayer()
    {
        canMove = true;
    }
}