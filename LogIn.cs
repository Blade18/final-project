using Proyecto26;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogIn : MonoBehaviour
{
    public InputField usernameText;
    public InputField passwordText;

    [SerializeField]
    private Text status;

    public static string Password;
    public static string playerName;

    public void OnSubmit()
    {
        Debug.Log("try to login");
        playerName = usernameText.text;
        Password = passwordText.text;
        if (Password == null || Password == "")
        {
            status.text = "Enter a password!";
            return;
        }
        if (playerName == null || playerName == "")
        {
            status.text = "Enter a username!";
            return;
        }
        RestClient.Get<User>("https://finalproject-fb30d.firebaseio.com/" + playerName + ".json").Then(response =>
        {
            if (string.Equals(response.userName, playerName) && string.Equals(response.password, Password))
            {
                Debug.Log("work");
                status.text = "";
                GameManager.GetUser(response);
                SceneManager.LoadScene(2);
            }
            else
                status.text = "Incorrect username or password";
        }).Catch(error =>
        {
            status.text = "Incorrect username or password";
        });

    }

    public void MoveScene()
    {
        SceneManager.LoadScene(1);
    }

}
