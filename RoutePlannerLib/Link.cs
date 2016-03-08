using System;
using System.Collections.Generic;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
	public enum TransportMode
	{
		Ship,
		Rail,
		Flight,
		Car,
		Bus,
		Tram
	};
	
	/// <summary>
	/// Represents a link between two cities with its distance
	/// </summary>
	public class Link : IComparable
	{
		private City fromCity;
		private City toCity;
		double distance;
		
		TransportMode transportMode = TransportMode.Car;
		
		public TransportMode TransportMode
		{
			get
			{
				return transportMode;
			}
		}

		public City FromCity
		{
			get
			{
				return fromCity;
			}
		}

		public City ToCity
		{
			get
			{
				return toCity;
			}
		}

		public Link(City _fromCity, City _toCity, double _distance)
		{
			fromCity = _fromCity;
			toCity = _toCity;
			distance = _distance;
		}

		public Link(City _fromCity, City _toCity, double _distance, TransportMode _transportMode) : this(_fromCity, _toCity, _distance)
		{
			transportMode = _transportMode;
		}

		public double Distance
		{
			get
			{
				return distance;
			}
		}
		
		/// <summary>
		/// Uses distance as default comparison criteria 
		/// </summary>
		public int CompareTo(object o)
		{
			return distance.CompareTo(((Link)o).distance);
		}
		
		/// <summary>
		/// checks if both cities of the link are included in the passed city list
		/// </summary>
		/// <param name="cities">list of city objects</param>
		/// <returns>true if both link-cities are in the list</returns>
		internal bool IsIncludedIn(List<City> cities)
		{
			var foundFrom = false;
			var foundTo = false;
			foreach (var c in cities)
			{
				if (!foundFrom && c.Name == FromCity.Name)
					foundFrom = true;
				
				if (!foundTo && c.Name == ToCity.Name)
					foundTo = true;
			}
			
			return foundTo && foundFrom;
		}
	}
}
