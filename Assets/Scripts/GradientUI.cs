using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GradientUI : MonoBehaviour
{
    public GameObject bar;
    public GameObject controllerPrefab;
    public GameObject colourSectionPrefab;
    public class Node
    {
        public Color color;
        [Range(0f, 1f)]
        public float position;
        public string name;
        public ColourSectionController csc;

        public Node(Color color, float position, string name)
        {
            this.color = color;
            this.position = position;
            this.name = name;
        }
    }

    public List<Node> nodes = new List<Node>();

    // Start is called before the first frame update
    void Start()
    {
        CreateNode("Initial 1", 1f, new Color(71f / 255f, 130f / 255f, 200f / 255f));
        CreateNode("Initial 2", 0.5f);

        nodes = nodes.OrderBy(x => x.position).ToList();
    }

    void CreateNode(string name, float position, Color? color = null)
    {
        if (color == null)
        {
            color = Color.white;
        }

        GameObject temp = Instantiate(controllerPrefab, bar.transform);
        ColourSectionController csc = temp.GetComponent<ColourSectionController>();

        csc.bar = bar.GetComponent<RectTransform>();
        Node node = new Node((Color)color, position, name);
        node.csc = csc;
        nodes.Add(node); 
        csc.node = node;
        CreateSection(node);
        csc.SetColour();

        temp.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
        temp.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);

        temp.transform.position = new Vector2(temp.transform.position.x, Utils.Map(node.position, 0, 1, csc.bar.rect.yMin + csc.bar.position.y, csc.bar.rect.yMax + csc.bar.position.y));

    }

    void CreateSection(Node node)
    {
            GameObject temp = Instantiate(colourSectionPrefab, bar.transform);
            node.csc.colourSection = temp;

            float bottom = 0f;
            float top = 0f;

            bottom = node.csc.bar.rect.yMin + node.csc.bar.position.y;
            top = Utils.Map(node.position, 0, 1, node.csc.bar.rect.yMin + node.csc.bar.position.y, node.csc.bar.rect.yMax + node.csc.bar.position.y);
            
            RectTransform rt = temp.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0f);
            rt.anchorMax = new Vector2(0.5f, 0f);
            Debug.Log(top - bottom);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, top - bottom);
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, (top - bottom) / 2f);
    }
}
