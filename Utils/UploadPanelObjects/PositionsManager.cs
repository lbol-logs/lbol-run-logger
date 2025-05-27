using UnityEngine;

namespace RunLogger.Utils.UploadPanelObjects
{
    internal static class PositionsManager
    {
        private const float X = 0;
        private const float Y = -14.88f;
        private const float BgOffsetX = -3.7f;
        private const float UploadOffsetX = 0.7f;
        private const float StatusOffsetY = 0.06f;
        private const float BoxOffsetY = 5.48f;

        internal const float QuickUploadOffsetX = 0.5f;
        internal const float QuickUploadScale = 0.004f;
        internal const float SwitchOffsetX = -600;

        internal static Vector3 BgPosition = new Vector3(X + PositionsManager.BgOffsetX, Y, 10);
        internal static Vector3 AutoUploadPosition = new Vector3(X, Y, 10);
        internal static Vector3 UploadPosition = new Vector3(X + PositionsManager.UploadOffsetX, Y, 10);
        internal static Vector3 StatusPosition = new Vector3(X + PositionsManager.UploadOffsetX, Y + PositionsManager.StatusOffsetY, 10);
        internal static Vector3 InputLocalPosition = new Vector3(0, 100, 0);
        internal static Vector3 InputSizeDelta = new Vector2(2000, 700);
        internal static Vector3 BoxPosition = new Vector3(X, Y + PositionsManager.BoxOffsetY, 10);
        internal static Vector3 CountLocalPosition = new Vector3(150, -340, 0);
    }
}