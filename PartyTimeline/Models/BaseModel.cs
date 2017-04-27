﻿using System;
using SQLite;

namespace PartyTimeline
{
	[Table("BaseModel")]
	public class BaseModel : IComparable<BaseModel>
	{
		private static Random idGenerator = new Random(DateTime.Now.Millisecond);
		[PrimaryKey, Column("_id")]
		public long Id { get; set; }
		[Column("date_created"), NotNull]
		public DateTime DateCreated { get; set; }
		[Column("date_modified"), NotNull]
		public DateTime DateLastModified { get; set; }

		public BaseModel()
		{
			SetRandomId();
		}

		public BaseModel(DateTime dateCreated)
		{
			SetRandomId();
			SetDateCreated(dateCreated);
		}

		public void SetDateCreated(DateTime dateCreated)
		{
			DateCreated = dateCreated;
			DateLastModified = dateCreated;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is BaseModel))
			{
				return false;
			}
			return ((BaseModel)obj).Id == this.Id;
		}

		public override int GetHashCode()
		{
			return (int)Id;
		}

		/// <summary>
		/// Compares other with this instance using <see cref="BaseModel.SortDateCreatedDescending"/>.
		/// </summary>
		/// <returns>A comparison value</returns>
		/// <param name="other">Other instance of a <see cref="BaseModel"/></param>
		public int CompareTo(BaseModel other)
		{
			return this.DateLastModified.Subtract(other.DateLastModified).Milliseconds;
		}

		public bool ModifiedAfter(BaseModel other)
		{
			return CompareTo(other) > 0;
		}

		private void SetRandomId()
		{
			Id = idGenerator.Next();
		}
	}
}
