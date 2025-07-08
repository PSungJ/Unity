using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class DOFManager : MonoBehaviour
{
    public DepthOfFieldDeprecated dof;

    public void EnableDOF()
    {
        dof.enabled = true;
    }
    public void DisableDOF()
    {
        dof.enabled = false;
    }
}
