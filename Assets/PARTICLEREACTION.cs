using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PARTICLEREACTION : MonoBehaviour
{
    GameObject QuestionMark;
    GameObject Heart;
    List<GameObject> Particle_LIST = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        QuestionMark = GameObject.Find("P_QuestionMark");
        Heart = GameObject.Find("P_Heart");
        Particle_LIST.Add(QuestionMark);
        Particle_LIST.Add(Heart);

        StartCoroutine(ExampleCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayParticle(int index)
    {
        if (index > Particle_LIST.Count)
        {
            return;
        }

        Particle_LIST[index].GetComponent<ParticleSystem>().Play();
    }


    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        PlayParticle(0);
        yield return new WaitForSeconds(2.0f);
        PlayParticle(1);
    }
}
