using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private LineRenderer lineRenderer;

    public float damage = 10f;
    public float range = 100f;
    public float shootDelay = 0.3f;

    float shootTimer;

    public GameObject barrel;
    public AudioClip shootSound;

    public Camera playerView;

    void Start()
    {
        //get animator and audio components of the model
        animator = GetComponentInParent<Animator>();
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        //make the animation be idle
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if(info.IsName("M14Shoot")) animator.SetBool("Shoot", false);
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        //add time to a timer if the player hasnt shot recently
        if(shootTimer < shootDelay) shootTimer += Time.deltaTime;
    }
    
    void Shoot()
    {
        

        //if the timer hasnt reached a specific time the player doesnt shoot
        if(shootTimer < shootDelay) return;

        //reset the timer since the player just shot
        shootTimer = 0f;
        
        //start playing the shoot animation
        animator.SetBool("Shoot", true);

        //this will make sure the raycast goes directly to the center of the screen, also it helps lineRenderer get a clear end position.
        Vector3 aimSpot = playerView.transform.position;
        aimSpot += playerView.transform.forward * 50f;

        RaycastHit hit;
        if(Physics.Raycast(barrel.transform.position, aimSpot, out hit, range))
        {
            //get raycast destination target and have it take damage
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }
        }

        lineRenderer.SetPosition(0, barrel.transform.position);
        lineRenderer.SetPosition(1, aimSpot);


        //simply play a shooting sound once
        audioSource.PlayOneShot(shootSound);
    }
}
