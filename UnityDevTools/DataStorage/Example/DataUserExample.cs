using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataUserExample : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField]
    private DataService _dataService;

    [SerializeField]
    private string[] _possibleStrings;

    [SerializeField]
    private ConcreateMono[] _possibleSkins;
    [SerializeField]
    private Button _changeSkinButton;
    [SerializeField]
    private Text _skinText;

    [SerializeField]
    private Button _changeCtorBtn;
    [SerializeField]
    private Text _ctorDataText;

    [SerializeField]
    private Button _changeCtorBtn1;
    [SerializeField]
    private Text _ctorDataText1;

#pragma warning restore

    private void Start()
    {
        if (_dataService.ExampleData.SomeComponents == null || _dataService.ExampleData.SomeComponents.Count != 3)
        {
            _dataService.ExampleData.SomeComponents = new List<IItem>(2);
            _dataService.ExampleData.SomeComponents.Add(_possibleSkins[0]);
            _dataService.ExampleData.SomeComponents.Add(new ConcreateSimple());
            _dataService.ExampleData.SomeComponents.Add(new ConcreateSimple1());
        }

        _skinText.text = _dataService.ExampleData.SomeComponents[0].ToString();
        _ctorDataText.text = _dataService.ExampleData.SomeComponents[1].ToString();
        _ctorDataText1.text = _dataService.ExampleData.SomeComponents[2].ToString();

        _changeSkinButton.onClick.AddListener(() =>
        {
            var itemIndex = 0;
            for (int i = 0; i < _possibleSkins.Length; i++)
            {
                if (_dataService.ExampleData.SomeComponents[0] == (IItem)_possibleSkins[i])
                {
                    itemIndex = i;
                    break;
                }
            }

            itemIndex++;
            itemIndex %= _possibleSkins.Length;
            _dataService.ExampleData.SomeComponents[0] = _possibleSkins[itemIndex];
            _skinText.text = _dataService.ExampleData.SomeComponents[0].ToString();
        });

        _changeCtorBtn.onClick.AddListener(() =>
        {
            var newData = new ConcreateSimple();
            newData.Id = Random.Range(int.MinValue, int.MaxValue).ToString();
            newData.OwnValue = Random.Range(int.MinValue, int.MaxValue);
            newData.Value = Random.Range(int.MinValue, int.MaxValue);

            _dataService.ExampleData.SomeComponents[1] = newData;
            _ctorDataText.text = _dataService.ExampleData.SomeComponents[1].ToString();
        });

        _changeCtorBtn1.onClick.AddListener(() =>
        {
            var newData = new ConcreateSimple1();
            newData.Id = Random.Range(int.MinValue, int.MaxValue).ToString();
            newData.OwnValue = Random.Range(int.MinValue, int.MaxValue);
            newData.OwnValueRatatatam = Random.Range(int.MinValue, int.MaxValue);
            newData.Value = Random.Range(int.MinValue, int.MaxValue);

            _dataService.ExampleData.SomeComponents[2] = newData;
            _ctorDataText1.text = _dataService.ExampleData.SomeComponents[2].ToString();
        });
    }
}
