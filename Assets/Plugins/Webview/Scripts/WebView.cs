using System;
using UnityEngine;

public class WebView: MonoBehaviour
{
    public bool Loaded { get; private set; }

    private WebViewWindowBase webViewObject = null;

    [SerializeField] private MessagePanel messagePanel = null;
    
    [Header("Padding")]
    [SerializeField] public int left;
    [SerializeField] public int top;
    [SerializeField] public int right;
    [SerializeField] public int bottom;

    // Event to call when webview starts, receives message.
    public Action<string> OnWebViewStarted;

    // Event to call when avatar is created, receives GLB url.
    public Action<string> OnAvatarCreated;

    /// <summary>
    ///     Create webview object attached to a MonoBehaviour object
    /// </summary>
    /// <param name="parent">Parent game object.</param>
    public void CreateWebView()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            messagePanel.SetMessage(MessagePanel.MessageType.NetworkError);
            messagePanel.SetVisible(true);
            messagePanel.TapToCloseEnabled = true;
        }
        else
        {
            #if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
                messagePanel.SetMessage(MessagePanel.MessageType.NotSupported);
                messagePanel.SetVisible(true);
                messagePanel.TapToCloseEnabled = true;
            #else
                if (webViewObject == null)
                {
                    messagePanel.SetMessage(MessagePanel.MessageType.Loading);
                    messagePanel.SetVisible(true);
                    messagePanel.TapToCloseEnabled = false;

                    #if UNITY_ANDROID
                        webViewObject = gameObject.AddComponent<AndroidWebViewWindow>();
                    #elif UNITY_IOS
                        webViewObject = gameObject.AddComponent<IOSWebViewWindow>();
                    #endif
                }

                webViewObject.OnLoaded = OnLoaded;
                webViewObject.OnJS = OnWebMessageReceived;

                WebViewOptions options = new WebViewOptions();
                webViewObject.Init(options);

                PartnerSO partner = Resources.Load<PartnerSO>("Partner");
                string url = partner.GetUrl();
                webViewObject.LoadURL(url);
            #endif
        }
        
        SetScreenPadding(left, top, right, bottom);
    }

    /// <summary>
    ///     Set webview screen padding in pixels.
    /// </summary>
    public void SetScreenPadding(int left, int top, int right, int bottom)
    {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;

        if (webViewObject)
        {
            webViewObject.SetMargins(left, top, right, bottom);
        }

        messagePanel.SetMargins(left, top, right, bottom);
    }

    public void SetVisible(bool visible)
    {
        webViewObject.IsVisible = visible;
    }

    private void OnWebMessageReceived(string message)
    {
        Debug.Log($"Message: {message}");

        if (message.Contains(".glb"))
        {
            webViewObject.IsVisible = false;
            OnAvatarCreated?.Invoke(message);
        }
    }

    private void OnLoaded(string message)
    {
        if (Loaded) return;

        Debug.Log("WebView Loaded.");

        webViewObject.EvaluateJS(@"
            if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                window.Unity = {
                    call: function(msg) { 
                        window.webkit.messageHandlers.unityControl.postMessage(msg); 
                    }
                }
            } 
            else {
                window.Unity = {
                    call: function(msg) {
                        window.location = 'unity:' + msg;
                    }
                }
            }

            function receiveMessage(event) {
			    Unity.call(event.data);
		    }

            window.removeEventListener('message', receiveMessage, false);
            window.addEventListener('message', receiveMessage, false);

            document.cookie = 'webview = true';
        ");

        Loaded = true;

        // Tasks break webview, used invoke instead.
        Invoke(nameof(DisplayWebView), 1f);
        Invoke(nameof(HideMessagePanel), 1.5f);
    }

    private void DisplayWebView() => webViewObject.IsVisible = true;
    private void HideMessagePanel() => messagePanel.gameObject.SetActive(false);

    private void OnDrawGizmos()
    {
        RectTransform rectTransform = transform as RectTransform;
        Gizmos.matrix = rectTransform.localToWorldMatrix;

        Gizmos.color = Color.green;
        Vector3 center = new Vector3((left - right) / 2, (bottom - top) / 2);
        Vector3 size = new Vector3(rectTransform.rect.width - (left + right), rectTransform.rect.height - (bottom + top));

        Gizmos.DrawWireCube(center, size);
    }
}
