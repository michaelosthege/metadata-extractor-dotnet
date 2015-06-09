/*
 * Copyright 2002-2015 Drew Noakes
 *
 *    Modified by Yakov Danilov <yakodani@gmail.com> for Imazen LLC (Ported from Java to C#)
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * More information about this project is available at:
 *
 *    https://drewnoakes.com/code/exif/
 *    https://github.com/drewnoakes/metadata-extractor
 */

using System.Collections.Generic;
using JetBrains.Annotations;
using Sharpen;

namespace Com.Drew.Metadata.Exif.Makernotes
{
    /// <summary>Describes tags specific to Fujifilm cameras.</summary>
    /// <author>Drew Noakes https://drewnoakes.com</author>
    public class FujifilmMakernoteDirectory : Directory
    {
        public const int TagMakernoteVersion = unchecked(0x0000);

        public const int TagSerialNumber = unchecked(0x0010);

        public const int TagQuality = unchecked(0x1000);

        public const int TagSharpness = unchecked(0x1001);

        public const int TagWhiteBalance = unchecked(0x1002);

        public const int TagColorSaturation = unchecked(0x1003);

        public const int TagTone = unchecked(0x1004);

        public const int TagColorTemperature = unchecked(0x1005);

        public const int TagContrast = unchecked(0x1006);

        public const int TagWhiteBalanceFineTune = unchecked(0x100a);

        public const int TagNoiseReduction = unchecked(0x100b);

        public const int TagHighIsoNoiseReduction = unchecked(0x100e);

        public const int TagFlashMode = unchecked(0x1010);

        public const int TagFlashEv = unchecked(0x1011);

        public const int TagMacro = unchecked(0x1020);

        public const int TagFocusMode = unchecked(0x1021);

        public const int TagFocusPixel = unchecked(0x1023);

        public const int TagSlowSync = unchecked(0x1030);

        public const int TagPictureMode = unchecked(0x1031);

        public const int TagExrAuto = unchecked(0x1033);

        public const int TagExrMode = unchecked(0x1034);

        public const int TagAutoBracketing = unchecked(0x1100);

        public const int TagSequenceNumber = unchecked(0x1101);

        public const int TagFinePixColor = unchecked(0x1210);

        public const int TagBlurWarning = unchecked(0x1300);

        public const int TagFocusWarning = unchecked(0x1301);

        public const int TagAutoExposureWarning = unchecked(0x1302);

        public const int TagGeImageSize = unchecked(0x1304);

        public const int TagDynamicRange = unchecked(0x1400);

        public const int TagFilmMode = unchecked(0x1401);

        public const int TagDynamicRangeSetting = unchecked(0x1402);

        public const int TagDevelopmentDynamicRange = unchecked(0x1403);

        public const int TagMinFocalLength = unchecked(0x1404);

        public const int TagMaxFocalLength = unchecked(0x1405);

        public const int TagMaxApertureAtMinFocal = unchecked(0x1406);

        public const int TagMaxApertureAtMaxFocal = unchecked(0x1407);

        public const int TagAutoDynamicRange = unchecked(0x140b);

        public const int TagFacesDetected = unchecked(0x4100);

        /// <summary>Left, top, right and bottom coordinates in full-sized image for each face detected.</summary>
        public const int TagFacePositions = unchecked(0x4103);

        public const int TagFaceRecInfo = unchecked(0x4282);

        public const int TagFileSource = unchecked(0x8000);

        public const int TagOrderNumber = unchecked(0x8002);

        public const int TagFrameNumber = unchecked(0x8003);

        public const int TagParallax = unchecked(0xb211);

        [NotNull]
        protected static readonly Dictionary<int?, string> TagNameMap = new Dictionary<int?, string>();

        static FujifilmMakernoteDirectory()
        {
            TagNameMap.Put(TagMakernoteVersion, "Makernote Version");
            TagNameMap.Put(TagSerialNumber, "Serial Number");
            TagNameMap.Put(TagQuality, "Quality");
            TagNameMap.Put(TagSharpness, "Sharpness");
            TagNameMap.Put(TagWhiteBalance, "White Balance");
            TagNameMap.Put(TagColorSaturation, "Color Saturation");
            TagNameMap.Put(TagTone, "Tone (Contrast)");
            TagNameMap.Put(TagColorTemperature, "Color Temperature");
            TagNameMap.Put(TagContrast, "Contrast");
            TagNameMap.Put(TagWhiteBalanceFineTune, "White Balance Fine Tune");
            TagNameMap.Put(TagNoiseReduction, "Noise Reduction");
            TagNameMap.Put(TagHighIsoNoiseReduction, "High ISO Noise Reduction");
            TagNameMap.Put(TagFlashMode, "Flash Mode");
            TagNameMap.Put(TagFlashEv, "Flash Strength");
            TagNameMap.Put(TagMacro, "Macro");
            TagNameMap.Put(TagFocusMode, "Focus Mode");
            TagNameMap.Put(TagFocusPixel, "Focus Pixel");
            TagNameMap.Put(TagSlowSync, "Slow Sync");
            TagNameMap.Put(TagPictureMode, "Picture Mode");
            TagNameMap.Put(TagExrAuto, "EXR Auto");
            TagNameMap.Put(TagExrMode, "EXR Mode");
            TagNameMap.Put(TagAutoBracketing, "Auto Bracketing");
            TagNameMap.Put(TagSequenceNumber, "Sequence Number");
            TagNameMap.Put(TagFinePixColor, "FinePix Color Setting");
            TagNameMap.Put(TagBlurWarning, "Blur Warning");
            TagNameMap.Put(TagFocusWarning, "Focus Warning");
            TagNameMap.Put(TagAutoExposureWarning, "AE Warning");
            TagNameMap.Put(TagGeImageSize, "GE Image Size");
            TagNameMap.Put(TagDynamicRange, "Dynamic Range");
            TagNameMap.Put(TagFilmMode, "Film Mode");
            TagNameMap.Put(TagDynamicRangeSetting, "Dynamic Range Setting");
            TagNameMap.Put(TagDevelopmentDynamicRange, "Development Dynamic Range");
            TagNameMap.Put(TagMinFocalLength, "Minimum Focal Length");
            TagNameMap.Put(TagMaxFocalLength, "Maximum Focal Length");
            TagNameMap.Put(TagMaxApertureAtMinFocal, "Maximum Aperture at Minimum Focal Length");
            TagNameMap.Put(TagMaxApertureAtMaxFocal, "Maximum Aperture at Maximum Focal Length");
            TagNameMap.Put(TagAutoDynamicRange, "Auto Dynamic Range");
            TagNameMap.Put(TagFacesDetected, "Faces Detected");
            TagNameMap.Put(TagFacePositions, "Face Positions");
            TagNameMap.Put(TagFaceRecInfo, "Face Detection Data");
            TagNameMap.Put(TagFileSource, "File Source");
            TagNameMap.Put(TagOrderNumber, "Order Number");
            TagNameMap.Put(TagFrameNumber, "Frame Number");
            TagNameMap.Put(TagParallax, "Parallax");
        }

        public FujifilmMakernoteDirectory()
        {
            SetDescriptor(new FujifilmMakernoteDescriptor(this));
        }

        public override string GetName()
        {
            return "Fujifilm Makernote";
        }

        protected override Dictionary<int?, string> GetTagNameMap()
        {
            return TagNameMap;
        }
    }
}