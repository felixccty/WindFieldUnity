using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 输入配置参数控制：ScrollBar+InputField
/// </summary>
public class winderDirSiderCtr: MonoBehaviour
{
    [SerializeField]
    private Scrollbar _scrollBar;
    [SerializeField]
    private InputField _inputField;

    private float _cur;
    public float Value
    {
        get { return _cur; }
    }

    private float Min { get; set; }
    private float Max { get; set; }
    public int DecimalPlaceCnt { get; set; }

    
    // 测试
    private void Start() 
    {
        Init(1, 10, 3, 0);
    }
   

    private void OnDestroy()
    {
        Destroy();
    }

    public void Init(float min, float max, float cur, int decimalPlaceCnt = 2)
    {
        Min = min;
        Max = max;
        DecimalPlaceCnt = decimalPlaceCnt;
        SetValue(cur);

        _scrollBar.onValueChanged.AddListener(OnScrollBarValueChanged);
        _inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        _inputField.contentType = InputField.ContentType.DecimalNumber;
    }

    public void Destroy()
    {
        _scrollBar.onValueChanged.RemoveListener(OnScrollBarValueChanged);
        _inputField.onEndEdit.RemoveListener(OnInputFieldEndEdit);
    }

    private void OnScrollBarValueChanged(float value)
    {
        SetValue(Min + (Max - Min) * value);
    }
    private void OnInputFieldEndEdit(string value)
    {
        if (float.TryParse(value, out float v))
        {
            SetValue(v);
        }
    }

    public void SetValue(float value)
    {
        int factor = (int)(Mathf.Pow(10, DecimalPlaceCnt));
        value = (int)(value * factor) / (float)factor;
        _cur = Mathf.Clamp(value, Min, Max);

        _scrollBar.SetValueWithoutNotify((Value - Min) / (Max - Min));
        _inputField.SetTextWithoutNotify(Value.ToString());
    }
}