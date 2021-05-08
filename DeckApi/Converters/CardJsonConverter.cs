using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using CardLib;

namespace DeckApi.Converters
{
    public class CardJsonConverter : JsonConverter<Card>
    {
        private static Dictionary<string, CardSuit> stringToSuitDict;
        private static Dictionary<string, CardRank> stringToRankDict;

        static CardJsonConverter()
        {
            stringToSuitDict = ((CardSuit[]) Enum.GetValues(typeof(CardSuit)))
                .ToDictionary(suit => suit.ToString(), suit => suit);
            stringToRankDict = ((CardRank[]) Enum.GetValues(typeof(CardRank)))
                .ToDictionary(rank => rank.ToString(), rank => rank);
        }
        public override Card Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var suit = CardSuit.NonSet;
            var rank = CardRank.NonSet;

            var propertyName = "";
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new Card(suit, rank);
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "Suit":
                            suit = stringToSuitDict[reader.GetString()];
                            break;
                        case "Rank":
                            rank = stringToRankDict[reader.GetString()];
                            break;
                    }
                }
            }
            
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Card value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            writer.WriteString("Suit", value.Suit.ToString());
            writer.WriteString("Rank", value.Rank.ToString());

            writer.WriteEndObject();
        }
    }
}