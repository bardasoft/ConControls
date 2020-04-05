using System;

namespace ConControls.WindowsApi.Types
{
    [Flags]
    enum ConCharAttributes : ushort
    {
        None = 0,

        ForeGroundBlue = 0x0001,
        ForeGroundGreen = 0x0002,
        ForeGroundRed = 0x0004,
        ForeGroundIntensity = 0x0008,

        BackGroundBlue = 0x0010,
        BackGroundGreen = 0x0020,
        BackGroundRed = 0x0040,
        BackGroundIntensity = 0x0080,
        
        CommonLvbLeadingByte = 0x0100,
        CommonLvbTrailingByte = 0x0200,
        CommonLvbGridHorizontal = 0x0400,
        CommonLvbGridLeftVertical = 0x0800,
        CommonLvbGridRightVertical = 0x1000,
        CommonLvbReverseVideo = 0x4000,
        CommonLvbUnderscore = 0x8000
    }
}
