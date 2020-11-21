using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    //Unity UI script to handle all parameters.
    public LSystem LSystemScript; public List<char> characters; public List<string> rules; int templateLoop = -1;
    public TMP_InputField rulesetText; public List<GameObject> Characters; public List<GameObject> Rules;
    public TMP_InputField speedText; public TMP_InputField angleText; public TMP_InputField lengthVarianceText; public TMP_InputField angleVarianceText; public TMP_InputField iterationText;
    public Button animateTrue; public Button animateFalse; public TextMeshProUGUI templateText; public TMP_InputField templateInputText;
    private void Update() { if (Input.GetKeyDown(KeyCode.R)) { Reset(); } if (Input.GetKeyDown(KeyCode.A)) { if (LSystemScript.animate) animateTrue.onClick.Invoke(); else animateFalse.onClick.Invoke(); } if (Input.GetKeyDown(KeyCode.T)) { AddTemplate(); } }
    //Updates the characters and rule size based on the value you add. Adds random values as to not return an empty error.
    public void RuleSet(string newCharacters)
    {
        if (newCharacters != "" && !newCharacters.Contains(">"))
        {
            //if the user sets the character length to zero, to prevent bugs resets them both.
            if (int.Parse(newCharacters) <= 0)
            {
                newCharacters = 1.ToString();
                Characters[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> X"; LSystemScript.characters[0] = characters[0];
                Rules[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> F[+X]F[-X]+X"; LSystemScript.rules[0] = rules[0];
            }
            else if (int.Parse(newCharacters) > 4) newCharacters = 4.ToString();
            //Checks if the number is less than how many is active, if so deactivate them and remove them from the characters and rules.
            rulesetText.text = "";
            int count = LSystemScript.characters.Count;
            for (int i = int.Parse(newCharacters); i < count; i++)
            {
                Characters[LSystemScript.characters.Count - 1].SetActive(false);
                Rules[LSystemScript.rules.Count - 1].SetActive(false);
                LSystemScript.characters.RemoveAt(LSystemScript.characters.Count - 1);
                LSystemScript.rules.RemoveAt(LSystemScript.rules.Count - 1);
            }
            //If the number is more than how many are active, then activate them and set them active.
            for (int i = 0; i < int.Parse(newCharacters); i++)
            {
                if (!Characters[i].activeSelf)
                {
                    //Upon re-adding that character and rule, then reset it from the prefabs.
                    if (i == 1)
                    {
                        Characters[i].SetActive(true);
                        Characters[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> " + characters[1]; LSystemScript.characters.Add(characters[1]);
                        Rules[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> FF"; LSystemScript.rules.Add(rules[1]);
                    }
                    if (i == 2)
                    {
                        Characters[i].SetActive(true);
                        Characters[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> " + characters[2]; LSystemScript.characters.Add(characters[2]);
                        Rules[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> [+FX][FX][-FX]"; LSystemScript.rules.Add(rules[2]);
                    }
                    if (i == 3)
                    {
                        Characters[i].SetActive(true);
                        Characters[3].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> " + characters[3]; LSystemScript.characters.Add(characters[3]);
                        Rules[3].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> XX"; LSystemScript.rules.Add(rules[3]);
                    }
                }
                Characters[i].SetActive(true); Rules[i].SetActive(true);
            }
        }
        LSystemScript.CreateRules();
    }
    //Button commands to update values from the L-System.
    public void Animate() { LSystemScript.animate = !LSystemScript.animate; }
    public void Iterate() { if (LSystemScript.iteration < 7) { LSystemScript.iteration++; LSystemScript.CreateRules(); } }
    public void Speed(string newSpeed)
    {
        if (newSpeed != "" && !newSpeed.Contains(">"))
        {
            if (int.Parse(newSpeed) <= 0) newSpeed = 1.ToString(); else if (int.Parse(newSpeed) > 25) newSpeed = 25.ToString();
            LSystemScript.speed = int.Parse(newSpeed);
            speedText.text = "";
            speedText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Integer\\Speed\\Value        : " + LSystemScript.speed.ToString() + ".0f";
        }
    }
    public void Angle(string newAngle)
    {
        if (newAngle != "" && !newAngle.Contains(">"))
        {
            if (float.Parse(newAngle) < 0) newAngle = 0.ToString(); else if (float.Parse(newAngle) > 180) newAngle = 180.ToString();
            LSystemScript.angle = float.Parse(newAngle);
            angleText.text = "";
            if (float.Parse(newAngle) % 1 == 0) angleText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Degrees\\Angle\\Value        : " + LSystemScript.angle.ToString() + ".0°";
            else angleText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Degrees\\Angle\\Value        : " + LSystemScript.angle.ToString() + "°";
        }
        LSystemScript.CreateRules();
    }
    public void LengthVariance(string newLengthVariance)
    {
        if (newLengthVariance != "" && !newLengthVariance.Contains(">"))
        {
            if (float.Parse(newLengthVariance) < 0) newLengthVariance = 0.ToString(); else if (float.Parse(newLengthVariance) > 1) newLengthVariance = 1.ToString();
            LSystemScript.lengthVariance = float.Parse(newLengthVariance);
            lengthVarianceText.text = "";
            if (LSystemScript.lengthVariance == 0 || LSystemScript.lengthVariance == 1)
                lengthVarianceText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Float\\LengthVariance\\Value : " + LSystemScript.lengthVariance.ToString() + ".0f";
            else lengthVarianceText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Float\\LengthVariance\\Value : " + LSystemScript.lengthVariance.ToString() + "f";            
        }
        LSystemScript.CreateRules();
    }
    public void AngleVariance(string newAngleVariance)
    {
        if (newAngleVariance != "" && !newAngleVariance.Contains(">"))
        {
            if (float.Parse(newAngleVariance) < 0) newAngleVariance = 0.ToString(); else if (float.Parse(newAngleVariance) > 1) newAngleVariance = 1.ToString();
            LSystemScript.angleVariance = float.Parse(newAngleVariance);
            angleVarianceText.text = "";
            if (LSystemScript.angleVariance == 0 || LSystemScript.angleVariance == 1)
                angleVarianceText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Float\\AngleVariance\\Value  : " + LSystemScript.angleVariance.ToString() + ".0f";
            else angleVarianceText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Float\\AngleVariance\\Value  : " + LSystemScript.angleVariance.ToString() + "f";
        }
        LSystemScript.CreateRules();
    }
    public void Iteration(string newIteration)
    {
        if (newIteration != "" && !newIteration.Contains(">"))
        {
            if (int.Parse(newIteration) < 0) newIteration = 0.ToString(); else if (int.Parse(newIteration) > 7) newIteration = 7.ToString();
            LSystemScript.iteration = int.Parse(newIteration);
            iterationText.text = "";
            LSystemScript.CreateRules();
        }
        LSystemScript.CreateRules();
    }
    //Updates characters and rules based on what object you edit in inspector.
    public void UpdateRules(GameObject textObject)
    {
        string newText = textObject.GetComponent<TMP_InputField>().text.ToUpper();
        newText.Replace(" ", string.Empty);
        if (newText != "" && newText != " " && !newText.Contains(">"))
        {
            textObject.GetComponent<TMP_InputField>().text = "";
            if (textObject == Characters[0])
            {
                //Find and replace if that character already exists, before applying the new character.
                for (int i = 0; i < characters.Count; i++) { if (characters[i] == newText[0]) characters[i] = LSystemScript.characters[0]; }
                for (int i = 0; i < LSystemScript.characters.Count; i++)
                {
                    if (LSystemScript.characters[i] == newText[0])
                    {
                        LSystemScript.characters[i] = LSystemScript.characters[0]; characters[i] = LSystemScript.characters[0];
                        Characters[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + LSystemScript.characters[0];
                    }
                }
                LSystemScript.characters[0] = newText[0]; characters[0] = newText[0];
                textObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + newText;
            }
            else if (textObject == Characters[1])
            {
                for (int i = 0; i < characters.Count; i++) { if (characters[i] == newText[0]) characters[i] = LSystemScript.characters[1]; }
                for (int i = 0; i < LSystemScript.characters.Count; i++)
                {
                    if (LSystemScript.characters[i] == newText[0])
                    {
                        LSystemScript.characters[i] = LSystemScript.characters[1];
                        Characters[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + LSystemScript.characters[1];
                    }
                }
                LSystemScript.characters[1] = newText[0]; characters[1] = newText[0];
                textObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + newText;
            }
            else if (textObject == Characters[2])
            {
                for (int i = 0; i < characters.Count; i++) { if (characters[i] == newText[0]) characters[i] = LSystemScript.characters[2]; }
                for (int i = 0; i < LSystemScript.characters.Count; i++)
                {
                    if (LSystemScript.characters[i] == newText[0])
                    {
                        LSystemScript.characters[i] = LSystemScript.characters[2];
                        Characters[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + LSystemScript.characters[2];
                    }
                }
                LSystemScript.characters[2] = newText[0]; characters[2] = newText[0];
                textObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + newText;
            }
            else if (textObject == Characters[3])
            {
                for (int i = 0; i < characters.Count; i++) { if (characters[i] == newText[0]) characters[i] = LSystemScript.characters[3]; }
                for (int i = 0; i < LSystemScript.characters.Count; i++)
                {
                    if (LSystemScript.characters[i] == newText[0])
                    {
                        LSystemScript.characters[i] = LSystemScript.characters[3]; 
                        Characters[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + LSystemScript.characters[3];
                    }
                }
                LSystemScript.characters[3] = newText[0]; characters[3] = newText[0];
                textObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + newText;
            }
            //Adds the rules based on the updated text.
            if (textObject == Rules[0]) { LSystemScript.rules[0] = newText; textObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + newText; }
            else if (textObject == Rules[1]) { LSystemScript.rules[1] = newText; textObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + newText; }
            else if (textObject == Rules[2]) { LSystemScript.rules[2] = newText; textObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + newText; }
            else if (textObject == Rules[3]) { LSystemScript.rules[3] = newText; textObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "> " + newText; }
        }
        else textObject.GetComponent<TMP_InputField>().text = "";
        LSystemScript.CreateRules();
    }
    //Loads from the templates.
    public void AddTemplate() { templateLoop++; if (templateLoop > 7) templateLoop = 0; Template(); }
    public void Template(string newText)
    {
        if (newText != "" && !newText.Contains(">"))
        {
            if (int.Parse(newText) < 0) newText = 0.ToString(); else if (int.Parse(newText) > 7) newText = 7.ToString();
            templateLoop = int.Parse(newText);
            templateInputText.text = "";
            Template();
        }
    }
    public void Template()
    {
        //Resets and adds the Characters and Rules for each template.
        LSystemScript.characters = new List<char>(); LSystemScript.rules = new List<string>();
        if (templateLoop == 0)
        {
            characters[0] = 'F'; characters[1] = 'X'; characters[2] = '+'; characters[3] = '-';            
            LSystemScript.characters.Add(characters[0]);
            Characters[0].SetActive(true); Characters[1].SetActive(false); Characters[2].SetActive(false); Characters[3].SetActive(false);

            rules[0] = "F[+F]F[-F]F"; rules[1] = "FF"; rules[2] = "[+FX][FX][-FX]"; rules[3] = "XX";            
            LSystemScript.rules.Add(rules[0]);
            Rules[0].SetActive(true); Rules[1].SetActive(false); Rules[2].SetActive(false); Rules[3].SetActive(false);

            LSystemScript.angle = 25.7f;
            LSystemScript.lengthVariance = 0;
            LSystemScript.angleVariance = 0;
            LSystemScript.iteration = 5;
        }
        else if (templateLoop == 1)
        {
            characters[0] = 'F'; characters[1] = 'X'; characters[2] = '+'; characters[3] = '-';
            LSystemScript.characters.Add(characters[0]);
            Characters[0].SetActive(true); Characters[1].SetActive(false); Characters[2].SetActive(false); Characters[3].SetActive(false);

            rules[0] = "F[+F]F[-F][F]"; rules[1] = "FF"; rules[2] = "[+FX][FX][-FX]"; rules[3] = "XX";
            LSystemScript.rules.Add(rules[0]);
            Rules[0].SetActive(true); Rules[1].SetActive(false); Rules[2].SetActive(false); Rules[3].SetActive(false);

            LSystemScript.angle = 20f;
            LSystemScript.lengthVariance = 0;
            LSystemScript.angleVariance = 0;
            LSystemScript.iteration = 5;
        }
        else if (templateLoop == 2)
        {
            characters[0] = 'F'; characters[1] = 'X'; characters[2] = '+'; characters[3] = '-';
            LSystemScript.characters.Add(characters[0]); Characters[0].SetActive(true); Characters[1].SetActive(false); Characters[2].SetActive(false); Characters[3].SetActive(false);

            rules[0] = "FF-[-F+F+F]+[+F-F-F]"; rules[1] = "FF"; rules[2] = "[+FX][FX][-FX]"; rules[3] = "XX";
            LSystemScript.rules.Add(rules[0]);
            Rules[0].SetActive(true); Rules[1].SetActive(false); Rules[2].SetActive(false); Rules[3].SetActive(false);

            LSystemScript.angle = 22.5f;
            LSystemScript.lengthVariance = 0;
            LSystemScript.angleVariance = 0;
            LSystemScript.iteration = 4;
        }
        else if (templateLoop == 3)
        {
            characters[0] = 'X'; characters[1] = 'F'; characters[2] = '+'; characters[3] = '-';
            LSystemScript.characters.Add(characters[0]); LSystemScript.characters.Add(characters[1]);
            Characters[0].SetActive(true); Characters[1].SetActive(true); Characters[2].SetActive(false); Characters[3].SetActive(false);

            rules[0] = "F[+X]F[-X]+X"; rules[1] = "FF"; rules[2] = "[+FX][FX][-FX]"; rules[3] = "XX";
            LSystemScript.rules.Add(rules[0]); LSystemScript.rules.Add(rules[1]);
            Rules[0].SetActive(true); Rules[1].SetActive(true); Rules[2].SetActive(false); Rules[3].SetActive(false); 

            LSystemScript.angle = 20;
            LSystemScript.lengthVariance = 0;
            LSystemScript.angleVariance = 0;
            LSystemScript.iteration = 7;
        }
        else if (templateLoop == 4)
        {
            characters[0] = 'X'; characters[1] = 'F'; characters[2] = '+'; characters[3] = '-';
            LSystemScript.characters.Add(characters[0]); LSystemScript.characters.Add(characters[1]);
            Characters[0].SetActive(true); Characters[1].SetActive(true); Characters[2].SetActive(false); Characters[3].SetActive(false);

            rules[0] = "F[+X][-X]FX"; rules[1] = "FF"; rules[2] = "[+FX][FX][-FX]"; rules[3] = "XX";
            LSystemScript.rules.Add(rules[0]); LSystemScript.rules.Add(rules[1]);
            Rules[0].SetActive(true); Rules[1].SetActive(true); Rules[2].SetActive(false); Rules[3].SetActive(false);

            LSystemScript.angle = 25.7f;
            LSystemScript.lengthVariance = 0;
            LSystemScript.angleVariance = 0;
            LSystemScript.iteration = 7;
        }
        else if (templateLoop == 5)
        {
            characters[0] = 'X'; characters[1] = 'F'; characters[2] = '+'; characters[3] = '-';
            LSystemScript.characters.Add(characters[0]); LSystemScript.characters.Add(characters[1]);
            Characters[0].SetActive(true); Characters[1].SetActive(true); Characters[2].SetActive(false); Characters[3].SetActive(false);

            rules[0] = "F-[[X]+X]+F[+FX]-X"; rules[1] = "FF"; rules[2] = "[+FX][FX][-FX]"; rules[3] = "XX";
            LSystemScript.rules.Add(rules[0]); LSystemScript.rules.Add(rules[1]);
            Rules[0].SetActive(true); Rules[1].SetActive(true); Rules[2].SetActive(false); Rules[3].SetActive(false);

            LSystemScript.angle = 22.5f;
            LSystemScript.lengthVariance = 0;
            LSystemScript.angleVariance = 0;
            LSystemScript.iteration = 5;
        }
        else if (templateLoop == 6)
        {
            characters[0] = 'X'; characters[1] = 'F'; characters[2] = '+'; characters[3] = '-';
            LSystemScript.characters.Add(characters[0]); LSystemScript.characters.Add(characters[1]);
            Characters[0].SetActive(true); Characters[1].SetActive(true); Characters[2].SetActive(false); Characters[3].SetActive(false);

            rules[0] = "F+[[X]-X]-F[-FX]+X"; rules[1] = "FF"; rules[2] = "[+FX][FX][-FX]"; rules[3] = "XX";
            LSystemScript.rules.Add(rules[0]); LSystemScript.rules.Add(rules[1]);
            Rules[0].SetActive(true); Rules[1].SetActive(true); Rules[2].SetActive(false); Rules[3].SetActive(false);

            LSystemScript.angle = 25f;
            LSystemScript.lengthVariance = 0;
            LSystemScript.angleVariance = 0;
            LSystemScript.iteration = 6;
        }
        else if (templateLoop == 7)
        {
            characters[0] = 'X'; characters[1] = 'F'; characters[2] = '+'; characters[3] = '-';
            LSystemScript.characters.Add(characters[0]); LSystemScript.characters.Add(characters[1]);
            Characters[0].SetActive(true); Characters[1].SetActive(true); Characters[2].SetActive(false); Characters[3].SetActive(false);

            rules[0] = "[FX[+F[-FX]FX][-F-FXFX]]"; rules[1] = "FF"; rules[2] = "[+FX][FX][-FX]"; rules[3] = "XX";
            LSystemScript.rules.Add(rules[0]); LSystemScript.rules.Add(rules[1]);
            Rules[0].SetActive(true); Rules[1].SetActive(true); Rules[2].SetActive(false); Rules[3].SetActive(false);

            LSystemScript.angle = 30f;
            LSystemScript.lengthVariance = 0f;
            LSystemScript.angleVariance = 0f;
            LSystemScript.iteration = 5;
        }
        UpdateText();
    }
    public void UpdateText()
    {
        Characters[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> " + characters[0]; Characters[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> " + characters[1];
        Characters[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> " + characters[2]; Characters[3].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> " + characters[3];

        Rules[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> " + rules[0]; Rules[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> " + rules[1];
        Rules[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> " + rules[2]; Rules[3].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "> " + rules[3];

        if (LSystemScript.angle % 1 == 0) angleText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Degrees\\Angle\\Value        : " + LSystemScript.angle.ToString() + ".0°";
        else angleText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Degrees\\Angle\\Value        : " + LSystemScript.angle.ToString() + "°";

        if (LSystemScript.lengthVariance == 0 || LSystemScript.lengthVariance == 1)
            lengthVarianceText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Float\\LengthVariance\\Value : " + LSystemScript.lengthVariance.ToString() + ".0f";
        else lengthVarianceText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Float\\LengthVariance\\Value : " + LSystemScript.lengthVariance.ToString() + "f";

        if (LSystemScript.angleVariance == 0 || LSystemScript.angleVariance == 1)
            angleVarianceText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Float\\AngleVariance\\Value  : " + LSystemScript.angleVariance.ToString() + ".0f";
        else angleVarianceText.transform.parent.GetComponent<TextMeshProUGUI>().text = "> D:\\Float\\AngleVariance\\Value  : " + LSystemScript.angleVariance.ToString() + "f";

        templateText.text = "> D:\\Template\\Number\\Input.GetKey('t'): " + templateLoop + ".0f";
        LSystemScript.CreateRules();
    }
    //Conditions to reset the iterations or scene.
    public void Reset() { LSystemScript.iteration = 0; LSystemScript.CreateRules(); }
    public void HardReset() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
}