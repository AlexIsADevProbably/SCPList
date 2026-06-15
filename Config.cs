namespace SCPList;

public class Config
{
    public bool Debug { get; set; }
    #if DEBUG
     = true;
    #else
     = false;
    #endif

    public string _watermarkText { get; set; } =
        "<color=#776AF5>C</color><color=#7265ED>i</color><color=#6D60E5>n</color><color=#685BDD>n</color><color=#6356D5>a</color><color=#5E51CD>m</color><color=#594CC5>o</color><color=#5447BD>n</color><color=#4F42B5>s</color> <color=#4538A5>S</color><color=#40339D>a</color><color=#3B2E95>p</color><color=#36298D>p</color><color=#312485>h</color><color=#2C1F7D>i</color><color=#271A75>r</color><color=#22156D>e</color>";
}
