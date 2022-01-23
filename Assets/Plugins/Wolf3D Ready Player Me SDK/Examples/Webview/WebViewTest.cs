using UnityEngine;

namespace Wolf3D.ReadyPlayerMe.AvatarSDK
{
    public class WebViewTest : MonoBehaviour
    {
        private GameObject avatar;

        [SerializeField] private WebView webView;
        [SerializeField] private GameObject loadingLabel = null;
        [SerializeField] private GameObject displayButton = null;

        public void DisplayWebView()
        {
            if(webView == null)
            {
                webView = FindObjectOfType<WebView>();
            }
            else if (webView.Loaded)
            {
                webView.SetVisible(true);
            }
            else
            {
                webView.CreateWebView();
                webView.OnAvatarCreated = OnAvatarCreated;
            }
        }

        // WebView callback for retrieving avatar url
        private void OnAvatarCreated(string url)
        {
            if (avatar) Destroy(avatar);

            webView.SetVisible(false);
            loadingLabel.SetActive(true);
            displayButton.SetActive(false);

            AvatarLoader avatarLoader = new AvatarLoader();
            avatarLoader.LoadAvatar(url, null, OnAvatarImported);
        }

        // AvatarLoader callback for retrieving loaded avatar game object
        private void OnAvatarImported(GameObject avatar, AvatarMetaData metaData)
        {
            this.avatar = avatar;
            loadingLabel.SetActive(false);
            displayButton.SetActive(true);

            Debug.Log("Loaded");
        }
    }
}
