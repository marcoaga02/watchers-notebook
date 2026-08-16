using UnityEngine;

public class HeightVisual : MonoBehaviour
{
    [SerializeField] private Transform body;
    [SerializeField] private Transform shadow;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SpriteMask waterMask;
    [SerializeField] private Creature creature;

    [Header("Fly")]
    [SerializeField] private float flyHeight = 0.8f;
    [SerializeField] private float groundY = -0.025f;

    [Header("Swim")]
    [SerializeField] private float maskSubmergedY = 1f;
    [SerializeField] private float maskDryY = 0f;

    [Header("Shadow")]
    [Tooltip("How much the shadow shrinks while flying, relative to its own authored scale (1 = no change).")]
    [SerializeField] private float flyingShadowScale = 0.6f;

    [SerializeField] private float lerpSpeed = 12f;

    private float _bodyTargetY;
    private float _maskTargetY;
    private float _shadowTargetScaleX;
    private float _shadowGroundScaleX;
    private float _shadowAspectRatio;

    private void Start()
    {
        _bodyTargetY = groundY;
        _maskTargetY = maskDryY;

        if (waterMask != null)
        {
            // the mask stays always enabled: it's its position that determines the effect
            waterMask.enabled = true;
            bodyRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            waterMask.transform.localPosition = new Vector3(0f, maskDryY, 0f);
        }

        if (shadow != null)
        {
            // whatever scale was authored in the Inspector is "normal, grounded":
            // flying only shrinks relative to that, it never imposes a fixed size
            _shadowGroundScaleX = shadow.localScale.x;
            _shadowAspectRatio = shadow.localScale.y / _shadowGroundScaleX;
            _shadowTargetScaleX = _shadowGroundScaleX;
        }
    }

    private void Update()
    {
        var probe = TerrainProbe.Instance;
        var capability = probe.GetRequiredCapability(transform.position);
        var swimming = waterMask != null && capability == probe.SwimmingSigil && creature.CanUse(probe.SwimmingSigil);
        var flying = capability == probe.FlyingSigil && creature.CanUse(probe.FlyingSigil);

        _bodyTargetY = flying ? flyHeight : groundY;
        _maskTargetY = swimming ? maskSubmergedY : maskDryY;
        _shadowTargetScaleX = flying ? _shadowGroundScaleX * flyingShadowScale : _shadowGroundScaleX;

        if (shadow != null)
        {
            shadow.gameObject.SetActive(!swimming);
        }
    }

    private void LateUpdate()
    {
        var t = Time.deltaTime * lerpSpeed;

        var b = body.localPosition;
        b.y = Mathf.Lerp(b.y, _bodyTargetY, t);
        body.localPosition = b;

        if (waterMask != null)
        {
            var m = waterMask.transform.localPosition;
            m.y = Mathf.Lerp(m.y, _maskTargetY + b.y, t);
            waterMask.transform.localPosition = m;
        }

        if (shadow != null)
        {
            var s = shadow.localScale;
            s.x = Mathf.Lerp(s.x, _shadowTargetScaleX, t);
            s.y = s.x * _shadowAspectRatio;
            shadow.localScale = s;
        }
    }
}