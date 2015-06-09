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

using Com.Drew.Lang;
using JetBrains.Annotations;
using Sharpen;

namespace Com.Drew.Metadata.Exif
{
    /// <summary>
    /// Provides human-readable string representations of tag values stored in a <see cref="GpsDirectory"/>.
    /// </summary>
    /// <author>Drew Noakes https://drewnoakes.com</author>
    public sealed class GpsDescriptor : TagDescriptor<GpsDirectory>
    {
        public GpsDescriptor([NotNull] GpsDirectory directory)
            : base(directory)
        {
        }

        public override string GetDescription(int tagType)
        {
            switch (tagType)
            {
                case GpsDirectory.TagVersionId:
                {
                    return GetGpsVersionIdDescription();
                }

                case GpsDirectory.TagAltitude:
                {
                    return GetGpsAltitudeDescription();
                }

                case GpsDirectory.TagAltitudeRef:
                {
                    return GetGpsAltitudeRefDescription();
                }

                case GpsDirectory.TagStatus:
                {
                    return GetGpsStatusDescription();
                }

                case GpsDirectory.TagMeasureMode:
                {
                    return GetGpsMeasureModeDescription();
                }

                case GpsDirectory.TagSpeedRef:
                {
                    return GetGpsSpeedRefDescription();
                }

                case GpsDirectory.TagTrackRef:
                case GpsDirectory.TagImgDirectionRef:
                case GpsDirectory.TagDestBearingRef:
                {
                    return GetGpsDirectionReferenceDescription(tagType);
                }

                case GpsDirectory.TagTrack:
                case GpsDirectory.TagImgDirection:
                case GpsDirectory.TagDestBearing:
                {
                    return GetGpsDirectionDescription(tagType);
                }

                case GpsDirectory.TagDestDistanceRef:
                {
                    return GetGpsDestinationReferenceDescription();
                }

                case GpsDirectory.TagTimeStamp:
                {
                    return GetGpsTimeStampDescription();
                }

                case GpsDirectory.TagLongitude:
                {
                    // three rational numbers -- displayed in HH"MM"SS.ss
                    return GetGpsLongitudeDescription();
                }

                case GpsDirectory.TagLatitude:
                {
                    // three rational numbers -- displayed in HH"MM"SS.ss
                    return GetGpsLatitudeDescription();
                }

                case GpsDirectory.TagDifferential:
                {
                    return GetGpsDifferentialDescription();
                }

                default:
                {
                    return base.GetDescription(tagType);
                }
            }
        }

        [CanBeNull]
        private string GetGpsVersionIdDescription()
        {
            return GetVersionBytesDescription(GpsDirectory.TagVersionId, 1);
        }

        [CanBeNull]
        public string GetGpsLatitudeDescription()
        {
            GeoLocation location = Directory.GetGeoLocation();
            return location == null ? null : GeoLocation.DecimalToDegreesMinutesSecondsString(location.GetLatitude());
        }

        [CanBeNull]
        public string GetGpsLongitudeDescription()
        {
            GeoLocation location = Directory.GetGeoLocation();
            return location == null ? null : GeoLocation.DecimalToDegreesMinutesSecondsString(location.GetLongitude());
        }

        [CanBeNull]
        public string GetGpsTimeStampDescription()
        {
            // time in hour, min, sec
            Rational[] timeComponents = Directory.GetRationalArray(GpsDirectory.TagTimeStamp);
            DecimalFormat df = new DecimalFormat("00.00");
            return timeComponents == null ? null : string.Format("{0:D2}:{1:D2}:{2} UTC", timeComponents[0].IntValue(), timeComponents[1].IntValue(), df.Format(timeComponents[2].DoubleValue()));
        }

        [CanBeNull]
        public string GetGpsDestinationReferenceDescription()
        {
            string value = Directory.GetString(GpsDirectory.TagDestDistanceRef);
            if (value == null)
            {
                return null;
            }
            string distanceRef = Extensions.Trim(value);
            if (Runtime.EqualsIgnoreCase("K", distanceRef))
            {
                return "kilometers";
            }
            if (Runtime.EqualsIgnoreCase("M", distanceRef))
            {
                return "miles";
            }
            if (Runtime.EqualsIgnoreCase("N", distanceRef))
            {
                return "knots";
            }
            return "Unknown (" + distanceRef + ")";
        }

        [CanBeNull]
        public string GetGpsDirectionDescription(int tagType)
        {
            Rational angle = Directory.GetRational(tagType);
            // provide a decimal version of rational numbers in the description, to avoid strings like "35334/199 degrees"
            string value = angle != null ? new DecimalFormat("0.##").Format(angle.DoubleValue()) : Directory.GetString(tagType);
            return value == null || Extensions.Trim(value).Length == 0 ? null : Extensions.Trim(value) + " degrees";
        }

        [CanBeNull]
        public string GetGpsDirectionReferenceDescription(int tagType)
        {
            string value = Directory.GetString(tagType);
            if (value == null)
            {
                return null;
            }
            string gpsDistRef = Extensions.Trim(value);
            if (Runtime.EqualsIgnoreCase("T", gpsDistRef))
            {
                return "True direction";
            }
            if (Runtime.EqualsIgnoreCase("M", gpsDistRef))
            {
                return "Magnetic direction";
            }
            return "Unknown (" + gpsDistRef + ")";
        }

        [CanBeNull]
        public string GetGpsSpeedRefDescription()
        {
            string value = Directory.GetString(GpsDirectory.TagSpeedRef);
            if (value == null)
            {
                return null;
            }
            string gpsSpeedRef = Extensions.Trim(value);
            if (Runtime.EqualsIgnoreCase("K", gpsSpeedRef))
            {
                return "kph";
            }
            if (Runtime.EqualsIgnoreCase("M", gpsSpeedRef))
            {
                return "mph";
            }
            if (Runtime.EqualsIgnoreCase("N", gpsSpeedRef))
            {
                return "knots";
            }
            return "Unknown (" + gpsSpeedRef + ")";
        }

        [CanBeNull]
        public string GetGpsMeasureModeDescription()
        {
            string value = Directory.GetString(GpsDirectory.TagMeasureMode);
            if (value == null)
            {
                return null;
            }
            string gpsSpeedMeasureMode = Extensions.Trim(value);
            if (Runtime.EqualsIgnoreCase("2", gpsSpeedMeasureMode))
            {
                return "2-dimensional measurement";
            }
            if (Runtime.EqualsIgnoreCase("3", gpsSpeedMeasureMode))
            {
                return "3-dimensional measurement";
            }
            return "Unknown (" + gpsSpeedMeasureMode + ")";
        }

        [CanBeNull]
        public string GetGpsStatusDescription()
        {
            string value = Directory.GetString(GpsDirectory.TagStatus);
            if (value == null)
            {
                return null;
            }
            string gpsStatus = Extensions.Trim(value);
            if (Runtime.EqualsIgnoreCase("A", gpsStatus))
            {
                return "Active (Measurement in progress)";
            }
            if (Runtime.EqualsIgnoreCase("V", gpsStatus))
            {
                return "Void (Measurement Interoperability)";
            }
            return "Unknown (" + gpsStatus + ")";
        }

        [CanBeNull]
        public string GetGpsAltitudeRefDescription()
        {
            return GetIndexedDescription(GpsDirectory.TagAltitudeRef, "Sea level", "Below sea level");
        }

        [CanBeNull]
        public string GetGpsAltitudeDescription()
        {
            Rational value = Directory.GetRational(GpsDirectory.TagAltitude);
            return value == null ? null : value.IntValue() + " metres";
        }

        [CanBeNull]
        public string GetGpsDifferentialDescription()
        {
            return GetIndexedDescription(GpsDirectory.TagDifferential, "No Correction", "Differential Corrected");
        }

        [CanBeNull]
        public string GetDegreesMinutesSecondsDescription()
        {
            GeoLocation location = Directory.GetGeoLocation();
            return location == null ? null : location.ToDmsString();
        }
    }
}