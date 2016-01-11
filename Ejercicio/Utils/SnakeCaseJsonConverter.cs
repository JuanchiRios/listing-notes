using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Extensions;
using RestSharp.Serializers;

namespace Ejercicio.Utils
{
	/// <summary>
	/// Default JSON serializer for request bodies
	/// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
	/// </summary>
	public class SnakeCaseJsonConverter : ISerializer, IDeserializer
	{
		/// <summary>
		/// Default serializer
		/// </summary>
		public SnakeCaseJsonConverter()
		{
			this.ContentType = "application/json; charset=utf-8";
		}

		/// <summary>
		/// Serialize the object as JSON
		/// </summary>
		/// <param name="obj">Object to serialize</param>
		/// <returns>JSON as String</returns>
		public string Serialize(object obj)
		{

			return JsonConvert.SerializeObject(obj, this.Settings);
		}

		public T Deserialize<T>(IRestResponse response)
		{
			return JsonConvert.DeserializeObject<T>(response.Content, this.Settings);

		}

		private JsonSerializerSettings Settings
		{
			get
			{
				return new JsonSerializerSettings
				{
					MissingMemberHandling = MissingMemberHandling.Ignore,
					NullValueHandling = NullValueHandling.Ignore,
					DefaultValueHandling = DefaultValueHandling.Include,
					ContractResolver = new SnakeCasePropertiesContractResolver(),
					Converters = new JsonConverter[] { new SnakeCaseEnumConverter() }
				};
			}
		}

		/// <summary>
		/// Unused for JSON Serialization
		/// </summary>
		public string DateFormat { get; set; }

		/// <summary>
		/// Unused for JSON Serialization
		/// </summary>
		public string RootElement { get; set; }

		/// <summary>
		/// Unused for JSON Serialization
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// Content type for serialized content
		/// </summary>
		public string ContentType { get; set; }
	}

	public class SnakeCaseEnumConverter : StringEnumConverter
	{
		public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			var val = Enum.GetName(value.GetType(), value).ToSnakeCase();
			serializer.Serialize(writer, val);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
				return null;

			var value = serializer
				.Deserialize<string>(reader)
				.ToPascalCase(CultureInfo.CurrentCulture);

			return ParseEnum(objectType, value);
		}

		/// <summary>
		/// Get value of the given <typeparam name="TEnum"></typeparam> from a string
		/// </summary>
		private static object ParseEnum(Type enumType, string value)
		{
			if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
				return ParseNullableEnum(enumType, value);
			return Enum.Parse(enumType, value, true);
		}

		/// <summary>
		/// Get value of the given Generic Argument <typeparam name="Nullable<TEnum>"></typeparam> from a string
		/// </summary>
		private static object ParseNullableEnum(Type nullableEnumType, string value)
		{
			var genericType = nullableEnumType.GetGenericArguments().First();
			return ParseEnum(genericType, value);
		}
	}

	/// <summary>
	/// Resolves member mappings for a type, snake casing property names.
	/// </summary>
	/// <remarks>https://github.com/ayoung/Newtonsoft.Json/blob/master/Newtonsoft.Json/Serialization/CamelCasePropertyNamesContractResolver.cs</remarks>
	public class SnakeCasePropertiesContractResolver : DefaultContractResolver
	{

		protected override string ResolvePropertyName(string propertyName)
		{
			// lower case the first letter of the passed in name
			return propertyName.ToSnakeCase();
		}
	}
}