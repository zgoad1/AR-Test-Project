using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PointCloudController : MonoBehaviour {

    public Material pointCloudMesh;

    void Start() {
        ARPointCloudManager m = GetComponent<ARPointCloudManager>();
        m.pointCloudsChanged += OnPointCloudsChanged;
    }

    void OnPointCloudsChanged(ARPointCloudChangedEventArgs args) {
        foreach(ARPointCloud c in args.added) {
            c.gameObject.AddComponent<MeshFilter>();
            MeshRenderer rend = c.gameObject.AddComponent<MeshRenderer>();
            rend.material = pointCloudMesh;
            c.gameObject.AddComponent<ARPointCloudMeshVisualizer>();
        }
    }
}
