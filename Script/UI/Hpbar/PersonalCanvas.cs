using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonalCanvas : MonoBehaviour
{
    public List<GameObject> _list_dmgText = new List<GameObject>();

    [Header("Prefab")]
    [SerializeField] int _prefabLength = 3;
    [SerializeField] GameObject _prefab_dmgText;

    private void Start()
    {
        if (_prefab_dmgText == null)
            _prefab_dmgText = Resources.Load("dmgText") as GameObject;

        for (int i = 0; i < _prefabLength; i++)
            AddObj(_prefab_dmgText, transform, _list_dmgText);
    }
    GameObject AddObj(GameObject prefab, Transform parents, List<GameObject> list)
    {
        GameObject obj = Instantiate(prefab, parents);
        list.Add(obj);
        return obj;
    }

    public GameObject Getobj(List<GameObject> list)
    {
        if (list == _list_dmgText)
        {
            GameObject obj = AddObj(_prefab_dmgText, transform, _list_dmgText);
            return obj;
        }

        return null;
    }
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
