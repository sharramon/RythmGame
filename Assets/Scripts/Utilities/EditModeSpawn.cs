using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
/// <summary>
///
/// </summary>
[ExecuteInEditMode]
public class EditModeSpawn : MonoBehaviour
{
    [Header("Main controls")]
    [SerializeField] bool placeObjects = false;
    [SerializeField] bool setFree = false;
    [SerializeField] bool getListFromParent = false;
    [SerializeField] bool delete = false;
    bool prevUpdateState = true;

    [Header("Placement Info")]
    [SerializeField] string groupName;
    [SerializeField] LayerMask mask;
    [SerializeField] float yOffset;
    [SerializeField] bool scaleObject = false;
    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] bool rotateX;
    [SerializeField] bool rotateY;
    [SerializeField] bool rotateZ;


    [Header("Placed objects")]
    [SerializeField] GameObject _parentObject;
    [SerializeField] GameObject placementObject;
    [SerializeField] List<GameObject> _placedObjectList = new List<GameObject>();


    private void OnEnable()
    {
        if (!Application.isEditor)
        {
            Destroy(this);
        }
        placeObjects = false;
        prevUpdateState = true;
    }

    private void Update()
    {
        if (placeObjects)
        {
            if (prevUpdateState)
            {
                prevUpdateState = false;
                SceneView.duringSceneGui += OnScene;
            }
        }
        if (!placeObjects)
        {
            if (!prevUpdateState)
            {
                prevUpdateState = true;
                SceneView.duringSceneGui -= OnScene;
            }
        }
        if (setFree)
        {
            setFree = false;
            SetFree();
        }
        if (getListFromParent)
        {
            getListFromParent = false;
            GetListFromParent();
        }
        if (delete)
        {
            delete = false;
            DeleteAll();
        }
    }
    void OnScene(SceneView scene)
    {
        Event e = Event.current;

        //if (e.type == EventType.MouseDown && e.button == 0)
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.A)
        {
            Debug.Log("left Mouse was pressed");
            if (_parentObject == null)
            {
                GameObject parentObject = new GameObject();
                parentObject.name = groupName;
                _parentObject = parentObject;
                EmptyList(_placedObjectList);
            }

            //Vector3 mousePos = e.mousePosition;
            //float ppp = EditorGUIUtility.pixelsPerPoint;
            //mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
            //mousePos.x *= ppp;

            //Ray ray = scene.camera.ScreenPointToRay(mousePos);
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                _placedObjectList = FixMissing(_placedObjectList);

                if (placementObject == null)
                {
                    Debug.LogError("Placement object is null");
                    return;
                }

                GameObject go = PlaceObject(hit);
                _placedObjectList.Add(go);
            }
            e.Use();
        }
    }
    private GameObject PlaceObject(RaycastHit hit)
    {
        GameObject go = Instantiate(placementObject);
        go.transform.position = hit.point + new Vector3(0, yOffset, 0);
        go.transform.SetParent(_parentObject.transform);
        if (scaleObject)
        {
            float scaleMultiplier = Random.Range(minScale, maxScale);
            go.transform.localScale = go.transform.localScale * scaleMultiplier;
        }
        if (rotateX)
        {
            float rotateChange = Random.Range(0, 360);
            Quaternion tempRotation = go.transform.localRotation;
            tempRotation *= Quaternion.Euler(rotateChange, 0, 0);
            go.transform.localRotation = tempRotation;
        }
        if (rotateY)
        {
            float rotateChange = Random.Range(0, 360);
            Quaternion tempRotation = go.transform.localRotation;
            tempRotation *= Quaternion.Euler(0, rotateChange, 0);
            go.transform.localRotation = tempRotation;
        }
        if (rotateZ)
        {
            float rotateChange = Random.Range(0, 360);
            Quaternion tempRotation = go.transform.localRotation;
            tempRotation *= Quaternion.Euler(0, 0, rotateChange);
            go.transform.localRotation = tempRotation;
        }
        Debug.Log("Instantiated at " + hit.point);
        go.name = $"{groupName}{_placedObjectList.Count}";

        return go;
    }
    public void EmptyList(List<GameObject> placedObjectList)
    {
        for (int i = 0; i < placedObjectList.Count; i++)
        {
            if (placedObjectList[i] != null)
            {
                DestroyImmediate(placedObjectList[i]);
            }
            placedObjectList.Clear();
        }
    }
    private List<GameObject> FixMissing(List<GameObject> placedObjectList)
    {
        Stack<int> objectsToDelete = new Stack<int>();

        //find all null and put them on a stack
        for (int i = 0; i < placedObjectList.Count; i++)
        {
            if (placedObjectList[i] == null)
            {
                Debug.Log($"Object at {i} is null");
                objectsToDelete.Push(i);
            }
        }

        //get all the objects on stack and delete them from the list
        while (objectsToDelete.Count != 0)
        {
            int index = objectsToDelete.Pop();
            placedObjectList.RemoveAt(index);
        }

        //rename all the objects according to new stack
        for (int i = 0; i < placedObjectList.Count; i++)
        {
            GameObject currentObject = placedObjectList[i];
            currentObject.name = $"{groupName}_{i}";
        }

        return placedObjectList;
    }

    private void SetFree()
    {
        _placedObjectList = FixMissing(_placedObjectList);
        _parentObject = null;
        _placedObjectList.Clear();
    }

    private void GetListFromParent()
    {
        if (_parentObject == null)
            return;

        foreach (Transform child in _parentObject.transform)
        {
            _placedObjectList.Add(child.gameObject);
        }
    }
    private void DeleteAll()
    {
        if (_parentObject != null)
        {
            DestroyImmediate(_parentObject);
            _parentObject = null;
        }
        for (int i = 0; i < _placedObjectList.Count; i++)
        {
            if (_placedObjectList[i] != null)
                DestroyImmediate(_placedObjectList[i]);
        }
        _placedObjectList.Clear();
    }
    public List<GameObject> GetObjectList()
    {
        _placedObjectList = FixMissing(_placedObjectList);
        return _placedObjectList;
    }

}
#endif