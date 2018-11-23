using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using TiltbrushToolkit.Attributes;
using TiltbrushToolkit.Exceptions;

namespace TiltbrushToolkit
{
    public class Metadata
    {
        public enum BrushType
        {
            [BrushId("c515dad7-4393-4681-81ad-162ef052241b")]
            OilPaint,
            [BrushId("c0012095-3ffd-4040-8ee1-fc180d346eaa")]
            Ink,
            [BrushId("fdf0326a-c0d1-4fed-b101-9db0ff6d071f")]
            ThickPaint,
            [BrushId("dea67637-cd1a-27e4-c9b1-52f4bbcb84e5")]
            WetPaint,
            [BrushId("429ed64a-4e97-4466-84d3-145a861ef684")]
            Marker,
            [BrushId("d90c6ad8-af0f-4b54-b422-e0f92abe1b3c")]
            TaperedMarker,
            [BrushId("0d3889f3-3ede-470c-8af4-de4813306126")]
            PinchedMarker,
            [BrushId("cf019139-d41c-4eb0-a1d0-5cf54b0a42f3")]
            Highlighter,
            [BrushId("280c0a7a-aad8-416c-a7d2-df63d129ca70")]
            Flat,
            [BrushId("c8ccb53d-ae13-45ef-8afb-b730d81394eb")]
            TaperedFlat,
            [BrushId("0d3889f3-3ede-470c-8af4-f44813306126")]
            PinchedFlat,
            [BrushId("accb32f5-4509-454f-93f8-1df3fd31df1b")]
            SoftHighlighter,
            [BrushId("cf7f0059-7aeb-53a4-2b67-c83d863a9ffa")]
            Spikes,
            [BrushId("2f212815-f4d3-c1a4-681a-feeaf9c6dc37")]
            Icing,
            Custom
        }
        //not everything contained in this class due to limitations of parsing JSON through standard .NET JavaScript Serializer class
        public Guid EnvironmentPreset { get; set; }
        public string[] Authors { get; set; }

        public int SchemaVersion = 1;
        public Guid[] BrushIndex { get; set; }
        public string ToJSON()
        {
            var obj = new
            {
                EnvironmentPreset = EnvironmentPreset,
                Authors = Authors,
                BrushIndex = BrushIndex,
                SchemaVersion = SchemaVersion
            };
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            return serialize.Serialize(obj);
        }
        public void AddBrush(BrushType brush, Guid? customGuid = null)
        {
            Guid brushGuid = Guid.Empty;
            if (brush != BrushType.Custom)
            {
                brushGuid = GetBrushGuid(brush);
            }
            else
            {
                if (!customGuid.HasValue)
                {
                    customGuid = Guid.Empty;
                }
                brushGuid = customGuid.Value;
            }
            if (brushGuid != Guid.Empty)
            {
                if (BrushIndex == null)
                {
                    BrushIndex = new Guid[1] { brushGuid };
                }
                else
                {
                    Guid[] temp = new Guid[BrushIndex.Length + 1];
                    BrushIndex.CopyTo(temp, 0);
                    temp[BrushIndex.Length] = brushGuid;
                    BrushIndex = temp;
                }
            }
        }
        public int GetBrushIndex(BrushType brush, Guid? customGuid = null)
        {
            Guid brushGuid = Guid.Empty;

            if (brush != BrushType.Custom)
            {
                brushGuid = GetBrushGuid(brush);
            }
            else
            {
                if (!customGuid.HasValue)
                {
                    customGuid = Guid.Empty;
                }
                brushGuid = customGuid.Value;
            }
            //just perform a basic loop through the array to get the index, 
            int index = -1;
            for(int i = 0; i < BrushIndex.Length; i++)
            {
                if(BrushIndex[i] == brushGuid)
                {
                    index = i;
                    break;
                }
            }
            if(index == -1)
            {
                //if no brush found then throw an exception to prevent corrupt files from being written.
                throw new BrushNotFoundException($"Brush: {brush.ToString()}, has not been added to the brush collection");
            }
            return index; 
        }
        private Guid GetBrushGuid(BrushType brush)
        {
            Guid brushGuid = Guid.Empty;
            if (brush != BrushType.Custom)
            {
                //get the guid from the enum attribute
                var attr = typeof(BrushType).GetMember(brush.ToString())[0].GetCustomAttributes(typeof(BrushIdAttribute), false)[0] as BrushIdAttribute;
                if (attr != null)
                {
                    brushGuid = attr.BrushGuid;
                }
            }
            return brushGuid;
        }
        public static Metadata FromJson(string json)
        {
            try
            {
                JavaScriptSerializer serialize = new JavaScriptSerializer();
                var metaData = serialize.Deserialize<Metadata>(json);
                return metaData;
            }
            catch(Exception ex)
            {
                throw new BadMetadataException(ex.Message);
            }
        }
    }
}
