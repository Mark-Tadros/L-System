// Controls and creates the L-System based on the characters and rules.
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Collections;
using TMPro;

public class LSystem : MonoBehaviour {
    public Transform tree;
    public bool animate; public GameObject loading;

    private string axiom;
    private string current; public TextMeshProUGUI currentText;
    public int iteration;
    public TextMeshProUGUI iterationText;
    public int speed;
    public List<char> characters; public List<string> rules;
    public float length; [Range(0, 1)] public float lengthVariance;
    public float angle; [Range(0, 1)] public float angleVariance;

    public string[] savedTemplates; public Color[] colours;
    public GameObject line;
    
    bool drawing = false; public float topPosition; float botPosition;

    // Loops through each iteration as the user continues to press Space up until a certain point to prevent excess usage.
    private void Update() { if (Input.GetKeyDown(KeyCode.Space)) { if (iteration < 7) { iteration++; CreateRules(); } } }
    private void Awake() { CreateRules(); }
    public void CreateRules()
    {
        StopAllCoroutines();
        StringBuilder next = new StringBuilder();
        Dictionary<char, string> ruleset = new Dictionary<char, string>();
        axiom = characters[0].ToString(); current = axiom;

        // Updates the Characters and Rules based on these specific ruleset.
        for (int i = 0; i < rules.Count; i++) { ruleset.Add(characters[i], rules[i]); }
        // Then loops through the current string and updates its values.
        for (int x = 0; x < iteration; x++)
        {
            next = new StringBuilder();
            char[] characters = current.ToCharArray();
            for (int y = 0; y < characters.Length; y++)
            {
                if (ruleset.ContainsKey(characters[y])) next.Append(ruleset[characters[y]]);
                else next.Append(characters[y].ToString());
            }
            current = next.ToString();
        }
        // Resets the previous Trees values before re-starting the coroutine.
        transform.localPosition = Vector3.zero; transform.localRotation = Quaternion.identity;
        foreach (Transform child in tree) Destroy(child.gameObject);

        currentText.text = "C:\\User\\CurrentString> " + current;
        iterationText.text = "> D:\\Integer\\Iteration\\Value    : " + iteration + ".0f";
        StartCoroutine(CreateLines());
    }
    private IEnumerator CreateLines()
    {
        List<Vector3> positions = new List<Vector3>(); List<Quaternion> rotations = new List<Quaternion>();
        topPosition = 1; botPosition = 0;
        drawing = true;
        if (animate) StartCoroutine(LerpCamera());
        else loading.SetActive(true);
        // Loops through each string and draws the conditions based on each value.
        for (int i = 0; i < current.Length; i++)
        {
            switch (current[i])
            {
                // If the line is an X, then move forward without creating anything.
                case 'X':
                    break;
                    
                // If its an F then create a Line and set it to the transform.position of the L-System.
                case 'F':
                    bool leaf = false; LineRenderer lineRenderer; Color newColour = colours[0];
                    lineRenderer = Instantiate(line).GetComponent<LineRenderer>();
                    lineRenderer.gameObject.transform.SetParent(tree);

                    lineRenderer.SetPosition(0, transform.position);
					transform.Translate(Vector3.up * (length + (Random.Range(-length * lengthVariance, length * lengthVariance))));
                    
                    lineRenderer.SetPosition(1, transform.position);

                    // Determines if this is the last line in the sequence, and adds some leaf attributes to differentiate it.
                    if (i + 1 < current.Length)
                    {
                        if (current[i + 1] == ']') leaf = true;
                        else
                        {
                            for (int x = i + 1; x < current.Length; x++)
                            {
                                if (current[x] == 'F') break;
                                if (current[x] == ']') { leaf = true; break; }
                            }                            
                        }                        
                    }
                    if (leaf) { lineRenderer.startWidth = 1f; lineRenderer.endWidth = 0f; newColour = colours[1]; }
                    lineRenderer.material.color = newColour;

                    if (animate && i % speed == 0) yield return null;
                    // Stores the value for the highest and lowest points.
                    if (topPosition < transform.position.y) topPosition = transform.position.y; if (botPosition > transform.position.y) botPosition = transform.position.y;
                    break;

                //Rotates the Line based on whether its calling a + or -.
                case '+':
                    transform.Rotate(Vector3.forward * angle * (1 + (angleVariance * Random.Range(-1.0f, 1.0f))));
                    break;

                case '-':
                    transform.Rotate(Vector3.back * angle * (1 + (angleVariance * Random.Range(-1.0f, 1.0f))));
                    break;

                // Saves the position of that branch.
                case '[':
                    positions.Add(transform.position); rotations.Add(transform.rotation);
                    break;

                // Continues drawing from the previous saved position when that branch is finished.
                case ']':
                    Vector3 lastPosition = positions[positions.Count - 1];
                    positions.RemoveAt(positions.Count - 1);
                    Quaternion lastRotation = rotations[rotations.Count - 1];
                    rotations.RemoveAt(rotations.Count - 1);

                    transform.position = lastPosition; transform.rotation = lastRotation;
                    break;
            }
        }
        drawing = false;
        if (!animate) StartCoroutine(LerpCamera());
        loading.SetActive(false);
    }
    // Lerps the Cameras position based on the animation of the Tree.
    IEnumerator LerpCamera()
    {
        Vector3 newPosition;
        while (drawing)
        {
            if (animate)
            {
                newPosition = new Vector3(0, (topPosition - botPosition) / 2, -1);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPosition, speed);
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, ((topPosition - botPosition + 1) * (Screen.width / Screen.height)) / 1.5f, speed);
            }
            yield return null;
        }
        newPosition = new Vector3(0, (topPosition - botPosition) / 2, -1);
        Camera.main.transform.position = newPosition;
        Camera.main.orthographicSize = ((topPosition - botPosition + 1) * (Screen.width / Screen.height)) / 1.5f;

        transform.localPosition = Vector3.zero; transform.localRotation = Quaternion.identity;
    }
}