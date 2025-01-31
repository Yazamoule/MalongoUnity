using NUnit.Framework;
using Radishmouse;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;  // For working with UI components like Image

public class DebugCustom : MonoBehaviour
{
    public class RayCustom
    {
        public string label;
        public float scale = 1f;
        public Color color = Color.white;
        public bool drawWorld = true;
        public bool drawUiSideView = true;
        public bool drawUiTopView = true;

        public bool updatedThisFrame = false;

        public Vector3 start = Vector3.zero;
        public Vector3 vector = Vector3.zero;

        public TextMeshProUGUI textSideView = null;
        public TextMeshProUGUI textTopView = null;

        public UILineRenderer uiSideViewLineRenderer = null;
        public UILineRenderer uiTopViewLineRenderer = null;


        public RayCustom(string _label, Color _color, bool _drawWorld, bool _drawUiHorizontal)
        {
            label = _label;
            color = _color;
            drawWorld = _drawWorld;
            drawUiSideView = _drawUiHorizontal;
        }
    }

    public bool drawAll = true;
    public bool drawRay = true;
    public bool drawSideView = true;
    public bool drawTopView = true;
    public bool drawText = true;

    [SerializeField, UnityEngine.Range(-0.1f, 2f)] float timeScale = -0.1f;

    [SerializeField] GameObject vectorUiToInstenciate = null;

    [SerializeField] Vector2 labelOffset = new Vector2(-30, -30);

    [SerializeField] float uiScale = 50f;

    [SerializeField] float worldRayLife = 2;

    [SerializeField] Vector2 uiSideViewVectorStart = new Vector2(1000, 1000);
    [SerializeField] Vector2 uiTopViewVectorStart = new Vector2(0, 0);

    Canvas canva;

    Transform defaultStartPos = null;

    bool flagFixedUpdate = false;
    bool flagUpdate = false;

    [SerializeField] float drawCooldown = 0.5f;
    float drawTimer = 0f;



    Dictionary<string, RayCustom> uiRays = new Dictionary<string, RayCustom>();


    private void Awake()
    {
        GameManager.Instance.debug = this;
        canva = GetComponent<Canvas>();
    }

    public void Start()
    {
        defaultStartPos = LevelManager.Instance.player.move.playerFoward;
    }

    public void OnDestroy()
    {
        GameManager.Instance.debug = null;
    }

    public void DrawRay(string _label, Color _color, Vector3 _vector, Vector3? _start, bool _drawWorld = true, bool _drawHorizontalUi = true)
    {
        RayCustom ray;
        if (!uiRays.TryGetValue(_label, out ray))
        {

            ray = new RayCustom(_label, _color, _drawWorld, _drawHorizontalUi);

            if (ray.drawUiSideView)
            {
                GameObject uiVector = Instantiate(vectorUiToInstenciate, canva.transform);

                ray.uiSideViewLineRenderer = uiVector.GetComponent<UILineRenderer>();
                ray.uiSideViewLineRenderer.points[0] = uiSideViewVectorStart;
                ray.uiSideViewLineRenderer.color = _color;

                ray.textSideView = uiVector.GetComponentInChildren<TextMeshProUGUI>();
                ray.textSideView.color = _color;
                ray.textSideView.text = _label;
            }

            if (ray.drawUiTopView)
            {
                GameObject uiVector = Instantiate(vectorUiToInstenciate, canva.transform);

                ray.uiTopViewLineRenderer = uiVector.GetComponent<UILineRenderer>();
                ray.uiTopViewLineRenderer.points[0] = uiTopViewVectorStart;
                ray.uiTopViewLineRenderer.color = _color;

                ray.textTopView = uiVector.GetComponentInChildren<TextMeshProUGUI>();
                ray.textTopView.color = _color;
                ray.textTopView.text = _label;
            }

            uiRays.Add(_label, ray);
        }

        ray.updatedThisFrame = true;

        ray.vector = _vector;
        ray.start = _start ?? defaultStartPos.position;

    }

    private void Update()
    {
        if (timeScale > 0)
        {
            Time.timeScale = timeScale;
        }




        if (flagFixedUpdate == true)
        {
            flagFixedUpdate = false;
            Draw();
        }
        else
        {
            flagUpdate = true;
        }


    }

    private void FixedUpdate()
    {
        drawTimer += Time.fixedDeltaTime;

        if (flagUpdate == true)
        {
            flagUpdate = false;
            Draw();
        }
        else
        {
            flagFixedUpdate = true;
        }
    }

    void Draw()
    {
        if (!drawAll)
            return;

        bool cooldownDrawRayOk = false;
        if (drawCooldown < drawTimer)
        {
            drawTimer -= drawCooldown;
            cooldownDrawRayOk = true;
        }

        foreach (KeyValuePair<string, RayCustom> pair in uiRays)
        {
            RayCustom ray = pair.Value;

            if (drawRay && cooldownDrawRayOk)
            {
                UpdateVectorWorld(ray);
            }

            if (drawSideView)
                UpdateUiSideView(ray);

            if (drawTopView)
                UpdateUiTopView(ray);

            ray.updatedThisFrame = false;
        }
    }

    void UpdateVectorWorld(RayCustom _ray)
    {
        if (!_ray.updatedThisFrame || !_ray.drawWorld)
            return;

        Debug.DrawLine(_ray.start, _ray.start + _ray.vector, _ray.color, worldRayLife);

    }

    void UpdateUiSideView(RayCustom _ray)
    {
        if (!_ray.drawUiSideView)
            return;

        if (!_ray.updatedThisFrame)
        {
            _ray.uiSideViewLineRenderer.enabled = false;
            _ray.textSideView.enabled = false;
            return;
        }
        _ray.uiSideViewLineRenderer.enabled = true;
        _ray.textSideView.enabled = true;

        Vector2 lineEnd = uiSideViewVectorStart + uiScale * new Vector2(Mathf.Sqrt(_ray.vector.x * _ray.vector.x + _ray.vector.z * _ray.vector.z), _ray.vector.y);

        _ray.uiSideViewLineRenderer.points[1] = lineEnd;

        _ray.uiSideViewLineRenderer.ForceUpdateMesh();

        _ray.textSideView.rectTransform.localPosition = lineEnd + labelOffset;

    }


    void UpdateUiTopView(RayCustom _ray)
    {
        if (!_ray.drawUiTopView)
            return;

        if (!_ray.updatedThisFrame)
        {
            _ray.uiTopViewLineRenderer.enabled = false;
            _ray.textTopView.enabled = false;
            return;
        }
        _ray.uiTopViewLineRenderer.enabled = true;
        _ray.textTopView.enabled = true;

        Vector2 lineEnd = uiTopViewVectorStart + uiScale * new Vector2(_ray.vector.x, _ray.vector.z);

        _ray.uiTopViewLineRenderer.points[1] = lineEnd;

        _ray.uiTopViewLineRenderer.ForceUpdateMesh();

        _ray.textTopView.rectTransform.localPosition = lineEnd + labelOffset;

    }
}