using System;
using System.Drawing;
using Rhino;
using Rhino.Commands;
using Rhino.Display;
using Rhino.PlugIns;

namespace RhinoAnywhere
{
    public class FrameBufferTest : Command
    {
        public FrameBufferTest()
        {
            Instance = this;
        }

        ///<summary>The only instance of the MyCommand command.</summary>
        public static FrameBufferTest Instance { get; private set; }

        public override string EnglishName => "FrameBufferTest";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Rhino.Display.DisplayPipeline.DrawForeground += DisplayPipeline_PostDrawObjects; ;
            return Result.Success;
        }

        private void DisplayPipeline_PostDrawObjects(object sender, DrawEventArgs e)
        {
            // TODO: complete command.
            RhinoView activeView = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;

            var size = activeView.Size;
            using (var bitmap = new Bitmap(size.Width, size.Height))
            {
                Bitmap outputBitmap = e.Display.FrameBuffer;
            }
        }
    }
}