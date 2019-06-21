using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ActiveAccount : MonoBehaviour {
	[SerializeField] private InputField Code;
	[SerializeField] private Text WarningMSG;
	[SerializeField] public string activeURL ;
	[SerializeField] public string resendURL ;

	[SerializeField] public string securePassword;
	private UiAccountManager GetLoginCanvas;


    void Start() => GetLoginCanvas = gameObject.GetComponent<UiAccountManager>();

    public void Resend() => StartCoroutine(Query_Php("Resend"));

    public void Active() => StartCoroutine(Query_Php("Active"));

    IEnumerator Query_Php(string type){
		if (type == "Active") {
			WWW query = new WWW (activeURL + "?" + "code=" + Code.text + "&secure=" + securePassword);
			yield return query;
            if (RemoveExceptNumber(query.text.Trim ()) == "1") {
				GetLoginCanvas.ToggleCanvas ("login");
			} else {
				WarningMSG.text = query.text;
			}
		} else if (type == "Resend") {
			string getTempAccount = PlayerPrefs.GetString ("TempUser");
			WWW query = new WWW (resendURL + "?" + "username=" + getTempAccount + "&secure=" + securePassword);
			yield return query;
            string[] split = query.text.Split(',');
            if (RemoveExceptNumber(split[0]) == "1") {
				WarningMSG.color = Color.green;
				WarningMSG.text = "Send Code To your Email: " + split[1];
			} else {
				WarningMSG.color = Color.red;

				WarningMSG.text = query.text;
			}
		}
	}
    
    static string RemoveExceptNumber(string s)
    {
        string result = string.Empty;
        foreach (var c in s)
        {
            int ascii = (int)c;
            if ((ascii >= 48 && ascii <= 57))
                result += c;
        }
        return result;
    }
}
