using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum movementType { Undefined,Keyboard, Controller }

    [Header("Movement")]
    [SerializeField]private movementType _movementType = movementType.Undefined;
    public MovementManager movementManager;
    public Vector3 RawMovement { get; private set; }
    public bool inverted = false;
    public float _currentHorizontalSpeed, _currentVerticalSpeed; 


    [Header("Player Activities")]
    private bool _active;

    private void Awake() => Invoke(nameof(Activate), 0.5f);
    void Activate() => _active = true;

    // Update is called once per frame
    void Update()
    {
        if (!_active) { return; }

        if (_movementType == movementType.Undefined) {
            GetMovementType(); 
            return; 
        }

        RunCollisionChecks(); // Collision Detection
        CalculateWalk(); // Horizontal & Vertical movement
        MoveCharacter(); // Actually preforms the movement
    }

    public void GetMovementType()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _movementType = movementType.Keyboard;
            movementManager = new KeyboardControls();
        }
        else if (Input.GetButton("Submit"))
        {
            _movementType = movementType.Controller;
            movementManager = new ControllerControls();
        }
    }

    #region Collisions

    [Header("COLLISION")] [SerializeField] private Bounds _characterBounds;
    [SerializeField] private int _detectorCount = 3;
    [SerializeField] private LayerMask _collisionObjects;
    [SerializeField] private float _detectionRayLength = 0.1f;
    [SerializeField] [Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

    private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
    private bool _colUp, _colRight, _colDown, _colLeft;

    private void RunCollisionChecks()
    {
        //generates ray range
        CalculateRayRanged();

        _colUp      = RunDetection(_raysUp);
        _colDown    = RunDetection(_raysDown);
        _colLeft    = RunDetection(_raysLeft);
        _colRight   = RunDetection(_raysRight);

        bool RunDetection(RayRange range)
        {
            return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _collisionObjects));
        }

    }

    private void CalculateRayRanged()
    {
        var b = new Bounds(transform.position + _characterBounds.center, _characterBounds.size);

        _raysDown = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
        _raysUp = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
        _raysLeft = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
        _raysRight = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);
    }

    private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
    {
        for (var i = 0; i < _detectorCount; i++)
        {
            var t = (float)i / (_detectorCount - 1);
            yield return Vector2.Lerp(range.Start, range.End, t);
        }
    }

    private void OnDrawGizmos()
    {
        // Bounds
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

        // Rays
        if (!Application.isPlaying)
        {
            CalculateRayRanged();
            Gizmos.color = Color.blue;
            foreach (var range in new List<RayRange> { _raysUp, _raysRight, _raysDown, _raysLeft })
            {
                foreach (var point in EvaluateRayPositions(range))
                {
                    Gizmos.DrawRay(point, range.Dir * _detectionRayLength);
                }
            }
        }

        if (!Application.isPlaying) return;

        // Draw the future position. Handy for visualizing gravity
        Gizmos.color = Color.red;
        var move = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.deltaTime;
        Gizmos.DrawWireCube(transform.position + _characterBounds.center + move, _characterBounds.size);
    }

    #endregion

    #region Walk

    [Header("WALKING")] [SerializeField] private float _acceleration = 90;
    [SerializeField] private float _moveClamp = 13;
    [SerializeField] private float _deAcceleration = 60f;

    private void CalculateWalk()
    {
        CalculateWalkHorizontal();
        CalculateWalkVertical();
    }

    void CalculateWalkHorizontal()
    {
        //Debug.Log(movementManager.Direction().x);
        if (movementManager.Direction().x != 0)
        {
            if (inverted) { _currentHorizontalSpeed += movementManager.Direction().x * _acceleration * Time.deltaTime * -1; }
            else { _currentHorizontalSpeed += movementManager.Direction().x * _acceleration * Time.deltaTime; }
            // Set horizontal move speed
            
            // clamped by max frame movement
            _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);
            
        }
        else
        {
            // No input. Let's slow the character down
            _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
        }

        if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft)
        {
            // Don't walk through walls
            _currentHorizontalSpeed = 0;
        }
    }
    void CalculateWalkVertical()
    {
        if (movementManager.Direction().y != 0)
        {
            // Set horizontal move speed
            if (inverted) { _currentVerticalSpeed += movementManager.Direction().y * _acceleration * Time.deltaTime * -1; }
            else { _currentVerticalSpeed += movementManager.Direction().y * _acceleration * Time.deltaTime; }

            // clamped by max frame movement
            _currentVerticalSpeed = Mathf.Clamp(_currentVerticalSpeed, -_moveClamp, _moveClamp);

        }
        else
        {
            // No input. Let's slow the character down
            _currentVerticalSpeed = Mathf.MoveTowards(_currentVerticalSpeed, 0, _deAcceleration * Time.deltaTime);
        }

        if (_currentVerticalSpeed > 0 && _colRight || _currentVerticalSpeed < 0 && _colLeft)
        {
            // Don't walk through walls
            _currentVerticalSpeed = 0;
        }
    }

    #endregion

    #region Move

    [Header("MOVE")]
    [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
    private int _freeColliderIterations = 10;

    // We cast our bounds before moving to avoid future collisions
    private void MoveCharacter()
    {
        var pos = transform.position + _characterBounds.center;
        RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
        var move = RawMovement * Time.deltaTime;
        var furthestPoint = pos + move;

        // check furthest movement. If nothing hit, move and don't do extra checks
        var hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _collisionObjects);
        if (!hit)
        {
            transform.position += move;
            return;
        }

        // otherwise increment away from current pos; see what closest position we can move to
        var positionToMoveTo = transform.position;
        for (int i = 1; i < _freeColliderIterations; i++)
        {
            // increment to check all but furthestPoint - we did that already
            var t = (float)i / _freeColliderIterations;
            var posToTry = Vector2.Lerp(pos, furthestPoint, t);

            //disabled nudging
            /*if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0))
            {
                transform.position = positionToMoveTo;

                // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                if (i == 1)
                {
                    //if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
                    var dir = transform.position - hit.transform.position;
                    transform.position += dir.normalized * move.magnitude;
                }

                return;
            }
            */
            positionToMoveTo = posToTry;
        }
    }

    #endregion
}