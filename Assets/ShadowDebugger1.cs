using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDebugger1 : MonoBehaviour
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

    public float coneAngle, angleBetweenLightandCorner;
    //float shadowAngle;

    public Vector3 LeftVertex, RightVertex;

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
            meshColliderCheckerPos = Vector3.zero;
            LeftVertex = transform.position;
            RightVertex = transform.position;
            //Debug.Log("SOno Dentro il cono di luce");
            
            for (int i = 0; i < mesh.vertices.Length/2; i++)
            {
                Vector3 vPos = transform.TransformPoint(mesh.vertices[i]);
                Vector3 dir = (vPos - LightPos.position).normalized;
                RaycastHit hit;
                if (Physics.Raycast(new Ray(LightPos.position, dir), out hit))
                {
                    if (hit.transform.tag == "Shadowable")
                    {
                        //GameObject obj = Instantiate(placeHolder);
                        //obj.transform.position = hit.point;
                    }

                }
               // Debug.DrawRay(LightPos.position, dir * lenght);
                points.Add(hit.point);
                points.Add(mesh.vertices[i]);
                //CHECK VERTEX

                    

                    if ((transform.TransformPoint(mesh.vertices[i]).x )<( LeftVertex.x))
                    {
                        LeftVertex = transform.TransformPoint(mesh.vertices[i]);
                    }
                    if((transform.TransformPoint(mesh.vertices[i]).x )>(RightVertex.x))
                    {
                        RightVertex = transform.TransformPoint(mesh.vertices[i]);
                    }


                Vector3 meshVertexWS = transform.TransformPoint(mesh.vertices[i]);

                meshColliderCheckerPos += hit.point + meshVertexWS;

                Vector3 shadowDir = meshVertexWS - LightPos.position;

                //shadowAngle += Vector3.Angle(LightPos.forward, shadowDir);
            }
            meshColliderCheckerPos /= (mesh.vertices.Length/2) * 2;
            //shadowAngle /= (mesh.vertices.Length / 3);




        }
        else
        {
            //Debug.Log("sonofuorui");
        }

        if (!created) //NON FUNZIONA
        {
            GameObject go = new GameObject();
            go.transform.position = meshColliderCheckerPos;
            //    go.AddComponent<MeshCollider>();
            //    Mesh m = new Mesh();
            //    m.vertices = points.ToArray();
            //    go.GetComponent<MeshCollider>().sharedMesh = m;
            created = true;
        }

        Inside = ShadowColliderCheck2();
        points.Clear();
    }
    //bool ShadowColliderCheck() //funziona SELL'OBJ IN OMBRA è QUELLO DA SPOSTARE E LA LUCE E L'OGGETTO CHE FA OMBRA SONO FISSI.
    //{
    //    //  HARDCODATO, BISOGNERà PASSARGLI NEL METODO I PUNTI DA CHECKARE.
    //    //proviamo controllo a cono perchè è il migliore
    //    //IL CONO COME ANGOLO NON HA QUELLO DELLA LUCE, MA QUELLO CHE SI CREA TRA LA LUCE E I VERTICI LATERALI CHE TOCCA.
    //    Vector3 light2checker = Left.position - LightPos.position;
    //    coneAngle = Vector3.Angle(LightPos.forward, light2checker);
    //    return coneAngle <= shadowAngle;

    //}

    bool ShadowColliderCheck2()//si muove la luce o il cubo
    {
        Vector3 dir2Left = (LeftVertex - LightPos.position).normalized;
        Vector3 dir2Right = (RightVertex - LightPos.position).normalized;

        Vector3 directionFromLight2Obstacle = (transform.position - LightPos.position).normalized;
        Vector3 dirBetweenLightandCorner = Left.position- LightPos.position;
         angleBetweenLightandCorner = Vector3.Angle(directionFromLight2Obstacle, dirBetweenLightandCorner);

        Debug.DrawRay(LightPos.position, dirBetweenLightandCorner,Color.red);
        Debug.DrawRay(LightPos.position, dir2Right*100, Color.blue);
        Debug.DrawRay(LightPos.position, dir2Left*100, Color.green);
        Debug.DrawRay(LightPos.position, (transform.position - LightPos.position).normalized*100, Color.cyan);

        coneAngle = Vector3.Angle(dir2Left, dir2Right)*0.5f;
        return coneAngle-1 >= angleBetweenLightandCorner;
    }


}
