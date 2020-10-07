using UnityEngine;

namespace PlatformerPathFinding.Examples {
    /// <summary>
    /// This is just for demonstration purpose, I strongly suggest you not use this.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour {
        [SerializeField] float _moveSpeed;
        [SerializeField] float _jumpStrength;
        [SerializeField] Rigidbody2D _rb;
        [SerializeField] LayerMask _mask;
        [SerializeField] Transform _groundCheck;
        [SerializeField] float _circleRadius;
        
        Vector3 _velocity = Vector3.zero;

        void Update() {
            var h = Input.GetAxisRaw("Horizontal");
            var jump = Input.GetButtonDown("Jump");

            float dt = Time.deltaTime;
            _rb.velocity = new Vector2(h * dt * _moveSpeed, _rb.velocity.y);
            
            bool isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _circleRadius, _mask);
            if (isGrounded && jump)
                _rb.AddForce(Vector2.up * _jumpStrength);
        }
    }
}