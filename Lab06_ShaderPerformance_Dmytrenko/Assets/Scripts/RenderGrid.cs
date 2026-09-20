using UnityEngine;

using UnityEngine.Rendering;

public sealed class RenderGrid : MonoBehaviour

{

[SerializeField] private Material sharedMaterial;

[SerializeField, Range(1, 40)] private int side = 20;

[SerializeField] private bool castShadows = true;

private void Start()

{

if (sharedMaterial == null)

{

Debug.LogError("Assign grid material", this);

enabled = false;

return;

}

for (int z = 0; z < side; z++)

for (int x = 0; x < side; x++)

{

GameObject cube =

GameObject.CreatePrimitive(PrimitiveType.Cube);

cube.transform.SetParent(transform, false);

cube.transform.localPosition = new Vector3(

(x - (side - 1) * 0.5f) * 1.5f,

0.5f,

(z - (side - 1) * 0.5f) * 1.5f);

Destroy(cube.GetComponent<Collider>());

Renderer renderer = cube.GetComponent<Renderer>();

renderer.sharedMaterial = sharedMaterial;

renderer.shadowCastingMode = castShadows

? ShadowCastingMode.On

: ShadowCastingMode.Off;

}

}

}