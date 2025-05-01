using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _instance;
    private string serverUrl = "http://localhost:3000"; // Change to your local IP for mobile testing

    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("NetworkManager");
                _instance = obj.AddComponent<NetworkManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    [Serializable]
    public class UserData
    {
        public int id;
        public string username;
        public string email;
        public int score;
    }

/*    [Serializable]
    public class EmailWrapper
    {
        public string email;
    }
*/

    // Register a new user
    public IEnumerator RegisterUser(string username, string email, System.Action<string> callback)
    {
        string jsonData = $"{{\"username\":\"{username}\", \"email\":\"{email}\"}}";
        using (UnityWebRequest request = new UnityWebRequest(serverUrl + "/register", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            callback(request.result == UnityWebRequest.Result.Success ? request.downloadHandler.text : request.error);
        }
    }

    private IEnumerator LoginUserRequest(string email, Action<bool, int, int> callback)
    {
        UserData wrapper = new UserData { email = email };
        string jsonData = JsonUtility.ToJson(wrapper);

        Debug.Log("Sending login JSON: " + jsonData);  // should log: {"email":"akram@gmail.com"}

        using (UnityWebRequest request = new UnityWebRequest(serverUrl + "/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var user = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
                callback(true, user.id, user.score);
            }
            else
            {
                Debug.LogError("Login error: " + request.downloadHandler.text);
                callback(false, -1, 0);
            }
        }
    }



    public void LoginUser()
    {
        string email = loginEmailInput.text;

        StartCoroutine(LoginUserRequest(email, (success, userId, score) =>
        {
            if (success)
            {
                LoggedInUserId = userId;
                HighScore = score;
                loginResultText.text = $"Welcome! Score: {score}";
                SceneManager.LoadScene("PlayerSelection"); // or next scene
            }
            else
            {
                loginResultText.text = "Login failed!";
            }
        }));
    }

    //to register new user

    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_Text resultText;

    public TMP_InputField loginEmailInput;
    public TMP_Text loginResultText;

    public static int LoggedInUserId = -1;
    public static int HighScore = 0;

    public void RegisterUser()
    {
        Debug.Log("RegisterUser() was called");
        string username = usernameInput.text;
        string email = emailInput.text;

        StartCoroutine(NetworkManager.Instance.RegisterUser(username, email, (response) =>
        {
            resultText.text = "Register Success";
            Debug.Log("Register Success");
            SceneManager.LoadScene("Login");
        }));
    }

    public void UpdateScoreIfBetter(float timeInSeconds)
    {
        StartCoroutine(UpdateScoreCoroutine(timeInSeconds));
    }

    private IEnumerator UpdateScoreCoroutine(float timeInSeconds)
    {
        int userId = LoggedInUserId;
        if (userId == -1)
        {
            Debug.LogWarning("No user logged in.");
            yield break;
        }

        // Get current user data from server
        UnityWebRequest getRequest = UnityWebRequest.Get($"{serverUrl}/user/{userId}");
        yield return getRequest.SendWebRequest();

        if (getRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch user data: " + getRequest.error);
            yield break;
        }

        UserData currentUser = JsonUtility.FromJson<UserData>(getRequest.downloadHandler.text);

        // Only update if new time is faster (lower)
        if (currentUser.score == 0 || timeInSeconds < currentUser.score)
        {
            Debug.Log($"New high score! Sending time {timeInSeconds}s to server.");
            string jsonData = JsonUtility.ToJson(new ScoreUpdate { id = userId, score = Mathf.FloorToInt(timeInSeconds) });

            UnityWebRequest postRequest = new UnityWebRequest(serverUrl + "/update-score", "PUT");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            postRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            postRequest.downloadHandler = new DownloadHandlerBuffer();
            postRequest.SetRequestHeader("Content-Type", "application/json");

            yield return postRequest.SendWebRequest();

            if (postRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Score updated successfully.");
            }
            else
            {
                Debug.LogError("Failed to update score: " + postRequest.error);
            }
        }
        else
        {
            Debug.Log("Existing score is better. No update sent.");
        }
    }

    [Serializable]
    public class ScoreUpdate
    {
        public int id;
        public int score;
    }
}
