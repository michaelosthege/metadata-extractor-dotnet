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

using System;
using System.Collections.Generic;
using System.Linq;
using Com.Drew.Imaging.Jpeg;
using JetBrains.Annotations;
using Sharpen;

namespace Com.Drew.Tools
{
    /// <summary>Extracts JPEG segments and writes them to individual files.</summary>
    /// <remarks>
    /// Extracts JPEG segments and writes them to individual files.
    /// <p/>
    /// Extracting only the required segment(s) for use in unit testing has several benefits:
    /// <list type="bullet">
    /// <item>Helps reduce the repository size. For example a small JPEG image may still be 20kB+ in size, yet its
    /// APPD (IPTC) segment may be as small as 200 bytes.</item>
    /// <item>Makes unit tests run more rapidly.</item>
    /// <item>Partially anonymises user-contributed data by removing image portions.</item>
    /// </list>
    /// </remarks>
    /// <author>Drew Noakes https://drewnoakes.com</author>
    public class ExtractJpegSegmentTool
    {
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Com.Drew.Imaging.Jpeg.JpegProcessingException"/>
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                PrintUsage();
                Environment.Exit(1);
            }
            string filePath = args[0];
            if (!new FilePath(filePath).Exists())
            {
                Console.Error.WriteLine((object)"File does not exist");
                PrintUsage();
                Environment.Exit(1);
            }
            ICollection<JpegSegmentType> segmentTypes = new HashSet<JpegSegmentType>();
            for (int i = 1; i < args.Length; i++)
            {
                JpegSegmentType segmentType = JpegSegmentType.ValueOf(args[i].ToUpper());
                if (!segmentType.CanContainMetadata)
                {
                    Console.Error.WriteLine("WARNING: Segment type {0} cannot contain metadata so it may not be necessary to extract it", segmentType);
                }
                segmentTypes.Add(segmentType);
            }
            if (segmentTypes.Count == 0)
            {
                // If none specified, use all that could reasonably contain metadata
                Collections.AddAll(segmentTypes, JpegSegmentType.CanContainMetadataTypes);
            }
            Console.Out.WriteLine((object)("Reading: " + filePath));
            JpegSegmentData segmentData = JpegSegmentReader.ReadSegments(new FilePath(filePath), segmentTypes);
            SaveSegmentFiles(filePath, segmentData);
        }

        /// <exception cref="System.IO.IOException"/>
        public static void SaveSegmentFiles([NotNull] string jpegFilePath, [NotNull] JpegSegmentData segmentData)
        {
            foreach (JpegSegmentType segmentType in segmentData.GetSegmentTypes())
            {
                IList<byte[]> segments = segmentData.GetSegments(segmentType).ToList();
                if (segments.Count == 0)
                {
                    continue;
                }
                if (segments.Count > 1)
                {
                    for (int i = 0; i < segments.Count; i++)
                    {
                        string outputFilePath = string.Format("{0}.{1}.{2}", jpegFilePath, Extensions.ConvertToString(segmentType).ToLower(), i);
                        Console.Out.WriteLine((object)("Writing: " + outputFilePath));
                        FileUtil.SaveBytes(new FilePath(outputFilePath), segments[i]);
                    }
                }
                else
                {
                    string outputFilePath = string.Format("{0}.{1}", jpegFilePath, Extensions.ConvertToString(segmentType).ToLower());
                    Console.Out.WriteLine((object)("Writing: " + outputFilePath));
                    FileUtil.SaveBytes(new FilePath(outputFilePath), segments[0]);
                }
            }
        }

        private static void PrintUsage()
        {
            Console.Out.WriteLine("USAGE:\n");
            Console.Out.WriteLine((object)"\tjava com.drew.tools.ExtractJpegSegmentTool <filename> [<segment> ...]\n");
            Console.Out.Write((object)"Where <segment> is zero or more of:");
            foreach (JpegSegmentType segmentType in typeof(JpegSegmentType).GetEnumConstants<JpegSegmentType>())
            {
                if (segmentType.CanContainMetadata)
                {
                    Console.Out.Write((object)(" " + Extensions.ConvertToString(segmentType)));
                }
            }
            Console.Out.WriteLine();
        }
    }
}