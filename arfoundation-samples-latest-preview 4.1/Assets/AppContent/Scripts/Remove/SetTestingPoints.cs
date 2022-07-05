using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTestingPoints : MonoBehaviour
{
    public Transform AnchorPoint;
    public Button AddButton;
    public Vector3 Point1;
    public Vector3 Point2;
    public Vector3 Point3;

   // Start is called before the first frame update
   IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        AnchorPoint.position = Point1;
        AddButton.onClick.Invoke();
        AnchorPoint.position = Point2;
        AddButton.onClick.Invoke();

      // AnchorPoint.position = Point3;
      // AddButton.onClick.Invoke();
    }

}
