using TMPro;
using UnityEngine;

public class HitPopups : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private float movey;
    [SerializeField] private float timer;
    [SerializeField] private Color Color;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
    }

    public static HitPopups Create(Vector3 pos, int Damager)
    {
        var pop = Instantiate(GlobalReferences.instances.hitPopups, pos, Quaternion.identity);

        HitPopups info = pop.GetComponent<HitPopups>();

        info.SetUp(Damager);

        return info;
    }

    private void SetUp(int Damager)
    {
        text.SetText(Damager.ToString());
        text.color = Color;
    }

    private void Update()
    {
        transform.position += new Vector3(0, movey, 0) * Time.deltaTime;

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            float timeToAlpha = 2;
            Color.a -= timeToAlpha * Time.deltaTime;
            text.color = Color;

            if (Color.a < 0) 
                Destroy(gameObject);
        }

    }
}
