using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderTextUpdateScript : MonoBehaviour {

	public Slider slider;
	public Text sliderTextBox;
	public string baseName;


	public void updateSliderText()
	{
		sliderTextBox.text = baseName + slider.value.ToString();
	}
}
