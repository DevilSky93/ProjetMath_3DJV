using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DropDownHandler : MonoBehaviour
    {
        public Text textBox;
        

        private void Start()
        {
            var dropdown = GetComponent<Dropdown>();

            dropdown.options.Clear();
            List<string> items = new List<string>
            {
                "Graham Scan",
                "Jarvis"
            };

            foreach (var item in items)
            {
                dropdown.options.Add(new Dropdown.OptionData() { text = item });
            }

            DropdownItemSelected(dropdown);
            
            dropdown.onValueChanged.AddListener(delegate
            {
                {
                    DropdownItemSelected(dropdown);
                }
            });
        }

        private void DropdownItemSelected(Dropdown dropdown)
        {
            int index = dropdown.value;
            PointsManager.Choice = (PointsManager.AlgoChoice)index;
            textBox.text = dropdown.options[index].text;
        }

        public void ExecuteAlgo()
        {
            PointsManager.ExecuteAlgorithm();
        }
    }
}