using UnityEngine;

public class HeightVisual : MonoBehaviour
{
    [SerializeField] private Transform body;
    [SerializeField] private Transform shadow;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SpriteMask waterMask;
    [SerializeField] private TerrainProbe probe;
    [SerializeField] private CapabilitySigil swimmingSigil;
    [SerializeField] private CapabilitySigil flyingSigil;

    [Header("Fly")]
    [SerializeField] private float flyHeight = 0.8f;
    [SerializeField] private float groundY = -0.025f;

    [Header("Swim")]
    [SerializeField] private float maskSubmergedY = 1f;
    [SerializeField] private float maskDryY = 0f;
    
    [SerializeField] private float lerpSpeed = 12f;

    private float _bodyTargetY;
    private float _maskTargetY;
    private float _shadowTargetScaleX = 3f;
    
    private void Start()
    {
        // the mask stays always enabled: it's its position that determines the effect
        waterMask.enabled = true;
        bodyRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        _bodyTargetY = groundY;
        _maskTargetY = maskDryY;
        waterMask.transform.localPosition = new Vector3(0f, maskDryY, 0f);
    }

    private void Update()
    {
        var capability = probe.GetRequiredCapability(transform.position);
        var swimming = capability == swimmingSigil;
        var flying = capability == flyingSigil;

        _bodyTargetY = flying ? flyHeight : groundY;
        _maskTargetY = swimming ? maskSubmergedY : maskDryY;
        _shadowTargetScaleX = flying ? 1.8f : 3f;

        shadow.gameObject.SetActive(!swimming);
    }

    private void LateUpdate()
    {
        var t = Time.deltaTime * lerpSpeed;

        var b = body.localPosition;
        b.y = Mathf.Lerp(b.y, _bodyTargetY, t);
        body.localPosition = b;

        var m = waterMask.transform.localPosition;
        m.y = Mathf.Lerp(m.y, _maskTargetY + b.y, t);
        waterMask.transform.localPosition = m;

        var s = shadow.localScale;
        s.x = Mathf.Lerp(s.x, _shadowTargetScaleX, t);
        s.y = s.x * 0.133f;
        shadow.localScale = s;
    }
}