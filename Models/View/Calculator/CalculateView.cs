using System;
using System.Collections.Generic;

namespace EliteBuild.Models.View
{
    public class CalculateView
    {
        public Int32 Category { get; set; }
        public Int32 Work { get; set; }
        public Single Price { get; set; }
        public List<RoomView> Items { get; set; } = new List<RoomView>();
    }

    public class RoomView
    {
        public RoomView(Int32 Input1, Int32 Input2)
        {
            this.Input1 = Input1;
            this.Input2 = Input2;
        }
        public Int32 Input1 { get; set; }
        public Int32 Input2 { get; set; }
    }
}