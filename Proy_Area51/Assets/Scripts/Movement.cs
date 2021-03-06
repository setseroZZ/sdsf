﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D charRigidbody2D;
    public float characterSpeed;
    public float jumpSpeed = 10;
    public float health = 10;
    public float maxHealth = 10;

    public GameObject personalSealImage;

    public static bool isPuzzleNotActive;

    private bool isSealed;

    public ApareceDelSuelo poderDelSuelo;
    public PoderDePuas poderDePuas;

    public Animator animator2d;


    public float rayDetectionDistance = 0.1f;
    public float normalizedHP { get { return health / maxHealth; } }
    Vector3 leftNode { get { return transform.position - new Vector3(0.5f, 1, 0); } }
    Vector3 rightNode { get { return transform.position + new Vector3(0.5f, -1, 0); } }
    bool isGrounded;
    public SpriteRenderer spriteRenderer;


    public bool isSpriteFacingLeft = false;

    public GameOverScript gameOverObject;


    public bool isSummoning=false;
    // Use this for initialization
    void Start()
    {
    	isPuzzleNotActive = false;
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isSealed && !isPuzzleNotActive&&!isSummoning)
            {
                float horizontalSpeed = Input.GetAxis("Horizontal");
                if (horizontalSpeed != 0)
                {
                    animator2d.SetInteger("Speed2", 1);

                }
                else
                {
                    animator2d.SetInteger("Speed2", 0);
                }
                if (horizontalSpeed < 0)
                {
                    if (spriteRenderer.flipX == isSpriteFacingLeft) { spriteRenderer.flipX = !isSpriteFacingLeft; }
                }
                else if (horizontalSpeed > 0)
                {
                    if (spriteRenderer.flipX == !isSpriteFacingLeft) { spriteRenderer.flipX = isSpriteFacingLeft; }
                }
                charRigidbody2D.velocity = new Vector2(horizontalSpeed * characterSpeed, charRigidbody2D.velocity.y);
            }
            else
            {
                
                charRigidbody2D.velocity = new Vector2(0f, charRigidbody2D.velocity.y);
                if (!isSummoning)
                animator2d.SetInteger("Speed2", 0);
            }
        
       
    }

    private void FixedUpdate()
    {
        
        if (!isSealed && !isPuzzleNotActive&&!isSummoning)
            {
                RaycastHit2D downLeft = Physics2D.Raycast(leftNode, Vector3.down, rayDetectionDistance);
                RaycastHit2D downRight = Physics2D.Raycast(rightNode, Vector3.down, rayDetectionDistance);

                if (isGrounded)
                {
                    if (!downLeft && !downRight)
                    {
                        isGrounded = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        charRigidbody2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                        isGrounded = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (downLeft.collider && downLeft.collider.CompareTag("Ground"))
                        {
                            SummonEarth(0);
                        }

                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (downLeft.collider && downLeft.collider.CompareTag("Ground"))
                        {
                            StartSummoning();


                        }
                    }
                }
                else
                {
                    if ((downLeft || downRight) && charRigidbody2D.velocity.y == 0)
                    {
                        isGrounded = true;
                    }
                }
            }
        
    }

    public void TakeDamage(int damage)
    {
        

        //health -= damage;

        //health=Mathf.Clamp(health, 0f, maxHealth);

        //if (health == 0 && !isPuzzleNotActive)
        //{
        //    personalSealImage.SetActive(true);
        //    isPuzzleNotActive = true;
        //}

        if (!isPuzzleNotActive)
        {
            health -= damage;

            health = Mathf.Clamp(health, 0f, maxHealth);
            if (health == 0)
            {
                personalSealImage.SetActive(true);
                isPuzzleNotActive = true;
                gameOverObject.playGameOver();

            }
        }

        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(leftNode, 0.2f);
        Gizmos.DrawSphere(rightNode, 0.2f);
        Gizmos.color = Color.white;
        Gizmos.DrawRay(leftNode, Vector3.down * rayDetectionDistance);
    }

    public void SummonEarth(float distance){
        Vector3 startPoint = new Vector3(transform.position.x+distance, transform.position.y, transform.position.z);
        poderDelSuelo.SummonThis(startPoint);
    }

    public void SummonSpike(float distance) {
        Vector3 startPoint = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
        poderDePuas.SummonThis(startPoint);
    }


    public void releaseFromCurse()
    {
        isSealed = false;
    }

    public void sealThis()
    {
        isSealed = true;
    }
    /*public static void stopMovement(){
    	isPuzzleNotActive=true;
    	charRigidbody2D.velocity =Vector2.zero;
    }*/
    public void renderFrontOfEverything()
    {
        spriteRenderer.material.renderQueue = 4001;
    }

    private void StartSummoning() {
        isSummoning = true;
        animator2d.SetInteger("Speed2", 2);
    }

    public void summonSpikeNow() {
        
        SummonSpike((spriteRenderer.flipX) ? -1 : 1);
        isSummoning = false;
    }

}
