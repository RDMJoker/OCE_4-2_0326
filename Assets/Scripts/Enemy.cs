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
        [SerializeField] float colorChangeDuration = 2f;

        Rigidbody2D rb;
        SpriteRenderer spriteRenderer;

        Coroutine colorChangeRoutine;
        Color colorToChangeTo;


        Vector2 target;

        bool isAggro;
        
        public bool IsAggro
        {
            get => isAggro;
            private set
            {
                if(IsAggro == value) return;
                isAggro = value;
                StopAllCoroutines(); //This would need to change, if we have multiple routines
                colorToChangeTo = IsAggro ? aggroColor : defaultColor;
                StartCoroutine(ColorChange(colorToChangeTo));
            }
        }

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
            if (!IsAggro) return;
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
                RaycastHit2D obstacleHitResult = Physics2D.Raycast(transform.position, directionalVector, Vector2.Distance(transform.position, target), obstacleLayerMask);
                IsAggro = !obstacleHitResult;
            }
            else
            {
                IsAggro = false;
            }
            
        }

        void ChangeColor(Color _color)
        {
            // spriteRenderer.color = _color;
            var color = Color.Lerp(defaultColor, aggroColor, Time.deltaTime * 0.8f);
            spriteRenderer.color = color;
        }
        
        
        IEnumerator ColorChange(Color _colorToChangeTo)
        {
            Color startColor = spriteRenderer.color;
            
            float t = 0;

            while (t < colorChangeDuration)
            {
                Color currentColor = Color.Lerp(startColor, _colorToChangeTo, t / colorChangeDuration);
                spriteRenderer.color = currentColor;
                t += Time.deltaTime;
                yield return null;
            }
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