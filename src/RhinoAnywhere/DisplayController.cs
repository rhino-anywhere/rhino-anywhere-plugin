using Rhino.Display;
using Rhino.Geometry;

namespace RhinoAnywhere
{
    public static class DisplayController
    {
        public static Point2d WebViewToServerWindowCoordinate(double xLocation, double yLocation)
        {
            RhinoView view = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;
            int rhinoX = (int)(view.Bounds.Width * xLocation);
            int rhinoY = (int)(view.Bounds.Height * yLocation);

            return view.ClientToScreen(new Point2d(rhinoX, rhinoY));
        }
    }
}
