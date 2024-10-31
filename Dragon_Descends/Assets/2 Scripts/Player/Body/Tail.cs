using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tail : BodyPart
{
    private void LateUpdate()
    {
        transform.up = prevBodyPart.transform.position - transform.position;
    }

    public void ChangeChaseBodyPart(GameObject bp)
    {
        prevBodyPart = bp;
        SetupJoint();
    }
}
