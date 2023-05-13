using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ILightableObject : MonoBehaviour
{
    public abstract string _name { get; }
    public abstract void LightObject(GameObject lightObject);
    public abstract void DimObject(GameObject lightObject);

}
