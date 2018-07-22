#region License
//
// Copyright 2002-2017 Drew Noakes
// Ported from Java to C# by Yakov Danilov for Imazen LLC in 2014
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
// More information about this project is available at:
//
//    https://github.com/drewnoakes/metadata-extractor-dotnet
//    https://drewnoakes.com/code/exif/
//
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.Xmp;
using MetadataExtractor.IO;
using Xunit;

namespace MetadataExtractor.Tests
{
    /// <summary>Unit tests for <see cref="ImageMetadataWriter"/>.</summary>
    /// <author>Michael Osthege</author>
    public sealed class ImageMetadataWriterTest
    {
        [Fact]
        public void TestWriteImageMetadata()
        {
            var originalStream = TestDataUtil.OpenRead("Data/xmpWriting_PictureWithMicrosoftXmp.jpg");
            XmpDirectory xmpDir = new XmpDirectory();
            xmpDir.SetXmpMetaElement(XElement.Parse(File.ReadAllText("Data/xmpWriting_XmpContent.xmp")));
            byte[] expectedResult = TestDataUtil.GetBytes("Data/xmpWriting_PictureWithMicrosoftXmpReencoded.jpg");

            var updatedStream = ImageMetadataWriter.WriteMetadata(originalStream, new Directory[] { xmpDir });

            var actualResult = updatedStream.ToArray();

            Assert.True(actualResult.SequenceEqual(expectedResult));
        }

        [Fact]
        public void TestXmpWriteReadIsEquivalent()
        {
            var originalStream = TestDataUtil.OpenRead("Data/xmpWriting_PictureWithoutXmp.jpg");
            XmpDirectory initialDir = new XmpDirectory();
            initialDir.SetXmpMetaElement(XElement.Parse(File.ReadAllText("Data/xmpWriting_XmpContent.xmp")));

            // write fresh Xmp into the picture
            var writtenStream = ImageMetadataWriter.WriteMetadata(originalStream, new Directory[] { initialDir });
            writtenStream.Seek(0, SeekOrigin.Begin);

            // read the Xmp back
            XmpDirectory reloadedDir = ImageMetadataReader.ReadMetadata(writtenStream).Where(d => d is XmpDirectory).FirstOrDefault() as XmpDirectory;

            // check that the Xmp did not change from write & read
            XElement norm_original = TestHelper.NormalizeWithoutAnyOrder(initialDir.XmpMetaElement);
            XElement norm_reloaded = TestHelper.NormalizeWithoutAnyOrder(reloadedDir.XmpMetaElement);
            Assert.True(XNode.DeepEquals(norm_original, norm_reloaded));
        }

        [Fact]
        public void TestSecondWriteFromInitialEqualsFirstWrite()
        {
            var originalStream = TestDataUtil.OpenRead("Data/xmpWriting_PictureWithMicrosoftXmp.jpg");
            XmpDirectory initialDir = new XmpDirectory();
            initialDir.SetXmpMetaElement(XElement.Parse(File.ReadAllText("Data/xmpWriting_XmpContent.xmp")));

            // overwrite the existing Xmp
            var firstWriteStream = ImageMetadataWriter.WriteMetadata(originalStream, new Directory[] { initialDir });
            firstWriteStream.Seek(0, SeekOrigin.Begin);

            // overwrite the result of the first write again with the same Xmp
            var secondWriteStream = ImageMetadataWriter.WriteMetadata(firstWriteStream, new Directory[] { initialDir });
            
            // check that overwriting again with the same content does not change the result
            Assert.True(firstWriteStream.ToArray().SequenceEqual(secondWriteStream.ToArray()));
        }

        [Fact]
        public void TestRepeatedReadWriteRemainsStable()
        {
            var originalVersion = TestDataUtil.OpenRead("Data/xmpWriting_PictureWithMicrosoftXmp.jpg");
            XmpDirectory originalDir = ImageMetadataReader.ReadMetadata(originalVersion).Where(d => d is XmpDirectory).FirstOrDefault() as XmpDirectory;

            // write for the first time - may cause difference to the original
            originalVersion.Seek(0, SeekOrigin.Begin);
            var currentVersion = ImageMetadataWriter.WriteMetadata(originalVersion, new Directory[] { originalDir });
            Debug.WriteLine($"Length changed from initially {originalVersion.Length} to {currentVersion.Length}");
            
            for (int i = 0; i < 3; i++)
            {
                currentVersion.Seek(0, SeekOrigin.Begin);
                XmpDirectory readDir = ImageMetadataReader.ReadMetadata(currentVersion).Where(d => d is XmpDirectory).FirstOrDefault() as XmpDirectory;

                currentVersion.Seek(0, SeekOrigin.Begin);
                var nextVersion = ImageMetadataWriter.WriteMetadata(currentVersion, new Directory[] { readDir });
                Debug.WriteLine($"Length changed from {currentVersion.Length} to {nextVersion.Length}");
                Assert.True(currentVersion.ToArray().SequenceEqual(nextVersion.ToArray()));
                currentVersion = nextVersion;
            }
        }
    }
}
