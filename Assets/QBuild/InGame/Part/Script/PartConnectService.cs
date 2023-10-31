using QBuild.Utilities;

namespace QBuild.Part
{
    public class PartConnectService
    {
        /// <summary>
        /// パーツの接続時に必要な処理を行う。
        /// </summary>
        /// <param name="partView">基準となるパーツ</param>
        /// <param name="dir">基準のパーツから接続される向き</param>
        /// <param name="otherPartView">接続したいパーツ</param>
        public static void ConnectPart(PartView partView, DirectionFRBL dir, PartView otherPartView)
        {
            partView.SetCanConnect(dir, false);
            otherPartView.SetCanConnect(dir.Turn180(), false);
        }
    }
}