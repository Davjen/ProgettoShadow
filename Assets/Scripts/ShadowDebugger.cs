using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDebugger : MonoBehaviour
{
    //Vector3[] points;
    public Transform LightPos;
    public Transform ponte;
    public GameObject placeHolder;
    private List<Vector3> points;
    [Range(0, 300)]
    public float lenght;
    [SerializeField]
    private LayerMask layerMask;
    public Light spotLight;
    bool created;
    Vector3 meshColliderCheckerPos;
    // Start is called before the first frame update
    void Start()
    {
        points = new List<Vector3>();
        spotLight = LightPos.GetComponent<Light>();
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
        Debug.Log(mesh.vertices.Length);
        Debug.Log(mesh.vertices.Length / 3);

        //float angle = Mathf.Cos(Vector3.Dot(LightPos.forward, transform.forward));
        //if(angle<=Mathf.Deg2Rad*spotLight.spotAngle)
        Vector3 direction = transform.position - LightPos.position;
        float angle = Vector3.Angle(LightPos.forward, direction);
        Debug.Log(spotLight.spotAngle);
        if (angle < spotLight.spotAngle * 0.5f)
        {
            meshColliderCheckerPos=Vector3.zero;
            Debug.Log("SOno Dentro il cono di luce");
            for (int i = 0; i <mesh.vertices.Length / 3; i++)
            {
                Vector3 vPos = transform.TransformPoint(mesh.vertices[i]);
                Vector3 dir = (vPos - LightPos.position).normalized;
                RaycastHit hit;
                if (Physics.Raycast(new Ray(LightPos.position, dir), out hit))
                {
                    if (hit.transform.tag == "Shadowable")
                    {
                        GameObject obj = Instantiate(placeHolder);
                        obj.transform.position = hit.point;
                    }

                }
                Debug.DrawRay(LightPos.position, dir * lenght);
                points.Add(hit.point);
                points.Add(mesh.vertices[i]);
                meshColliderCheckerPos += hit.point + mesh.vertices[i] ;
            }
            meshColliderCheckerPos/= (mesh.vertices.Length / 3);
            


        }
        else
        {
            Debug.Log("sonofuorui");
        }

        if(!created)
        {
            GameObject go = new GameObject();
            go.transform.position = meshColliderCheckerPos;
            go.AddComponent<MeshCollider>();
            Mesh m = new Mesh();
            m.vertices = points.ToArray();
            go.GetComponent<MeshCollider>().sharedMesh = m;
            created = true;
        }
    }
}
