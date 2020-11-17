using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Threading.Tasks;

public class LoadedAddressableLocations : MonoBehaviour {

    [SerializeField] private string _label;

    public IList<IResourceLocation> AssetLocations { get; } = new List<IResourceLocation>();

    private void Start() {
        InitAndWaitUntilLocLoaded();
    }

    private async Task InitAndWaitUntilLocLoaded() {

        await AddressableLocationLoader.GetAllByLabel(_label, AssetLocations);

        foreach(var location in AssetLocations) {
            Debug.Log(location.PrimaryKey);
        }
    }

}
