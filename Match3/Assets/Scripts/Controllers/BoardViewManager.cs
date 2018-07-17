using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using UnityEngine;

public class BoardViewManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;
    private Material firstMaterial;
    private Material secondMaterial;
    private Material therdMaterial;
    private Material fourthMaterial;

    private bool isMoving;
    private List<Transform> _movingTransforms;
    private List<Vector3> _localTargetPositionsList;
    private int _speed;

    private ObjectPooling TilePool;

    private void Start()
    {
        BuildMaterials();
    }

    private void OnDestroy()
    {
        if (TilePool)
            Destroy(TilePool);
        StopAllCoroutines();
    }

    private void Update()
    {
        if (isMoving)
            Move();
    }

    public void Initialize(int count, Transform parent)
    {
        if (TilePool)
            Destroy(TilePool);
        _prefab.transform.localScale *= parent.localScale.y;
        TilePool = BuildObjectPool(count, _prefab, parent);
    }

    public PoolObject CreateTile(int variable, Vector2 coordinates)
    {
        var po = TilePool.GetPoolObject();
        var go = po.gameObject;
        go.SetActive(true);
        go.transform.localPosition = coordinates;
        go.GetComponent<Renderer>().material = GetColor(variable);

        return po;
    }

    public void RemoveTile(Transform tile)
    {
        var po = tile.GetComponent<PoolObject>();
        if (!po)
            Destroy(tile.gameObject);

        po.ReturnToPool();
    }

    public void UpdatePosition(IEnumerable<TileViewModel> list, int speed)
    {
        var tileViewModels = list as TileViewModel[] ?? list.ToArray();

        _movingTransforms = tileViewModels.Select(v => v.Transform).ToList();
        _localTargetPositionsList = tileViewModels.Select(v => new Vector3(v.Model.X, v.Model.Y)).ToList();

        _speed = speed;
        isMoving = true;
    }

    private void Move()
    {
        for (int i = 0; i < _movingTransforms.Count(); i++)
        {
            if (_movingTransforms[i].localPosition.x == _localTargetPositionsList[i].x &&
                _movingTransforms[i].localPosition.y == _localTargetPositionsList[i].y)
            {
                _movingTransforms.Remove(_movingTransforms[i]);
                _localTargetPositionsList.Remove(_localTargetPositionsList[i]);
                i--;
                if (TryFinish())
                {
                    Finish();
                    return;
                }
            }
            else
            {
                _movingTransforms[i].localPosition = Vector3.MoveTowards(_movingTransforms[i].localPosition, _localTargetPositionsList[i], _speed * Time.deltaTime);
            }
        }
    }

    private bool TryFinish()
    {
        if (_movingTransforms.Count == 0)
            return true;

        return false;
    }

    private void Finish()
    {
        EventsManager.TilesFinishMoving();
        isMoving = false;
    }

    private ObjectPooling BuildObjectPool(int count, GameObject prefab, Transform parent)
    {
        var pool = ScriptableObject.CreateInstance<ObjectPooling>();
        pool.Initialize(count, prefab, parent);

        return pool;
    }

    private void BuildMaterials()
    {
        var mat = _prefab.GetComponent<Renderer>().material;

        firstMaterial = new Material(mat) { color = Color.red };
        secondMaterial = new Material(mat) { color = Color.blue };
        therdMaterial = new Material(mat) { color = Color.green };
        fourthMaterial = new Material(mat) { color = Color.yellow };
    }

    private Material GetColor(int i)
    {
        switch (i)
        {
            case 1:
                return firstMaterial;
            case 2:
                return secondMaterial;
            case 3:
                return therdMaterial;
            case 4:
                return fourthMaterial;
            default:
                return new Material(_prefab.GetComponent<Renderer>().material) { color = Color.black };
        }
    }
}
