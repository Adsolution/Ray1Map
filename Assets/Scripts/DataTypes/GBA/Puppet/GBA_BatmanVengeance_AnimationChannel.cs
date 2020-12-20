﻿using static R1Engine.GBA_AnimationChannel;

namespace R1Engine
{
    public class GBA_BatmanVengeance_AnimationChannel : R1Serializable {
        #region Data
        public short XPosition { get; set; }
        public short YPosition { get; set; }
        public ushort ImageIndex { get; set; }

		#endregion

		#region Parsed
        public int XSize { get; set; } = 1;
        public int YSize { get; set; } = 1;
        public int PaletteIndex { get; set; }
        public bool IsFlippedHorizontally { get; set; }
        public bool IsFlippedVertically { get; set; }
        public Shape SpriteShape { get; set; }
        public int SpriteSize { get; set; }
        #endregion

        #region Public Methods

        public override void SerializeImpl(SerializerObject s) 
        {
            s.SerializeBitValues<byte>(bitFunc =>
            {
                bitFunc(default, 6, name: "Padding");
                IsFlippedHorizontally = bitFunc(IsFlippedHorizontally ? 1 : 0, 1, name: nameof(IsFlippedHorizontally)) == 1;
                IsFlippedVertically = bitFunc(IsFlippedVertically ? 1 : 0, 1, name: nameof(IsFlippedVertically)) == 1;
            });
            s.SerializeBitValues<byte>(bitFunc =>
            {
                SpriteSize = bitFunc(SpriteSize, 2, name: nameof(SpriteSize));
                SpriteShape = (Shape)bitFunc((int)SpriteShape, 2, name: nameof(SpriteShape));
                PaletteIndex = bitFunc(PaletteIndex, 4, name: nameof(PaletteIndex));
            });

            YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));
            XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
            ImageIndex = s.Serialize<ushort>(ImageIndex, name: nameof(ImageIndex));

            // Calculate size
            XSize = 1;
            YSize = 1;
            switch (SpriteShape) {
                case Shape.Square:
                    XSize = 1 << SpriteSize;
                    YSize = XSize;
                    break;
                case Shape.Wide:
                    switch (SpriteSize) {
                        case 0: XSize = 2; YSize = 1; break;
                        case 1: XSize = 4; YSize = 1; break;
                        case 2: XSize = 4; YSize = 2; break;
                        case 3: XSize = 8; YSize = 4; break;
                    }
                    break;
                case Shape.Tall:
                    switch (SpriteSize) {
                        case 0: XSize = 1; YSize = 2; break;
                        case 1: XSize = 1; YSize = 4; break;
                        case 2: XSize = 2; YSize = 4; break;
                        case 3: XSize = 4; YSize = 8; break;
                    }
                    break;
            }
        }

        #endregion
    }
}