using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line : MonoBehaviour
{
    // Start is called before the first frame update

    //the points of the line
    public Transform firstSelected;
    public Transform secondSelected;

    //the prefab used for drawing the line   
    GameObject lineInstance;

    //the prefab used for writing the text
    GameObject textInstance;

    //thickness
    float th=0.002f;

    public void SetParamValues(Transform p1, Transform p2)
    {
        firstSelected = p1;
        secondSelected = p2;


        //create line and text
        lineInstance = transform.GetChild(0).gameObject;
        textInstance = transform.GetChild(1).gameObject;

    }

    // Update is called once per frame
    public void Update()
    {

        if (secondSelected!=null && firstSelected!=null)
        {
            //direction and center of the transform
            Vector3 dir = secondSelected.transform.position - firstSelected.transform.position;
            Vector3 center = firstSelected.transform.position + dir / 2;

            lineInstance.transform.position = center;
            lineInstance.transform.rotation = Quaternion.LookRotation(dir, firstSelected.transform.up);
            lineInstance.transform.localScale = new Vector3(0.02f, th, dir.magnitude);


            //orient the text prop
            textInstance.transform.position = center;
            if (dir.x > 0)
            {
                textInstance.transform.rotation = Quaternion.LookRotation(-dir, firstSelected.transform.right)*Quaternion.Euler(180,90,0);
                //textInstance.transform.right = dir;
            }
            else
            {
                textInstance.transform.rotation = Quaternion.LookRotation(dir, firstSelected.transform.right) * Quaternion.Euler(180,90, 0);
                //textInstance.transform.right =- dir;
            }


            textInstance.transform.position = center+ firstSelected.up*th*1.01f;

           //set the text
           Text txScp = textInstance.transform.GetChild(0).GetComponent<Text>();
            txScp.text = Mathf.Round(dir.magnitude * 1000) / 10 + " cm";
        }
    }
}
