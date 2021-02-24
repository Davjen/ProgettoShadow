using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDebugger : MonoBehaviour
{
    //Vector3[] points;
    public Transform light;
    public Transform ponte;
    private List<Vector3> points;
    [Range(0, 300)]
    public float lenght;
    // Start is called before the first frame update
    void Start()
    {
        points = new List<Vector3>();
    }
    private void OnValidate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        //Vector3[] points = mesh.vertices;
        //Vector3 dir1 = light.position - ponte.position;
        //if (Physics.Raycast(new Vector3)
        //{

        //}
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            Vector3 vPos = transform.TransformPoint(mesh.vertices[i]);
            Vector3 dir = (vPos - light.position).normalized;
            RaycastHit hit;
            Physics.Raycast(new Ray(light.position, dir), out hit, LayerMask.NameToLayer("Obstacles"));
            points.Add(hit.point);
        }
    }
