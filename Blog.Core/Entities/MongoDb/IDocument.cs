﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Blog.Core.Entities.MongoDb
{
	public interface IDocument
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		ObjectId Id { get; set; }

		DateTime CreatedAt { get; }
		DateTime UpdatedAt { get; set; }
		DateTime CreatedBy { get; set; }
	}
}