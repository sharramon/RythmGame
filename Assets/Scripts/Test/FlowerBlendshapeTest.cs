using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBlendshapeTest : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer m_flowerMesh;

    private void Start()
    {
        if (this.GetComponent<SkinnedMeshRenderer>())
        {
            m_flowerMesh = this.GetComponent<SkinnedMeshRenderer>();
            GetBlendshapes(m_flowerMesh);
        }
    }

    private void GetBlendshapes(SkinnedMeshRenderer skinnedMesh)
    {
        Mesh mesh = skinnedMesh.sharedMesh;

        for(int i = 0; i < mesh.blendShapeCount; i++)
        {
            string blendShapeName = mesh.GetBlendShapeName(i);
            float blendShapeWeight = skinnedMesh.GetBlendShapeWeight(i);

            Debug.Log("Blend Shape Name: " + blendShapeName + ", Weight: " + blendShapeWeight);
        }
    }
}
