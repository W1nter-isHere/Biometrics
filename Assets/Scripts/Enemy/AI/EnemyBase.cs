using Pathfinding;
using Player;
using Unity.Mathematics;
using UnityEngine;

namespace Enemy.AI
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Seeker))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] protected EnemyStats stats;
        [SerializeField] protected Transform target;
        [SerializeField] protected Transform groundChecker;
        [SerializeField] protected Transform attackPoint;

        protected Seeker Seeker;
        protected Path Path;
        protected int CurrentWaypoint;
        protected Rigidbody2D Rigidbody2D;
        [SerializeField]
        protected float MovementSpeed;
        protected bool OnGround;
        protected bool HasLineOfSight;
        protected Vector2 DirectionOfTarget;

        private readonly Collider2D[] _groundColliders = new Collider2D[2];
        private bool _facingRight = true;
        
        protected virtual void Start()
        {
            Seeker = GetComponent<Seeker>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            InvokeRepeating(nameof(UpdatePath), 0, 0.5f);
            SetupTarget();
            SetupStats();
        }

        protected virtual void Update()
        {
            if (PlayerController.Instance.Object.GameState == GameState.Paused) return;
            Physics2D.IgnoreLayerCollision(6, 7);
            Physics2D.IgnoreLayerCollision(6, 6);
            if (target == null) return;
            var position = attackPoint.position;
            DirectionOfTarget = ((Vector2)(target.position - position)).normalized;
            
            HasLineOfSight = false;
            foreach (var hit in Physics2D.RaycastAll(position, DirectionOfTarget, stats.seekRadius))
            {
                if (hit.collider == null) return;
                if (hit.collider.CompareTag("Environment")) break;
                if (hit.collider.CompareTag("Enemy")) continue;
                if (hit.collider.CompareTag("Projectile")) continue;
                HasLineOfSight = hit.collider.gameObject == target.gameObject;   
            }
        }

        protected virtual void PathEvaluated(Path p)
        {
            if (p.error) return;
            Path = p;
            CurrentWaypoint = 0;
        }
        
        protected virtual void UpdatePath()
        {
            if (!Seeker.IsDone()) return;
            if (Vector3.Distance(target.position, transform.position) <= stats.seekRadius)
            {
                Seeker.StartPath(Rigidbody2D.position, target.position, PathEvaluated);
            }
        }
        
        protected virtual void SetupTarget()
        {
            target = PlayerController.Instance.transform;
        }
        
        protected virtual void SetupStats()
        {
            MovementSpeed = stats.movementSpeed;
            if (stats.enemySprite == null) return;
            GetComponent<SpriteRenderer>().sprite = stats.enemySprite;
        }

        protected bool MoveTowardsTarget(bool verticalAllowed = false, bool jumpEnabled = true)
        {
            if (PlayerController.Instance.Object.GameState == GameState.Paused) return false;
            if (Path == null) return false;
            if (CurrentWaypoint >= Path.vectorPath.Count) return true;
            
            OnGround = false;
            for (var i = 0; i < Physics2D.OverlapBoxNonAlloc(groundChecker.position, new Vector2(0.15f, 0.1f), 0, _groundColliders); i++)
            {
                var c = _groundColliders[i];
                if (c == null) continue;
                if (c.gameObject == gameObject) continue;
                OnGround = true;
            }
            
            var direction = ((Vector2)Path.vectorPath[CurrentWaypoint] - Rigidbody2D.position).normalized;
            var next = false;
            if (direction == Vector2.zero && CurrentWaypoint < Path.vectorPath.Count)
            {
                direction = ((Vector2)Path.vectorPath[CurrentWaypoint + 1] - Rigidbody2D.position).normalized;
                next = true;
            }
            
            var force = direction.normalized * MovementSpeed * Time.fixedDeltaTime;
            if (!verticalAllowed) force.y = 0;
            
            // var currentPos = Rigidbody2D.position;
            // Rigidbody2D.MovePosition(new Vector2(math.lerp(currentPos.x, currentPos.x + force.x, 0.5f), math.lerp(currentPos.y, currentPos.y + force.y, 0.5f)));
            
            Rigidbody2D.AddForce(force*200);
            
            if (jumpEnabled && OnGround)
            {
                if (direction.y > 0)
                {
                    Rigidbody2D.AddForce(Vector2.up * MovementSpeed * 0.4f, ForceMode2D.Impulse);
                }
            }
            
            var distanceToNextWaypoint = Vector2.Distance(Rigidbody2D.position, Path.vectorPath[next ? CurrentWaypoint + 1 : CurrentWaypoint]);
            if (distanceToNextWaypoint < stats.waypointTolerance) CurrentWaypoint++;
            
            // flipping the player appropriately
            if (Rigidbody2D.velocity.x > 0.05f && !_facingRight)
            {
                Flip();
            }
            else if (Rigidbody2D.velocity.x < -0.05f && _facingRight)
            {
                Flip();
            }
            
            return false;
        }

        private void Flip()
        {
            if (PlayerController.Instance.Object.GameState == GameState.Paused) return;
            _facingRight = !_facingRight;
            transform.Rotate(0, 180, 0);
        }

        protected void LookTowardsTarget()
        {
            if (PlayerController.Instance.Object.GameState == GameState.Paused) return;
            if (DirectionOfTarget.x < 0 && _facingRight)
            {
                Flip();
            }

            if (DirectionOfTarget.x > 0 && !_facingRight)
            {
                Flip();
            }
        }
    }
}