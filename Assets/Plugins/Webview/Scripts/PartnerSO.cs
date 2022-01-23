using UnityEngine;

public class PartnerSO : ScriptableObject
{
    private const string DefaultSubdomain = "demo";

    public string Subdomain = null;

    public string GetUrl()
    {
        return $"https://{GetSubdomain()}.readyplayer.me/avatar";
    }

    public string GetSubdomain()
    {
        return Subdomain ?? DefaultSubdomain;
    }
}
