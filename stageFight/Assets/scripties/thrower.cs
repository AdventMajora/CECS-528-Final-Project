using UnityEngine;
using System.Collections;

public class thrower : MonoBehaviour {

	public GameObject[] items;                // Objects to be thrown
	public GameObject[] players;
	public GameObject[] platforms;
    public float spawnTime = 3f;            // How long between each spawn.
    public float destroyTime = 5f;
    public float rotationSpeed = 1000f;
    public bool spawning = true;
    public float stageSizeX = 50f;
    public float stageSizeZ = 50f;

    // a, b, and c for ellipsoid equation
    public float a = 1;
    public float b = 1;
    public float c = 1;

    private float camWidth;
    private float camHeight;

	// Use this for initialization
	void Start () {
		beginSpawning ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void Spawn()
	{
		if(!spawning)
		{
			return;
		}

		// Select random item and spawn it relative to parent
		int itemIndex = Random.Range (0, items.Length);
		GameObject newObject = (GameObject)Instantiate (items [itemIndex], transform.localPosition + getRandomPosition(), Quaternion.identity);
		newObject.transform.parent = transform;

		setVelocity (newObject);

		// Setting velocity
		//newObject.GetComponent<Rigidbody> ().velocity = transform.localPosition - newObject.transform.position;

		Destroy (newObject, destroyTime);
	}

	private void setVelocity (GameObject thrownObject)
	{
		// Decide between 4 options
		//	1. Throw at Player A (red)
		//	2. Throw at Player B (blue)
		//	3. Throw at random position on Red stage
		//	4. Throw at random position on Blue Stage

		int decision = Random.Range (1, 5);
		Vector3 origin = thrownObject.transform.position;
		Vector3 destination;
		Vector3 velocity;
		Vector3 shift;

		switch (decision){
		case 1:
			// Throw at red player
			destination = players [0].transform.position;
			break;
		case 2:
			// Throw at blue player
			destination = players [1].transform.position;
			break;
		case 3:
			// THrow at red stage
			shift = new Vector3 (Random.Range (-stageSizeX, stageSizeX), 0, Random.Range (-stageSizeZ, stageSizeZ));
			destination = platforms [0].transform.position + shift;
			break;
		case 4:
			// Throw at blue stage
			shift = new Vector3 (Random.Range (-stageSizeX, stageSizeX), 0, Random.Range (-stageSizeZ, stageSizeZ));
			destination = platforms [1].transform.position + shift;
			break;
		default:
			destination = new Vector3 (0, 0, 0);
			break;
		}

		velocity = getThrowVector (origin, destination, 60f);

		// Setting spin
		thrownObject.GetComponent<Rigidbody> ().angularVelocity = rotationSpeed * new Vector3 (Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f));
		thrownObject.GetComponent<Rigidbody> ().velocity = velocity;
	}

	private Vector3 getThrowVector(Vector3 origin, Vector3 destination, float angleDeg) 
	{
		

		// If below stage, throw at steeper angle
		if(origin.y < 0) {
			Vector3 dir = destination - origin;
			float height = dir.y;
			dir.y = 0;
			float dist = dir.magnitude;
			float angleRad = Mathf.Deg2Rad * angleDeg;
			dir.y = dist * Mathf.Tan(angleRad);
			dist += height / Mathf.Tan(angleRad);

			print (dist + " " + Physics.gravity.magnitude + " " + angleRad);

			float velocity = Mathf.Sqrt(Mathf.Abs(dist) * Physics.gravity.magnitude / Mathf.Sin(2 * angleRad));

			return velocity * dir.normalized;
		} else {
			// Otherwise, throw nearly direct
			return destination - origin;
		}
	}



	private Vector3 getRandomPosition ()
	{
		// Generate random PHI and THETA
		float r = 0;
		float theta = Random.Range (0, 2 * Mathf.PI);
		float phi = Random.Range (0, Mathf.PI);

		float val1 = Mathf.Pow (Mathf.Cos (phi), 2) * Mathf.Pow (Mathf.Sin (theta), 2) / Mathf.Pow (a * 100, 2);
		float val2 = Mathf.Pow (Mathf.Sin (phi), 2) * Mathf.Pow (Mathf.Sin (theta), 2) / Mathf.Pow (b * 100, 2);
		float val3 = Mathf.Pow (Mathf.Cos (theta), 2) / Mathf.Pow (c * 100, 2);

		r = Mathf.Sqrt (1 / (val1 + val2 + val3));

		float x = r * Mathf.Sin (phi) * Mathf.Cos (theta);
		float y = r * Mathf.Sin (phi) * Mathf.Sin (theta);
		float z = r * Mathf.Cos (phi);

		// Flip y if too low
		if (y < -120) {
			y = Mathf.Abs (y);
		}

		// Return position relative to parent
		return new Vector3 (x, y, z);
	}

	public void beginSpawning ()
	{
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
		spawning = true;
	}

	public void stopSpawning()
	{
		CancelInvoke ();
		spawning = false;
	}
}
