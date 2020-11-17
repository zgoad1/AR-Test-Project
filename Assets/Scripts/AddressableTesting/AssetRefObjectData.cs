using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetRefObjectData : MonoBehaviour {

    [SerializeField] private AssetReference _someRef;
    [SerializeField] private List<AssetReference> _listOfRefs = new List<AssetReference>();

    [SerializeField] private List<GameObject> _completedObjs = new List<GameObject>();

    private void Start() {
        _listOfRefs.Add(_someRef);
        StartCoroutine(LoadAndWaitUntilComplete());
    }

    private IEnumerator LoadAndWaitUntilComplete() {
        yield return AssetRefLoader.CreateAssetsAddToList(_listOfRefs, _completedObjs);
    }

}
