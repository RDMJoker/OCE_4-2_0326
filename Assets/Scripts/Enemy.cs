using System;
using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 1.5f;
        [SerializeField] LayerMask playerLayerMask;
        [SerializeField] LayerMask obstacleLayerMask;
        [SerializeField] float aggroRadius = 2f;
        [SerializeField] Color aggroColor;
        [SerializeField] Color defaultColor;
        [SerializeField] float damage;

        Rigidbody2D rb;
        SpriteRenderer spriteRenderer;

        Coroutine colorChangeRoutine;


        Vector2 target;
        bool isAggro;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            ChangeColor(defaultColor);
            StartCoroutine(ColorChange());
        }

        // void FixedUpdate()
        // {
        //     var directionalVector = player.transform.position - transform.position;
        //     rb.linearVelocity = new Vector2(directionalVector.normalized.x * moveSpeed, 0);
        // }

        void FixedUpdate()
        {
            CheckForPlayer();
            if (!isAggro) return;
            Vector2 directionalVector = target - (Vector2)transform.position;
            rb.linearVelocity = new Vector2(directionalVector.normalized.x * moveSpeed, 0);
        }

        void CheckForPlayer()
        {
            Collider2D hitResult = Physics2D.OverlapCircle(transform.position, aggroRadius, playerLayerMask);
            if (hitResult)
            {
                target = hitResult.transform.position;
                Vector2 directionalVector = target - (Vector2)transform.position;
                RaycastHit2D obstacleHitResult = Physics2D.Raycast(transform.position, directionalVector,
                    Vector2.Distance(transform.position, target), obstacleLayerMask);
                isAggro = !obstacleHitResult;
                // if (colorChangeRoutine != null) return;
                // colorChangeRoutine = StartCoroutine(ColorChange());
            }
            else isAggro = false;
            //if(colorChangeRoutine == null) return;
            //StopCoroutine(colorChangeRoutine);
            // colorChangeRoutine = null;

            //Color color = isAggro ? aggroColor : defaultColor; //WTF-Bedingung = What ? True : False
            //ChangeColor(color);
        }

        void ChangeColor(Color _color)
        {
            // spriteRenderer.color = _color;
            var color = Color.Lerp(defaultColor, aggroColor, Time.deltaTime * 0.8f);
            spriteRenderer.color = color;
        }

        IEnumerator ColorChange()
        {
            float duration = 2f;
            float t = 0;

            while (t < duration)
            {
                Color currentColor = Color.Lerp(defaultColor, aggroColor, t / duration);
                spriteRenderer.color = currentColor;
                t += Time.deltaTime;
                // Debug.Log("T:" + t);
                //Debug.Log("T / Duration: "+ t / duration);
                yield return null;
            }

            yield return new WaitForSeconds(2f);

            Debug.Log("ColorChange done! :) ");
            colorChangeRoutine = null;
        }


        void OnCollisionEnter2D(Collision2D _other)
        {
            if (_other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}