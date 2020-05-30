using Proyecto26;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField usernameText;
    public InputField passwordText;
    public InputField passwordTextCon;

    [SerializeField]
    private Text status;

    public static string password;
    public static string passwordcon;
    public static string playerName;

    private string databaseURL = "https://finalproject-fb30d.firebaseio.com/";

    private int passwordcheck = 1;
    private bool namecheck = true;

    public void OnSubmit()
    {
        Debug.Log("try to signup");
        namecheck = true;
        playerName = usernameText.text;
        password = passwordText.text;
        passwordcon = passwordTextCon.text;
        if (password == null || password == "" || passwordcon == null || passwordcon == "")
        {
            status.text = "Enter a password!";
            return;
        }
        if (playerName == null || playerName == "" )
        {
            status.text = "Enter a username!";
            return;
        }
        if (password.Length <= 7)
        {
            status.text = "Use 8 characters or more for your password";
            return;
        }
        if (playerName.Length > 8)
        {
            status.text = "User name is to long";
            return;
        }

        RestClient.Get<User>("https://finalproject-fb30d.firebaseio.com/" + playerName + ".json").Then(response =>
        {
            if (string.Equals(response.userName, playerName))
            {
                namecheck = false;
                Debug.Log("uncorrect");
                status.text = "That username is taken. Try another.";
            }

        }).Catch(error =>
        {
            Debug.Log("good usesrname");
            if (namecheck)
            {
                passwordcheck = string.Compare(password, passwordcon);
                if (passwordcheck == 0)
                {
                    status.text = "";
                    PostToDatabase();
                }
                else
                    status.text = "Those passwords didn't match. Try again.";
            }
        });

    }

    private void PostToDatabase()
    {
        Debug.Log("send");
        User user = new User();
        RestClient.Put("https://finalproject-fb30d.firebaseio.com/" + playerName + ".json", user);
        SceneManager.LoadScene(0);
    }

    public void MoveScene()
    {
        SceneManager.LoadScene(0);
    }

}
