using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapChecker : MonoBehaviour
{
    public Transform Left, Right, Top;
    public Transform LightPos;
    private Light spotLight;

    bool isInside;
    // Start is called before the first frame update
    void Start()
    {
        spotLight = LightPos.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        isInside = ShadowColliderCheck();
    }

    bool ShadowColliderCheck()
    {
        //  HARDCODATO, BISOGNERà PASSARGLI NEL METODO I PUNTI DA CHECKARE.
        //proviamo controllo a cono perchè è il migliore
        //IL CONO COME ANGOLO NON HA QUELLO DELLA LUCE, MA QUELLO CHE SI CREA TRA LA LUCE E I VERTICI LATERALI CHE TOCCA.
        Vector3 light2checker = Left.position - LightPos.position;
        float coneAngle = Vector3.Angle(LightPos.forward, light2checker);
        return coneAngle <= spotLight.spotAngle * 0.5f;

    }
}
