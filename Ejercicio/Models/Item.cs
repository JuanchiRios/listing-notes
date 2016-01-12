using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Ejercicio.Models
{
	public class Item
	{
		public int? AvailableQuantity { get; set; }
		public ListingType ListingTypeId { get; set; }
		public string CurrencyId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string CategoryId { get; set; }
		public string Warranty { get; set; }
		public ListingCondition Condition { get; set; }
		public string VideoId { get; set; }
		public IList<PictureInfo> Pictures { get; set; }
		public string Id { get; set; }
		public string SiteId { get; set; }
		public long SellerId { get; set; }
		public string Permalink { get; set; }
		public string ParentItemId { get; set; }
		public double? Price { get; set; }
		public string Thumbnail { get; set; }
		public ListingStatus Status { get; set; }
		public string[] SubStatus { get; set; }
		public DateTime StopTime { get; set; }
}

	public enum ListingStatus { Active, Paused, Closed, UnderReview, Inactive, NotYetActive, PaymentRequired }
	public enum ListingType { GoldSpecial, GoldPremium, Gold, Silver, Bronze, Free, GoldPro }
	public enum ListingCondition { NotSpecified, New, Used }

	public class PictureInfo
	{
		public string Id { get; set; }
		public string Url { get; set; }
	}

}