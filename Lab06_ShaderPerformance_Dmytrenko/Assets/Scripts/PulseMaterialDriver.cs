using UnityEngine;

[RequireComponent(typeof(Renderer))]

public sealed class PulseMaterialDriver : MonoBehaviour

{

[SerializeField, Range(0f, 10f)]

private float pulseSpeed = 2f;

private Renderer targetRenderer;

private Material originalMaterial;

private Material runtimeMaterial;

private float appliedSpeed = float.NaN;

private static readonly int SpeedId =

Shader.PropertyToID("_PulseSpeed");

private void Awake()

{

targetRenderer = GetComponent<Renderer>();

originalMaterial = targetRenderer.sharedMaterial;

if (originalMaterial == null ||

!originalMaterial.HasProperty(SpeedId))

{

Debug.LogError("Assign PulseGraph material", this);

enabled = false;

return;

}

runtimeMaterial = new Material(originalMaterial);

targetRenderer.sharedMaterial = runtimeMaterial;

}

private void Update()

{

if (runtimeMaterial == null ||

Mathf.Approximately(appliedSpeed, pulseSpeed)) return;

runtimeMaterial.SetFloat(SpeedId, pulseSpeed);

appliedSpeed = pulseSpeed;

}

private void OnDestroy()

{

if (runtimeMaterial == null) return;

if (targetRenderer != null &&

targetRenderer.sharedMaterial == runtimeMaterial)

targetRenderer.sharedMaterial = originalMaterial;

Destroy(runtimeMaterial);

}

}

