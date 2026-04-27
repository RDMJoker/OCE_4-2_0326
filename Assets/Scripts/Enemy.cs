using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] LayerMask obstacleLayerMask;
    [SerializeField] float aggroRadius = 2f;
    [SerializeField] Color aggroColor;
    [SerializeField] Color defaultColor;

    //[SerializeField] GameObject player;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    Vector2 target;
    bool isAggro;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeColor(defaultColor);
    }

    // void FixedUpdate()
    // {
    //     var directionalVector = player.transform.position - transform.position;
    //     rb.linearVelocity = new Vector2(directionalVector.normalized.x * moveSpeed, 0);
    // }

    void FixedUpdate()
    {
        CheckForPlayer();
        if (isAggro)
        {
            Vector2 directionalVector = target - (Vector2)transform.position;
            rb.linearVelocity = new Vector2(directionalVector.normalized.x * moveSpeed, 0);
        }
    }

    void CheckForPlayer()
    {
        Collider2D hitResult = Physics2D.OverlapCircle(transform.position, aggroRadius, playerLayerMask);
        if (hitResult != null)
        {
            target = hitResult.transform.position;
            Vector2 directionalVector = target - (Vector2)transform.position;
            RaycastHit2D obstacleHitResult = Physics2D.Raycast(transform.position, directionalVector, Vector2.Distance(transform.position, target), obstacleLayerMask);
            isAggro = !obstacleHitResult;
            
        }
        else isAggro = false;
        
        Color color = isAggro ? aggroColor : defaultColor; //WTF-Bedingung = What ? True : False
        ChangeColor(color);
    }

    void ChangeColor(Color _color)
    {
        spriteRenderer.color = _color;
    }
}