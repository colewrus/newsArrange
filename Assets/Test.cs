using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class Test : MonoBehaviour {

    public enum NewSource { ap, pp, cnn};

    NewSource mysource = NewSource.ap;


	public static int count;

	public string url = "http://hosted2.ap.org/atom/APDEFAULT/3d281c11a96b4ad082fe88aa0db04305";
	public List<GameObject> newsObjs = new List<GameObject>();
	public List<RawObj> newsStories = new List<RawObj> ();
	public List<GameObject> homepageText = new List<GameObject> ();

	public GameObject homescreen;
	public GameObject Datapage;

    XmlNamespaceManager nsmgr;
    // Use this for initialization
    void Start () {
		Datapage.SetActive (true);
		homescreen.SetActive (false);
		//////StartCoroutine (News ());
	}

	public IEnumerator News(){
        if(mysource == NewSource.pp)
        {
            url = "http://feeds.propublica.org/propublica/main";
        }
        if(mysource == NewSource.ap)
        {
            url = "http://hosted2.ap.org/atom/APDEFAULT/3d281c11a96b4ad082fe88aa0db04305";
        }
        if(mysource == NewSource.cnn)
        {
            url = "http://rss.cnn.com/rss/cnn_topstories.rss";
        }

        WWW www = new WWW (url);

		yield return www;
		if (www.isDone) {
			XmlDocument xmlDoc = new XmlDocument ();
			xmlDoc.LoadXml (www.text);

			XmlNodeList titleList;
			XmlNodeList summaryList;

          
            
            nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("ab", "http://www.w3.org/2005/Atom");
           
            //xmlDoc.SelectNodes ("//id");
            //xmlDoc.DocumentElement.ChildNodes;
            //Debug.Log (xmlDoc.InnerText);
            if(mysource == NewSource.ap)
            {
                summaryList = xmlDoc.DocumentElement.SelectNodes("//ab:entry", nsmgr);
                foreach (XmlNode xmlNode in summaryList)
                {
                    xmlNode.RemoveChild(xmlNode.FirstChild);
                }
                Debug.Log(summaryList.Count);
                for (int i = 0; i < newsObjs.Count; i++)
                {
                    TextMeshProUGUI titleText = newsObjs[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    titleText.text = summaryList[i].FirstChild.InnerText; //title
                    newsObjs[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = summaryList[i].ChildNodes[1].InnerText; //body

                    RawObj tempObj = new RawObj();
                    tempObj.Title = titleText.text;
                    tempObj.Summary = summaryList[i].ChildNodes[1].InnerText;
                    tempObj.posID = i;
                    newsStories.Add(tempObj);
                }
            }
            
            if(mysource == NewSource.pp)
            {
               summaryList = xmlDoc.DocumentElement.SelectNodes("//item", nsmgr);
                
               for(int i=0; i < newsObjs.Count; i++)
                {
                    //Debug.Log(summaryList[i].FirstChild.InnerText);//title
                    string result = Regex.Replace(summaryList[i].ChildNodes[4].InnerText, @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>", string.Empty);
                    result = result.TrimStart();
                    Debug.Log(result);
                    //string[] x = summaryList[i].ChildNodes[4].InnerText.Split();
                   
                }
            }

            if(mysource == NewSource.cnn)
            {
               
                for(int i=0; i < newsObjs.Count; i++)
                {
                    summaryList = xmlDoc.DocumentElement.SelectNodes("//item", nsmgr);
                    
                    string result = Regex.Replace(summaryList[i].ChildNodes[1].InnerText, @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>", string.Empty);
                    newsObjs[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = summaryList[i].ChildNodes[0].InnerText;
                    newsObjs[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = result;
                }
            }

		}
	}

	public void AddStory(int id){
		
		Homepage ();
		for (int i = 0; i < homepageText.Count; i++) { 
			if (!homepageText [i].activeSelf) {
				homepageText [i].transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = newsStories [id].Title;

				homepageText[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = newsStories[id].Summary;
				homepageText [i].SetActive (true);
				break;
			}
		}

	}

	void Update(){

	}


    public void SelectSource(string s)
    {
        if (s == "ap")
            mysource = NewSource.ap;
        if (s == "pp")
            mysource = NewSource.pp;
        if (s == "cnn")
            mysource = NewSource.cnn;

        StartCoroutine(News());
    }
	public void Homepage(){ //switch to the homepage screen
		homescreen.SetActive (true);
		Datapage.SetActive (false);
	}

	public void Datacanvas(){
		homescreen.SetActive (false);
		Datapage.SetActive (true);
	}
}
