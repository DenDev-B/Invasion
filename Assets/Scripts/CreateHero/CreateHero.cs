using UnityEngine;

public class CreateHero : MonoBehaviour
{
    public int heroCount;

    GameObject hero;

    int num = 1;

    void Start()
    {
        if (heroCount > 0)
        {
            onCreateHero("Player" + num);
        }
    }

    private void onCreateHero(object p)
    {
        if (hero)
            Destroy(hero);

       /* hero = Instantiate(Resources.Load<GameObject>("Hero/" + v));
        hero.transform.position = new Vector3(0f, 0f, 0f);
        hero.transform.SetParent(view);

        hero.name = string.Format(v);*/
    }

   
}
