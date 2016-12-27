//Controller Class - Generate population data and determine year with highests population. cameronstiffler@gmail.com 2016
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
	//Data
	private int earliestBirthYear = 1850;
	private int latestBirthYear = 1990;
	private int startYear = 1900;
	private int endYear = 2000;
	public int numberOfPeople = 1000;
	public int maxLifeSpan = 100;
	public Person[] people; //store the randomly generated data set of people
	public Year[] years; //store our yearly populations for time period
	private Person currentPerson;
	private Year currentYear;
	private int currentGraphYear = 0;
	int highestPopulation = 0;
	int lowestPopulation = 100000000;

	//Movement
	public Transform startMarker;
	public Transform endMarker;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	public GameObject peakMarker;
	public GameObject drawMarker;
	public TextMesh yearMarkerText;
	public TextMesh peakText;
	public Camera followCamera;
	public Camera graphCamera;
	private float scaleTransform;
	private float horizontalScale = 2;

	// Initialization
	void Start () {
		//Data
		people = new Person[numberOfPeople];
		years = new Year[endYear-startYear+1];
		GeneratePeople ();
		Debug.Log("highest population year: "+findHighestPopulationYear());

		//Graph
		scaleTransform = ((float) lowestPopulation) / ((float) highestPopulation);
		startMarker.position = new Vector3(currentGraphYear, ((float)years[currentGraphYear].population/(float)lowestPopulation)*50f, 0);
		endMarker.position = new Vector3(currentGraphYear+1, ((float)years[currentGraphYear+1].population/(float)lowestPopulation)*50f, 0);
		startTime = Time.time;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

	}

	// Update is called once per frame
	void Update () {
		//Graph
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

		if (transform.position == endMarker.position && currentGraphYear < years.Length - 1) {
			currentGraphYear++;
			startTime = Time.time;
			journeyLength = Vector3.Distance (startMarker.position, endMarker.position);
			startMarker.position = endMarker.position;
			endMarker.position = new Vector3 ((float)currentGraphYear * horizontalScale, ((float)years [currentGraphYear].population / (float)lowestPopulation) * 50f, 0);
			yearMarkerText.text = years [currentGraphYear].population.ToString () + " people in " + years [currentGraphYear].name.ToString ();
			if (years [currentGraphYear].isMostPopulousYear) {
				peakMarker.transform.position = new Vector3 ((float)currentGraphYear * horizontalScale, ((float)years [currentGraphYear].population / (float)lowestPopulation) * 50f + 3.5f, 0);
				peakText.text = "population peaks in " + years [currentGraphYear].name.ToString () + " with " + years [currentGraphYear].population.ToString () + " people";
				followCamera.enabled = false;
				graphCamera.enabled = true;
				yearMarkerText.characterSize = 3;
			}
		} else if (currentGraphYear > 99 ){
			drawMarker.transform.position = new Vector3 (0, 0, -900);
		}
	}


	//Data


	//Generate Our Test Data Randomly
	void GeneratePeople () {
		//Debug.Log("Generating people");
		for (int a = 0; a < numberOfPeople; a++)
		{
			int yoBirth = Random.Range(earliestBirthYear, latestBirthYear);
			int yoDeath = yoBirth + Random.Range(0, maxLifeSpan);
			string name = "person" + a;
			//Debug.Log(" attempting to create person"+a+" "+yoBirth+"-"+yoDeath);
			currentPerson = new Person(name, yoBirth, yoDeath);
			people[a] = currentPerson;
		}
	}

	//Determine and return the year with the highest population
	int findHighestPopulationYear() {
		//Debug.Log("Finding Peek Population Year");
		int highestPopulationYear = 0;
		int population;
		for (int y = startYear; y <= endYear; y++) {
			//Debug.Log("Checking Year: " + y);
			population = 0;
			for (int p = 0; p < numberOfPeople; p++) {
				//Debug.Log("Checking person: " + people [p].name+" with birth year "+people [p].birth+" and death year "+people [p].death);
				if (people [p].birth <= y && people [p].death >= y) {
					population++;
				}
				if (population > highestPopulation) {
					//Debug.Log("New Highest Pop Year:"+y);
					highestPopulation = population;
					highestPopulationYear = y;
				}
			}
			if (population < lowestPopulation) {
				//Debug.Log("New Lowest Pop Year:"+y);
				lowestPopulation = population;
			}
			//Add year with population to years array so we have a couple ways of getting at our data.
			currentYear = new Year(y, population);
			years[y-startYear] = currentYear;
		}

		years [highestPopulationYear - startYear].isMostPopulousYear = true;//Most populous year knows it is

		return highestPopulationYear;
	}
}

public class Person {
	public int birth;
	public int death;
	public string name;
	public Person(string iname, int ibirth, int ideath)
	{
		name = iname;
		birth = ibirth;
		death = ideath;
		Debug.Log("created "+name+" with life span "+birth+"-"+death);
	}
}

public class Year {
	public int population;
	public int name;
	public bool isMostPopulousYear = false;
	public Year(int iname, int ipopulation)
	{
		name = iname;
		population = ipopulation;
		//Debug.Log("year "+name+" population is "+population);
	}
}