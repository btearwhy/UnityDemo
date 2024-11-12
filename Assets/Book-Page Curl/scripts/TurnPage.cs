using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPage : MonoBehaviour
{
    // Start is called before the first frame update
    public float t;
    public bool animateAutomatically = true;
    public bool reverseDirection = false;
    public float duration = 0.5f;
    public Vector2 _pageSize;
    public float pageTurnAmount = 180f;

    // This method computes rho, theta, and A for time parameter t using pre-defined functions to simulate a natural page turn
    // without finger tracking, i.e., for a quick swipe of the finger to turn to the next page.
    // These functions were constructed empirically by breaking down a page turn into phases and experimenting with trial and error
    // until we got acceptable results. This basic example consists of three distinct phases, but a more elegant solution yielding
    // smoother transitions can be obtained by curve fitting functions to our key data points once satisfied with the behavior.
    public float angle1 = 90.0f;        //  }
    public float angle2 = 8.0f;            //  }
    public float angle3 = 6.0f;            //  }
    public float a1 = -15.0f;            //  }
    public float a2 = -2.5f;            //  }--- Experiment with these parameters to adjust the page turn behavior to your liking.
    public float a3 = -3.5f;            //  }
    public float theta1 = 0.05f;        //  }
    public float theta2 = 0.5f;            //  }
    public float theta3 = 10.0f;        //  }
    public float theta4 = 2.0f;            //  }

    private Mesh mesh;
    private float PI;
    private float RAD;
    private float rho;                    // Rotation of the page around the spine of the book (y axis).
    private float theta;                // Angle of the cone modeling the page curl deformation. Valid range of {0...π/2}. Very small values close to zero may give weird but interesting results such as a scroll effect.
                                        // Smaller values produce a pronounced curling effect across the width of the page. A value of π/2 (90˚) results in a perfectly flat page.
    private float apex;                    // Translation of the cone apex along the spine of the book. Larger values position the apex farther away from the page, resulting in an elongated cone and
                                           // more gradual curvature across the height of the page. Smaller values position the apex closer to the page and result in a more pronounced curl from a page corner.
    private Vector3[] v0;


    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        PI = Mathf.PI;
        RAD = 180f / PI;
        v0 = mesh.vertices;
        if (v0.Length > 0)
        {
            Vector3 lastVector = v0[v0.Length - 1];
            _pageSize = new Vector2(lastVector.x, lastVector.z);
        }
    }


    void Update()
    {
        if (animateAutomatically)
            t = (duration * Time.time) % 1f;
        /*if (reverseDirection)
            t = 1f - t;*/

        RebuildMesh(t);
    }


    void CalculateMesh(float t)
    {
        float dt;
        float n1;
        float n2;
        float ang1 = angle1 / RAD;
        float ang2 = angle2 / RAD;
        float ang3 = angle3 / RAD;

        if (t <= 0.15f)
        {
            // Start off with a flat page with no deformation at the beginning of a page turn, then begin to curl the page gradually
            // as the hand lifts it off the surface of the book.
            dt = t / 0.15f;
            n1 = Mathf.Sin((PI * Mathf.Pow(dt, theta1)) / 2f);
            n2 = Mathf.Sin((PI * Mathf.Pow(dt, theta2)) / 2f);
            theta = FuncLinear(n1, ang1, ang2);
            apex = FuncLinear(n2, a1, a2);
        }
        else if (t <= 0.4f)
        {
            // Produce the most pronounced curling near the middle of the turn. Here small values of theta and A
            // result in a short, fat cone that distinctly show the curl effect.
            dt = (t - 0.15f) / 0.25f;
            theta = FuncLinear(dt, ang2, ang3);
            apex = FuncLinear(dt, a2, a3);
        }
        else
        {
            // Near the middle of the turn, the hand has released the page so it can return to its normal form.
            // Ease out the curl until it returns to a flat page at the completion of the turn. More advanced simulations
            // could apply a slight wobble to the page as it falls down like in real life.
            dt = (t - 0.4f) / 0.6f;
            n1 = Mathf.Sin((PI * Mathf.Pow(dt, theta3)) / 2f);
            n2 = Mathf.Sin((PI * Mathf.Pow(dt, theta4)) / 2f);
            theta = FuncLinear(n1, ang3, ang1);
            apex = FuncLinear(n2, a3, a1);
        }
    }


    void RebuildMesh(float t)
    {
        if (v0.Length == 0) return;

        CalculateMesh(t);

        Vector3[] newMesh = new Vector3[v0.Length];

        for (int i = 0; i < newMesh.Length; i++)
            newMesh[i] = CurlTurn(v0[i]);

        mesh.vertices = newMesh;
        mesh.RecalculateNormals();

        // Here rho, the angle of the page rotation around the spine, is a linear function of time t. This is the simplest case and looks
        // Good Enough. A side effect is that due to the curling effect, the page appears to accelerate quickly at the beginning
        // of the turn, then slow down toward the end as the page uncurls and returns to its natural form, just like in real life.
        // A non-linear function may be slightly more realistic but is beyond the scope of this example.
        rho = t * pageTurnAmount;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rho);
    }


    public Vector3 CurlTurn(Vector3 p)
    {
        float rhs = Mathf.Sqrt((p.x * p.x) + Mathf.Pow((p.z - apex), 2.0f));
        float r = rhs * Mathf.Sin(theta);
        float beta = Mathf.Asin(p.x / rhs) / Mathf.Sin(theta);
        p.x = r * Mathf.Sin(beta);
        p.z = (rhs + apex) - ((r * (1 - Mathf.Cos(beta))) * Mathf.Sin(theta));
        p.y = (r * (1 - Mathf.Cos(beta))) * Mathf.Cos(theta);
        return p;
    }


    public float FuncLinear(float ft, float f0, float f1)
    {
        return (f0 + ((f1 - f0) * ft));
    }
}
