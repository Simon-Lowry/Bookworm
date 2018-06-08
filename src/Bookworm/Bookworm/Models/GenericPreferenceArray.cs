using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NReco.CF.Taste.Model;

namespace Bookworm.Models
{
    public class GenericPreferenceArray 
    {
        /// <summary>
        /// Size of length of the "array"
        /// </summary>
        public int Length()
        {
            throw new NotImplementedException();
        }


        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get preference at specified index
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>a materialized <see cref="IPreference"/> representation of the preference at i</returns>
        public IPreference Get(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets preference at i from information in the given <see cref="IPreference"/>
        /// </summary>
        /// <param name="i">index</param>
        /// <param name="pref">pref</param>
        public void Set(int i, IPreference pref)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get user ID from preference at specified index
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>user ID from preference at i</returns>
        public long GetUserID(int i)
        {
            throw new NotImplementedException();
        }

        /// Sets user ID for preference at i.
        /// 
        /// @param i
        ///          index
        /// @param userID
        ///          new user ID
        public void SetUserID(int i, long userID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get item ID from preference at specified index
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>item ID from preference at i</returns>
        public long GetItemID(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets item ID for preference at i.
        /// </summary>
        /// <param name="i">index</param>
        /// <param name="itemID">new item ID</param>
        public void SetItemID(int i, long itemID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all IDs
        /// </summary>
        /// <returns>all user or item IDs</returns>
        public long[] GetIDs()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get preference value
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>preference value from preference at i</returns>
        float GetValue(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets preference value for preference at i.
        /// </summary>
        /// <param name="i">index</param>
        /// <param name="value">new preference value</param>
        public void SetValue(int i, float value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clone object instance
        /// </summary>
        /// <returns>independent copy of this object</returns>
        public IPreferenceArray Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts underlying array by user ID, ascending.
        /// </summary>
        public void SortByUser()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts underlying array by item ID, ascending.
        /// </summary>
        public void SortByItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts underlying array by preference value, ascending.
        /// </summary>
        public void SortByValue()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts underlying array by preference value, descending.
        /// </summary>
        public void SortByValueReversed()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if array contains a preference with given user ID
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>true if array contains a preference with given user ID</returns>
        public bool HasPrefWithUserID(long userID)
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// Check if array contains a preference with given item ID
        /// </summary>
        /// <param name="itemID">item ID</param>
        /// <returns>true if array contains a preference with given item ID</returns>
        public bool HasPrefWithItemID(long itemID)
        {
            throw new NotImplementedException();
        }
    }
}