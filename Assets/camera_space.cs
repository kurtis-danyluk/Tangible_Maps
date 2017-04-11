using UnityEngine;
using System.Collections;

public class camera_space : MonoBehaviour {

    private float z_sc = 500;
    private float x_sc = 833;
    private float y_sc = 100;

    private float z_m_s = 0;
    private float x_m_s = 0;

    OneEuroFilter<Vector3> vec3Filter;
    OneEuroFilter heightFilter;

    public float filterfrequency = 240.0f;

    ArrayList last_frames;

	// Use this for initialization
	void Start () {

        vec3Filter = new OneEuroFilter<Vector3>(filterfrequency);
        heightFilter = new OneEuroFilter(filterfrequency);
	
	}
	
	// Update is called once per frame
	void Update () {

        //Range: -3,3
        float z_t = (tran.position.x * z_sc);
        //Range: -2,2
        float x_t = (tran.position.y * x_sc);
        //Range: 4,8
    //    float y_t = (tran.position.y * y_sc);

        float oh_tt = Terrain.activeTerrain.SampleHeight(transform.position);
        float h_tt = Terrain.activeTerrain.SampleHeight(new Vector3(x_t, 0, -z_t));
     //   h_tt = heightFilter.Filter(h_tt);

        // transform.position = tran.position;
        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3((x_t + x_m_s), h_tt, -(z_t + z_m_s));



        transform.position = newPos;

        Vector3 dif_pos = (newPos - oldPos).normalized;
      //  Vector3 nPos = oldPos + dif_pos * 15.0f;
      //  nPos.y = Terrain.activeTerrain.SampleHeight(nPos);

      //  dif_pos = (nPos - oldPos);


        Vector3 filteredVec = vec3Filter.Filter<Vector3>(dif_pos);
        dif_pos = filteredVec;
        float y_rot = heightFilter.Filter(tran.rotation.y);
        Quaternion q_r = Quaternion.LookRotation(new Vector3(dif_pos.x, dif_pos.z, -y_rot));
        transform.rotation = transform.rotation = Quaternion.Lerp(transform.rotation,q_r, 0.1f);
        


	}
    public Transform tran;

    public void tune_X(float v)
    {
        x_sc = v;
    }
    public void tune_Y(float v)
    {
        y_sc = v;
    }


}
