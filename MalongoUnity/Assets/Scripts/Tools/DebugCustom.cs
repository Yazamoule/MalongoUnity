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
        public bool drawUiHorizontal = true;

        public bool updatedThisFrame = false;

        public Vector3 start = Vector3.zero;
        public Vector3 vector = Vector3.zero;

        public TextMeshProUGUI text = null;

        public UILineRenderer uILineRenderer = null;


        public RayCustom(string _label, Color _color, bool _drawWorld, bool _drawUiHorizontal)
        {
            label = _label;
            color = _color;
            drawWorld = _drawWorld;
            drawUiHorizontal = _drawUiHorizontal;
        }
    }

    public bool drawAll = true;
    public bool drawRay = true;
    public bool drawHorizontal = true;
    public bool drawText = true;

    [SerializeField, UnityEngine.Range(-0.1f, 2f)] float timeScale = -0.1f;

    [SerializeField] GameObject vectorUiToInstenciate = null;

    [SerializeField] Vector2 uiVectorStart = new Vector2(1000, 1000);
    [SerializeField] Vector2 labelOffset = new Vector2(-30, -30);

    [SerializeField] float uiScale = 50f;

    [SerializeField] float worldRayLife = 2;

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

            if (ray.drawUiHorizontal)
            {
                GameObject uiVector = Instantiate(vectorUiToInstenciate, canva.transform);

                ray.uILineRenderer = uiVector.GetComponent<UILineRenderer>();
                ray.uILineRenderer.points[0] = uiVectorStart;
                ray.uILineRenderer.color = _color;

                ray.text = uiVector.GetComponentInChildren<TextMeshProUGUI>();
                ray.text.color = _color;
                ray.text.text = _label;
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

            if (drawHorizontal)
                UpdateVectorUi(ray);

            ray.updatedThisFrame = false;
        }
    }

    void UpdateVectorWorld(RayCustom _ray)
    {
        if (!_ray.updatedThisFrame || !_ray.drawWorld)
            return;

        Debug.DrawLine(_ray.start, _ray.start + _ray.vector, _ray.color, worldRayLife);

    }

    void UpdateVectorUi(RayCustom _ray)
    {
        if (!_ray.drawUiHorizontal)
            return;

        if (!_ray.updatedThisFrame)
        {
            _ray.uILineRenderer.enabled = false;
            _ray.text.enabled = false;
            //_ray.uILineRenderer.color.WithAlpha(0f);
            return;
        }
        _ray.uILineRenderer.enabled = true;
        _ray.text.enabled = true;
        //_ray.uILineRenderer.color.WithAlpha(1f);

        Vector2 lineEnd = uiVectorStart + uiScale * new Vector2(Mathf.Sqrt(_ray.vector.x * _ray.vector.x + _ray.vector.z * _ray.vector.z), _ray.vector.y);

        _ray.uILineRenderer.points[1] = lineEnd;

        _ray.uILineRenderer.ForceUpdateMesh();

        _ray.text.rectTransform.localPosition = lineEnd + labelOffset;

    }
}