using System.Collections.Generic;

public class UpdateInfoHelper 
{

    public string GetTitle() => I18nMgr.T("Game Update Announcement");
    public List<string> GetUpdateContent(string content)
    {
        List<string> rows = new List<string>();

        rows.Add($"<color=#68FFD6>【{I18nMgr.T("Version information")}】</color>");

        string strGameTheme = I18nMgr.T(ApplicationSettings.Instance.gameTheme);
        rows.Add($"* {I18nMgr.T("GAME THEME")}: {strGameTheme}");

        string strReleaseDebug = ApplicationSettings.Instance.isRelease ? "Release" : "Debug";
        string ver = GlobalData.hotfixVersion;
        rows.Add($"* {I18nMgr.T("VER")}: v{ver} - {I18nMgr.T(strReleaseDebug)}");

        string strIsMachine = ApplicationSettings.Instance.isMachine ? "True" : "False";
        rows.Add($"* {I18nMgr.T("IS MACHINE")}: {I18nMgr.T(strIsMachine)}");

        rows.Add($"<color=#68FFD6>【{I18nMgr.T("Update content")}】</color>");

        List<string> _res = GameMaker.Utils.GetContentLST(content);
        List<string> res = new List<string>();
        for (int i = 0; i < _res.Count; i++)
        {
      
            if (string.IsNullOrEmpty(_res[i]))// 已经支持多国语言
                continue;

            res.Add("* " + _res[i]);
        }
        rows.AddRange(res);

        // pages = SplitIntoPages(rows, pageContentRow);

        return rows;
    }
}
