using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePlayer : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer bodyRender;

    [SerializeField]
    private MeshRenderer armRender1;

    [SerializeField]
    private LineRenderer lineRenderer1;

    [SerializeField]
    private MeshRenderer armRender2;

    [SerializeField]
    private LineRenderer lineRenderer2;

    /// <summary>
    /// Hides or shows the player by enabling or disabling rendering components. This could all be done in script for neatness.
    /// </summary>
    /// <param name="isEnabled">If the player should be shown or not.</param>
    public void EnableRenderers(bool isEnabled)
    {
        bodyRender.enabled = isEnabled;
        armRender1.enabled = isEnabled;
        armRender2.enabled = isEnabled;
        lineRenderer1.enabled = isEnabled;
        lineRenderer2.enabled = isEnabled;
    }



}
