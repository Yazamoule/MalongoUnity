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

    [SerializeField] GameObject vectorUiToInstenciate = null;

    [SerializeField] Vector2 uiVectorStart = new Vector2(1000, 1000);
    [SerializeField] Vector2 labelOffset = new Vector2(-30, -30);

    [SerializeField] float uiScale = 50f;


    Canvas canva;

    Transform defaultStartPos = null;

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
            GameObject uiVector = Instantiate(vectorUiToInstenciate, canva.transform);

            ray = new RayCustom(_label, _color, _drawWorld, _drawHorizontalUi);

            ray.uILineRenderer = uiVector.GetComponent<UILineRenderer>();
            ray.uILineRenderer.points[0] = uiVectorStart;
            ray.uILineRenderer.color = _color;

            ray.text = uiVector.GetComponentInChildren<TextMeshProUGUI>();
            ray.text.color = _color;
            ray.text.text = _label;

            uiRays.Add(_label, ray);
        }

        ray.vector = _vector;
        ray.start = _start;
    }

    private void FixedUpdate()
    {
        if (!drawAll)
            return;

        if (drawRay)
        {
            UpdateVectorWorld();
        }

        if (drawHorizontal)
        {
            UpdateVectorUi();
        }
    }

    void UpdateVectorWorld()
    {
        foreach (KeyValuePair<string, RayCustom> pair in uiRays)
        {
            RayCustom ray = pair.Value;
            
            if (!ray.drawWorld)
                continue;
            
            Debug.DrawLine(ray.start, ray.start + ray.vector, ray.color, 1f);
        }
    }

    void UpdateVectorUi()
    {
        foreach (KeyValuePair<string, RayCustom> pair in uiRays)
        {
            RayCustom ray = pair.Value;

            if(!ray.drawUiHorizontal)
                continue;

            Vector2 lineEnd = uiVectorStart + uiScale * new Vector2(Mathf.Sqrt(ray.vector.x * ray.vector.x + ray.vector.z * ray.vector.z), ray.vector.y);


            ray.uILineRenderer.points[1] = lineEnd;
            ray.uILineRenderer.ForceUpdateMesh();


            ray.text.rectTransform.localPosition = lineEnd + labelOffset;

            //ray.uiLineTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ray.vector.magnitude * 100);
            //ray.uiLineTransform.rotation = Quaternion.FromToRotation(Vector3.right, ray.vector);
        }
    }
}