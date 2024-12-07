
/// <summary>
/// Return Example :
/// 
/// "thumbnail": {
///    "source": "https://upload.wikimedia.org/wikipedia/commons/thumb/7/75/Logo_Virtual_Regatta.svg/320px-Logo_Virtual_Regatta.svg.png",
///        "width": 320,
///        "height": 149
/// },
/// "extract": "Virtual Regatta is an online web browser sailing race simulator, though the development of a mobile app version of the game has seen a significant number of users shift to this platform in recent years."
/// 
/// 
/// </summary>
[System.Serializable]
public class WikipediaSummary
{
    public Thumbnail thumbnail;
    public string extract;
}
