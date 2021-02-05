using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Week1_1 : MonoBehaviour
{
    public Text Shing;
    string animal ;
    public Text Showtext;
    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void inventory()
    {
        animal = Shing.text;

        switch (animal)
        {
            case "Cat": 
               Showtext.text = animal + " is found";
                break;
            case "Dog":
                Showtext.text = animal + " is found";
                break;
            case "Fish":
                Showtext.text = animal + " is found";
                break;
            case "Lion":
                Showtext.text = animal + " is found";
                break;
            case "Bee":
                Showtext.text = animal + " is found";
                break;
            case "Wolf":
                Showtext.text = animal + " is found";
                break;
            default:
                Showtext.text = animal + " is not found";
                break;
        }



    }

}
