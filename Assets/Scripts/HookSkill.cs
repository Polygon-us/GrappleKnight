using UnityEngine;
using UnityEngine.InputSystem;

public class HookSkill : ISkill
{
    private Camera _mainCamera;
    private Mouse _mousePosition;

    private Transform _transform;
    private Transform _hookBegin;
    private Transform _hookEnd;

    private GameObject _rope;
    private MeshFilter _ropeMeshFilter;
    
    private SpringJoint2D _springJoint2D;

    private float _hookMaxDistance;
    

    private Mesh _ropeMesh;
    private Vector3[] _ropeMeshVertices = new Vector3[4];
    private Vector2[] _ropeMeshUV = new Vector2[4];
    private int[] _ropeMeshTriangles = new int[6];
    
    
    private bool _onHook;
    public HookSkill(Transform transform,Transform hookEnd,Transform hookBegin, SpringJoint2D springJoint2D
        ,float hookMaxDistance, GameObject rope)
    {
        _transform = transform;
        _hookBegin = hookBegin;
        _hookEnd = hookEnd;
        _hookMaxDistance = hookMaxDistance;
        _mainCamera = Camera.main;
        _mousePosition = Mouse.current;
        _ropeMesh = new Mesh();
        _rope = rope;
        _ropeMeshFilter = _rope.GetComponent<MeshFilter>();
        _springJoint2D = springJoint2D;
        _springJoint2D.anchor = hookBegin.localPosition;
        InitMesh();
    }
    
    public void InitSkill()
    {
        Vector3 newPosition = _mousePosition.position.ReadValue();
        newPosition = _mainCamera.ScreenToWorldPoint(newPosition);
        newPosition.z = 0;
        
        Vector3 createPosition = newPosition - _hookBegin.position;
        
        RaycastHit2D hit = Physics2D.Raycast(_hookBegin.position, createPosition, _hookMaxDistance);
        Physics2D.Raycast( _hookBegin.position, createPosition, _hookMaxDistance);
        
        if (hit.collider != null)
        {
            _hookEnd.parent = null;
            _hookEnd.gameObject.SetActive(true);
            _rope.SetActive(true);
            _springJoint2D.enabled = true;
            _hookEnd.position = hit.point;
            _springJoint2D.distance = Vector3.Distance(_hookBegin.position,_hookEnd.position);
            _onHook = true;
        }
    }

    private void InitMesh()
    {
        _ropeMeshUV[0] = new Vector2(0, 0);
        _ropeMeshUV[1] = new Vector2(0, 1);
        _ropeMeshUV[2] = new Vector2(1, 1);
        _ropeMeshUV[3] = new Vector2(1, 0);

        _ropeMeshTriangles[0] = 0;
        _ropeMeshTriangles[1] = 1;
        _ropeMeshTriangles[2] = 2;
            
        _ropeMeshTriangles[3] = 0;
        _ropeMeshTriangles[4] = 2;
        _ropeMeshTriangles[5] = 3;
    }
    public bool DoSkill()
    {
        if (_onHook)
        {
            _ropeMeshVertices[0] = _hookBegin.localPosition;
            _ropeMeshVertices[1] = _transform.InverseTransformPoint(_hookEnd.position);
            _ropeMeshVertices[2] = _transform.InverseTransformPoint(_hookEnd.position) + new Vector3(0.1f, 0f, 0f);
            _ropeMeshVertices[3] = _hookBegin.localPosition + new Vector3(0.1f, 0f, 0f);

            _ropeMesh.vertices = _ropeMeshVertices;
            _ropeMesh.uv = _ropeMeshUV;
            _ropeMesh.triangles = _ropeMeshTriangles;

            _ropeMeshFilter.mesh = _ropeMesh;
            return true;
        }
        return false;

    }

    public void UndoSkill()
    {
        _hookEnd.parent = _transform;
        _hookEnd.gameObject.SetActive(false);
        _rope.SetActive(false);
        _springJoint2D.enabled = false;
        _onHook = false;
    }

    public PlayerMovementTypeEnum SendActionMapTypeEnum()
    {
        return PlayerMovementTypeEnum.HookMovement;
    }

    public void UnsubscribeActions()
    {
        
    }
    
}