using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDebugger : MonoBehaviour
{
    //Vector3[] points;
    public Transform LightPos;
    public Transform ponte;
    public GameObject placeHolder;
    public List<Vector3> points;
    [Range(0, 300)]
    public float lenght;
    public Light spotLight;
    bool created;
    public Vector3 meshColliderCheckerPos;
    public bool Inside;
    // Start is called before the first frame update

    //CHECKER PER POSIZIONE
    public Transform Up, Left, Right;

    float shadowAngle;
   
    void Start()
    {
        points = new List<Vector3>();
        spotLight = LightPos.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        Vector3 direction = transform.position - LightPos.position;
        float angle = Vector3.Angle(LightPos.forward, direction);
        
        if (angle < spotLight.spotAngle * 0.5f)
        {
            meshColliderCheckerPos=Vector3.zero;
           //Debug.Log("SOno Dentro il cono di luce");
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
                Vector3 meshVertexWS = transform.TransformPoint(mesh.vertices[i]);

                meshColliderCheckerPos += hit.point + meshVertexWS;

                Vector3 shadowDir = meshVertexWS - LightPos.position;

                shadowAngle += Vector3.Angle(LightPos.forward, shadowDir);
            }
            meshColliderCheckerPos /= (mesh.vertices.Length / 3) * 2;
            shadowAngle /= (mesh.vertices.Length / 3);




        }
        else
        {
           //Debug.Log("sonofuorui");
        }

        if(!created) //NON FUNZIONA
        {
            GameObject go = new GameObject();
            go.transform.position = meshColliderCheckerPos;
        //    go.AddComponent<MeshCollider>();
        //    Mesh m = new Mesh();
        //    m.vertices = points.ToArray();
        //    go.GetComponent<MeshCollider>().sharedMesh = m;
            created = true;
        }

        Inside = ShadowColliderCheck();

    }
    bool ShadowColliderCheck() //funziona!
    {
        //  HARDCODATO, BISOGNERà PASSARGLI NEL METODO I PUNTI DA CHECKARE.
        //proviamo controllo a cono perchè è il migliore
        //IL CONO COME ANGOLO NON HA QUELLO DELLA LUCE, MA QUELLO CHE SI CREA TRA LA LUCE E I VERTICI LATERALI CHE TOCCA.
        Vector3 light2checker = Left.position - LightPos.position;
        float coneAngle = Vector3.Angle(LightPos.forward, light2checker);
        return coneAngle <= shadowAngle;

    }


}
