using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing;

namespace Triangulation;

public class PointFJsonConverter : JsonConverter<PointF>
{
	public override bool CanRead => true;
	public override bool CanWrite => true;

	public override PointF ReadJson(JsonReader reader, Type objectType, PointF existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		var json = JObject.Load(reader);

		return new PointF(json.Value<float>("x"),
			json.Value<float>("y"));
	}

	public override void WriteJson(JsonWriter writer, PointF value, JsonSerializer serializer)
	{
		var json = new JObject
		{
			["x"] = value.X,
			["y"] = value.Y,
		};

		json.WriteTo(writer);
	}
}
