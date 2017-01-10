/*
 * CIS 602-02 GAMIFICATION IN BIO-INFOMATICS
 * MEDICAL SIMULATOR v0.1 (Development Build)
 * @author:
 * Vishnu Vardhan Kumar Pallati (01468680)
 * Sumukhi Kappa (01464824)
 * Harini Pughazendi (01505097
 * Shree Lekha Kakkerla (01510251)
 * Abhilash Vinod Kumar (01532780)
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class pinchZoom : MonoBehaviour 
{
	#region declationg variables
	public float cameraSpeed = 0.00001f;
	public GameObject BackButton;
	public GameObject medicinebutton;
	public GameObject medicineBoxPanelObject;
	public GameObject patientInfoPadObject;
	public Text patientInfoPadTextObject;
	public Text statsText;
	public GameObject mainMenu;

	private bool isZoomedIn = false;
	private Vector2 screenSize;
	private Vector3 followPoint;
	private Vector3 followBackPoint;
	private bool isZoomRequested = false;
	private bool isZoomOutRequested = false;
	private int selectedPatient;
	private int noOfDiseases;


	//development purpose only
	public Slider healthSlider;
	public Slider medicineSlider;
	public bool isGameStarted;

	//patients
	private patient[] patients;
	//scripts used to animate status bars
	public healthBarUpdate[] healthBarUpdateScripts;
	public healthBarUpdate[] medicineBarUpdateScripts;
	public GameObject[] patientBedSprite;
	public GameObject[] heartRateMoniterSprite;
	public GameObject[] RipSprite;

	//audio variable declarations
	public AudioSource audioSource;
	public AudioClip CorrectMedicineSound;
	public AudioClip wrongMedicineSound;
	public AudioClip zoomSound;

	#endregion

	void Start()
	{
		isGameStarted = false;
		noOfDiseases = 3;
		PatientStart ();
	}
	
	void Update()
	{
		if (isGameStarted) {

						PatientUpdate ();

						zoomInZoomOutUpdate ();
				}
	}

	public void StartGame()
	{
		isGameStarted = true;

		PatientStart ();
	}


	#region camera zoomIn out code
	//used by back button to zoom out camera
	public void moveCameraBack()
	{
		isZoomRequested = false;
		isZoomedIn = false;
		isZoomOutRequested = true;
		
		followBackPoint = -(Camera.main.gameObject.transform.position);

		//play zoom sound
		audioSource.PlayOneShot (zoomSound, 1.0f);
	}

	  
	private void zoomInZoomOutUpdate()
	{

		//detect if user tapped on patient to zoom in
		if (!isZoomRequested) //if used did not tap on any patient
		{
			if (Input.touchCount > 0) //if user taps detect where he tapped
			{
				//get the touch point
				Vector2 touchZero = Input.GetTouch(0).position;
				
				//detect weather he tapped on the patient
				if((touchZero.y < (800 * 0.85f)) && (touchZero.y > (800 * 0.725f))) // present on top half
				{ 
					//saperate by patient and set the follow point
					if ( touchZero.x > 1024) // user tapped on patient 5
					{
						followPoint = new Vector3(19, 10, 0);
						patientInfoPadTextObject.text = "Patient 5 Disease " + patients[4].diseaseNo.ToString();
						selectedPatient = 4;
						
					}
					else if (touchZero.x > 768)// user tapped on patient 4
					{
						followPoint = new Vector3(10, 10, 0);
						patientInfoPadTextObject.text = "Patient 4 Disease " + (patients[3].diseaseNo ).ToString();
						selectedPatient = 3;
					}
					else if (touchZero.x > 512) // user tapped on patient 3
					{
						followPoint = new Vector3(0, 10, 0);
						patientInfoPadTextObject.text = "Patient 3 Disease " + (patients[2].diseaseNo ).ToString();
						selectedPatient = 2;
					}
					else if (touchZero.x > 256) // user tapped on patient 2
					{
						followPoint = new Vector3(-9, 10, 0);
						patientInfoPadTextObject.text = "Patient 2 Disease " + (patients[1].diseaseNo ).ToString();
						selectedPatient = 1;
					}
					else // user tapped on patient 1
					{
						followPoint = new Vector3(-19, 10, 0);
						patientInfoPadTextObject.text = "Patient 1 Disease " + (patients[0].diseaseNo).ToString();
						selectedPatient = 0;
					}
					
					isZoomRequested = true;
					//display buttons on patient screen
					medicinebutton.gameObject.SetActive(true);
					BackButton.gameObject.SetActive(true);
					patientInfoPadObject.gameObject.SetActive(true);

					//play zoom audio
					audioSource.PlayOneShot (zoomSound, 1.0f);
					
					
				}
				else if ((touchZero.y < (800 * 0.425f)) && (touchZero.y > (800 * 0.3f))) // present in lower half
				{
					
					//saperate by patient and set the follow point
					if ( touchZero.x > 1024) // user tapped on patient 10
					{
						followPoint = new Vector3(19, -5, 0);
						patientInfoPadTextObject.text = "Patient 10 Disease " + patients[9].diseaseNo.ToString();
						selectedPatient = 9;
					}
					else if (touchZero.x > 768)// user tapped on patient 9
					{
						followPoint = new Vector3(10, -5, -0);
						patientInfoPadTextObject.text = "Patient 9 Disease " + patients[8].diseaseNo.ToString();
						selectedPatient = 8;
					}
					else if (touchZero.x > 512) // user tapped on patient 8
					{
						followPoint = new Vector3(0, -5, 0);
						patientInfoPadTextObject.text = "Patient 8 Disease " + patients[7].diseaseNo.ToString();
						selectedPatient = 7;
					}
					else if (touchZero.x > 256) // user tapped on patient 7
					{
						followPoint = new Vector3(-9, -5, 0);
						patientInfoPadTextObject.text = "Patient 7 Disease " + patients[6].diseaseNo.ToString();
						selectedPatient = 6;
					}
					else // user tapped on patient 6
					{
						followPoint = new Vector3(-19, -5, 0);
						patientInfoPadTextObject.text = "Patient 6 Disease " + patients[5].diseaseNo.ToString();
						selectedPatient = 5;
					}
					isZoomRequested = true;
					//display buttons on patient screen
					medicinebutton.gameObject.SetActive(true);
					BackButton.gameObject.SetActive(true);
					patientInfoPadObject.gameObject.SetActive(true);
					
					audioSource.PlayOneShot (zoomSound, 1.0f);
				}
				
			}
		}
		
		 
		// zoom in to the selected patient
		if ((!isZoomedIn) && (isZoomRequested))
		{
			if ((followPoint.x == 0.0f) && (followPoint.y == 0.0f))
				isZoomedIn = true;
			
			Vector3 currentCameraPositon = Camera.main.gameObject.transform.position;
			if ( ( (followPoint.x > 0.0f) && (currentCameraPositon.x > followPoint.x) ) || ( (followPoint.x < 0.0f) && (currentCameraPositon.x < followPoint.x) ))
				followPoint.x = 0.0f;
			if ((currentCameraPositon.y < -5 ) || (currentCameraPositon.y > 10))
				followPoint.y = 0.0f;
			
			
			
			
			Camera.main.transform.Translate(new Vector3(followPoint.x * 3 * Time.deltaTime, followPoint.y * 3 * Time.deltaTime, 0.0f));
			if (GetComponent<Camera>().orthographicSize > 3.0f)
				GetComponent<Camera>().orthographicSize -= 0.6f;
			
		}

		//device back button event
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			//play zoom sound
			audioSource.PlayOneShot (zoomSound, 1.0f);

			if (isZoomRequested)
			{
				isZoomRequested = false;
				isZoomedIn = false;
				isZoomOutRequested = true;
				
				followBackPoint = -(Camera.main.gameObject.transform.position);
				
				//hide buttons on patient screen
				medicinebutton.gameObject.SetActive(false);
				BackButton.gameObject.SetActive(false);
				medicineBoxPanelObject.gameObject.SetActive(false);
				patientInfoPadObject.gameObject.SetActive(false);
				
				
			}
			else
			{
				Application.Quit(); 
			}
		}
		
		//zoom back to main screen
		if ((isZoomOutRequested)) 
		{
			
			if ((followBackPoint.x == 0.0f) && (followBackPoint.y == 0.0f))
				isZoomOutRequested = false;
			
			Vector3 currentCameraPositon = Camera.main.gameObject.transform.position;
			if ( ( (followBackPoint.x > 0.0f) && (currentCameraPositon.x > 0 ) ) || ( (followBackPoint.x < 0.0f) && (currentCameraPositon.x < 0) ))
				followBackPoint.x = 0.0f;
			if  (( (followBackPoint.y > 0.0f) && (currentCameraPositon.y > 0 ) ) || ( (followBackPoint.y < 0.0f) && (currentCameraPositon.y < 0) ))
				followBackPoint.y = 0.0f;
			
			Camera.main.transform.Translate(new Vector3(followBackPoint.x * 3 * Time.deltaTime, followBackPoint.y * 3 * Time.deltaTime, 0.0f));
			if (GetComponent<Camera>().orthographicSize < 15.0f)
				GetComponent<Camera>().orthographicSize += 0.6f;
			
		}

	}



    public void clickEvent(int x, int y)
    {


        if (!isZoomedIn)
        {
            //detect weather he tapped on the patient
            if ((y < (545)) && (y > (435))) // present on top half
            {
                //saperate by patient and set the follow point
                if (x > 770) // user tapped on patient 5
                {
                    followPoint = new Vector3(19, 10, 0);
                    patientInfoPadTextObject.text = "Patient 5 Disease " + patients[4].diseaseNo.ToString();
                    selectedPatient = 4;

                }
                else if (x > 576)// user tapped on patient 4
                {
                    followPoint = new Vector3(10, 10, 0);
                    patientInfoPadTextObject.text = "Patient 4 Disease " + (patients[3].diseaseNo).ToString();
                    selectedPatient = 3;
                }
                else if (x > 384) // user tapped on patient 3
                {
                    followPoint = new Vector3(0, 10, 0);
                    patientInfoPadTextObject.text = "Patient 3 Disease " + (patients[2].diseaseNo).ToString();
                    selectedPatient = 2;
                }
                else if (x > 192) // user tapped on patient 2
                {
                    followPoint = new Vector3(-9, 10, 0);
                    patientInfoPadTextObject.text = "Patient 2 Disease " + (patients[1].diseaseNo).ToString();
                    selectedPatient = 1;
                }
                else // user tapped on patient 1
                {
                    followPoint = new Vector3(-19, 10, 0);
                    patientInfoPadTextObject.text = "Patient 1 Disease " + (patients[0].diseaseNo).ToString();
                    selectedPatient = 0;
                }

                isZoomRequested = true;
                //display buttons on patient screen
                medicinebutton.gameObject.SetActive(true);
                BackButton.gameObject.SetActive(true);
                patientInfoPadObject.gameObject.SetActive(true);

                //play zoom audio
                audioSource.PlayOneShot(zoomSound, 1.0f);


            }
            else if ((y < (600 * 0.425f)) && (y > (600 * 0.3f))) // present in lower half
            {

                //saperate by patient and set the follow point
                if (x > 770) // user tapped on patient 10
                {
                    followPoint = new Vector3(19, -5, 0);
                    patientInfoPadTextObject.text = "Patient 10 Disease " + patients[9].diseaseNo.ToString();
                    selectedPatient = 9;
                }
                else if (x > 576)// user tapped on patient 9
                {
                    followPoint = new Vector3(10, -5, -0);
                    patientInfoPadTextObject.text = "Patient 9 Disease " + patients[8].diseaseNo.ToString();
                    selectedPatient = 8;
                }
                else if (x > 384) // user tapped on patient 8
                {
                    followPoint = new Vector3(0, -5, 0);
                    patientInfoPadTextObject.text = "Patient 8 Disease " + patients[7].diseaseNo.ToString();
                    selectedPatient = 7;
                }
                else if (x > 192) // user tapped on patient 7
                {
                    followPoint = new Vector3(-9, -5, 0);
                    patientInfoPadTextObject.text = "Patient 7 Disease " + patients[6].diseaseNo.ToString();
                    selectedPatient = 6;
                }
                else // user tapped on patient 6
                {
                    followPoint = new Vector3(-19, -5, 0);
                    patientInfoPadTextObject.text = "Patient 6 Disease " + patients[5].diseaseNo.ToString();
                    selectedPatient = 5;
                }
                isZoomRequested = true;
                //display buttons on patient screen
                medicinebutton.gameObject.SetActive(true);
                BackButton.gameObject.SetActive(true);
                patientInfoPadObject.gameObject.SetActive(true);

                audioSource.PlayOneShot(zoomSound, 1.0f);
            }



        }





        // zoom in to the selected patient
        if ((!isZoomedIn) && (isZoomRequested))
        {
            if ((followPoint.x == 0.0f) && (followPoint.y == 0.0f))
                isZoomedIn = true;

            Vector3 currentCameraPositon = Camera.main.gameObject.transform.position;
            if (((followPoint.x > 0.0f) && (currentCameraPositon.x > followPoint.x)) || ((followPoint.x < 0.0f) && (currentCameraPositon.x < followPoint.x)))
                followPoint.x = 0.0f;
            if ((currentCameraPositon.y < -5) || (currentCameraPositon.y > 10))
                followPoint.y = 0.0f;




            Camera.main.transform.Translate(new Vector3(followPoint.x * 3 * Time.deltaTime, followPoint.y * 3 * Time.deltaTime, 0.0f));
            if (GetComponent<Camera>().orthographicSize > 3.0f)
                GetComponent<Camera>().orthographicSize -= 0.6f;

        }

        //device back button event
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //play zoom sound
            audioSource.PlayOneShot(zoomSound, 1.0f);

            if (isZoomRequested)
            {
                isZoomRequested = false;
                isZoomedIn = false;
                isZoomOutRequested = true;

                followBackPoint = -(Camera.main.gameObject.transform.position);

                //hide buttons on patient screen
                medicinebutton.gameObject.SetActive(false);
                BackButton.gameObject.SetActive(false);
                medicineBoxPanelObject.gameObject.SetActive(false);
                patientInfoPadObject.gameObject.SetActive(false);


            }
            else
            {
                Application.Quit();
            }
        }

        //zoom back to main screen
        if ((isZoomOutRequested))
        {

            if ((followBackPoint.x == 0.0f) && (followBackPoint.y == 0.0f))
                isZoomOutRequested = false;

            Vector3 currentCameraPositon = Camera.main.gameObject.transform.position;
            if (((followBackPoint.x > 0.0f) && (currentCameraPositon.x > 0)) || ((followBackPoint.x < 0.0f) && (currentCameraPositon.x < 0)))
                followBackPoint.x = 0.0f;
            if (((followBackPoint.y > 0.0f) && (currentCameraPositon.y > 0)) || ((followBackPoint.y < 0.0f) && (currentCameraPositon.y < 0)))
                followBackPoint.y = 0.0f;

            Camera.main.transform.Translate(new Vector3(followBackPoint.x * 3 * Time.deltaTime, followBackPoint.y * 3 * Time.deltaTime, 0.0f));
            if (GetComponent<Camera>().orthographicSize < 15.0f)
                GetComponent<Camera>().orthographicSize += 0.6f;

        }

    }


    #endregion



    #region patient class

    //code for game logic goes here
    public class patient
	{
		public string Name;
		public int diseaseNo;//will be related to medicine no to interact
		public string DiseaseName;
		public float healthValueState;
		public float medicineDoseRate;
		public bool isCured;
		public bool isDead;
		public float healthSpeed, medicineSpeed;

		private bool isCorrectTreatementGiven;
		
		//patient construction
		public patient(int DiseaseNo, float HealthValueState, float MedicineDoseRate)
		{
			diseaseNo = DiseaseNo;
			healthValueState = HealthValueState;
			medicineDoseRate = MedicineDoseRate;

			isDead = false;
			isCured = false;
			isCorrectTreatementGiven = false;
		}
		
		public bool giveMedicine (int medGivenForDiseaseNo)
		{
			if (medGivenForDiseaseNo == diseaseNo) 
			{ //correct medicine is given
				isCorrectTreatementGiven = true;
				medicineDoseRate = 100;
				
				return true;
			}
			else 
			{
				//decrease health for giving wrong medicine
				healthValueState -= 10;
				return false;
			}
			
		}
		
		public void patientUpdate()
		{

						

						//if correct medicine given increase health and decrease medicine rate
						if (isCorrectTreatementGiven) {
								//decrease medicine dose rate
								medicineDoseRate -= Time.deltaTime * medicineSpeed;
				
								//increase health
				healthValueState += Time.deltaTime * healthSpeed;
						} else {
								//decrease health
				healthValueState -= Time.deltaTime * healthSpeed;

								//medicineDoseRate -= Time.deltaTime;
						}
			
						if ((medicineDoseRate < 0)) {
								isCorrectTreatementGiven = false;
								medicineDoseRate = 0;
						}
				}
	}
	
	//function called by medicine buttons
	public void giveMedicineToSelectedPatient (int medicineNo)
	{
		if (patients [selectedPatient].giveMedicine (medicineNo)) {
						//if correct medicine moveCameraBack
						isZoomRequested = false;
						isZoomedIn = false;
						isZoomOutRequested = true;
			
						followBackPoint = -(Camera.main.gameObject.transform.position);
			
						//hide buttons on patient screen
						medicinebutton.gameObject.SetActive (false);
						BackButton.gameObject.SetActive (false);
						medicineBoxPanelObject.gameObject.SetActive (false);
						patientInfoPadObject.gameObject.SetActive (false);

						//play correct medicine sound
						audioSource.PlayOneShot (CorrectMedicineSound, 1.0f);
				} else {
						//wrong medicine is given , play aww music
						audioSource.PlayOneShot (wrongMedicineSound, 1.0f);
				}
	}

	//for testing purpose only
	public void changeGameSpeed()
	{
		for (int i = 0; i < 10; i++) {
			patients[i].healthSpeed = healthSlider.value;
			patients[i].medicineSpeed = medicineSlider.value;
			
		}
		
	}

	public void changeDifficulty(int difficultyLevel)
	{
		//difficulty level easy
		if (difficultyLevel == 1) {
						for (int i = 0; i < 10; i++) {
								patients [i].healthSpeed = 0.9f;
								patients [i].medicineSpeed = 1.0f;
				
						}
				} else if (difficultyLevel == 2) {//difficulty level medium
						for (int i = 0; i < 10; i++) {
								patients [i].healthSpeed = 0.7f;
								patients [i].medicineSpeed = 1.4f;
				
						}
				} else if (difficultyLevel == 3) {//difficulty level hard
						for (int i = 0; i < 10; i++) {
								patients [i].healthSpeed = 0.5f;
								patients [i].medicineSpeed = 2.0f;
				
						}
				}
	}
	
	public void PatientUpdate()
	{

		int PatientCount = 0;
		//updating the patient stats and updating the bars;
		for (int i=0; i<10; i++) {
			//patient is not cured and not dead then go ahead
			if ((!patients [i].isCured) && (!patients[i].isDead)) {
				
				
				//if patient's health is grater than 100, change his status
				if (patients[i].healthValueState > 100.0f) {
					//is just now cured
					patients[i].isCured = true;
					
					patientBedSprite[i].SetActive(false);
					RipSprite[i].SetActive(false);
				}
				
				//if patient health is less than 0, kill him
				if (patients[i].healthValueState < 0.1f)
				{
					patients[i].isDead = true;
					heartRateMoniterSprite[i].SetActive(false);

					patientBedSprite[i].SetActive(false);
				}
				
				patients [i].patientUpdate ();
				healthBarUpdateScripts [i].updateHealth (patients [i].healthValueState);
				medicineBarUpdateScripts [i].updateHealth (patients [i].medicineDoseRate);
			} else {// Patient is cured
				PatientCount++;
				
			}

			if (PatientCount == 10)//game over
			{
				isGameStarted = false;
				//Calculate the stats
				int curedPatients = 0;
				for (int j = 0; j < 10; j++) {
					if (patients[j].isCured)
						curedPatients++;
				}
				// update the stats Text
				statsText.text = "Patients Cured = : " + curedPatients.ToString() + "\nPatients Dead = " + (10 - curedPatients).ToString();
				//show the  Main Menu with Stats Screen
				mainMenu.SetActive(true);

			}
		}
	}

	public void changeNoOfDiseasesTo(int newNoOfDiseases)
	{
		noOfDiseases = newNoOfDiseases;
	}

	public void PatientStart()
	{
		selectedPatient = 0;

		//create patients
		patients = new patient[10];
		for (int i=0; i<10; i++) {
			//initiating the patients
			patients [i] = new patient (Random.Range (1, noOfDiseases+1), Random.Range (20.0f, 70.0f), 0.0f);
			patientBedSprite[i].SetActive(true);
			RipSprite[i].SetActive(true);
			heartRateMoniterSprite[i].SetActive(true);
			
		}
		
		changeGameSpeed ();
	}

	#endregion
}
