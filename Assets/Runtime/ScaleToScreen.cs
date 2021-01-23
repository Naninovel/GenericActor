using Naninovel;
using UnityEngine;

[RequireComponent(typeof(RenderCanvas))]
public class ScaleToScreen : MonoBehaviour
{
    private class Matcher : CameraMatcher
    {
        protected override Vector2 ReferenceSize { get; }

        private readonly Transform transform;
        private readonly Vector3 initialScale;

        public Matcher (ICameraManager cameraManager, Vector2 referenceSize, Transform transform, float updateDelay) 
            : base(cameraManager, updateDelay, transform.gameObject)
        {
            this.transform = transform;
            initialScale = transform.localScale;
            ReferenceSize = referenceSize;
        }

        protected override void ApplyScale (float scaleFactor)
        {
            transform.localScale = initialScale * scaleFactor;
        }
    }
    
    [SerializeField] private float updateDelay = .1f;
    [SerializeField] private CameraMatchMode matchMode = default;
    [SerializeField] private float customMatchRatio = default;

    private Matcher matcher;

    private void Awake ()
    {
        var renderCanvas = GetComponent<RenderCanvas>();
        var cameraManager = Engine.GetService<ICameraManager>();
        matcher = new Matcher(cameraManager, renderCanvas.Size, transform, updateDelay);
        matcher.MatchMode = matchMode;
        matcher.CustomMatchRatio = customMatchRatio;
    }

    private void OnEnable () => matcher.Start();
    
    private void OnDisable () => matcher.Stop();
}
