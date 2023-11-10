using FunApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.ComponentModel;
using System.Web.UI.WebControls;


namespace FunApplication.ViewModels
{
    public class GameIndexViewModel
    {
        public IPagedList<Game> Games { get; set; }
        // public IQueryable<Game> Games { get; set; }
        public string Search { get; set; } //replaces the ViewBag.Search
        public IEnumerable<CategoryWithCount> CatsWithCount { get; set; } // will hold all of the CategoryWithCount items to be used inside the select control in the view.
        public string Category { get; set; } //will be used as the name of the select control in the view
        public string Publisher { get; set; }
        public string SortBy { get; set; }
        public Dictionary<string, string> Sorts { get; set; }

        public IEnumerable<SelectListItem> CatFilterItems
        {
            get
            {
                var allCats = CatsWithCount.Select(cc => new SelectListItem
                {
                    Value = cc.CategoryName,
                    Text = cc.CatNameWithCount
                });
                return allCats;
            }
        }
    }
    public class CategoryWithCount
    {
        public int GameCount { get; set; }
        public string CategoryName { get; set; }
        public string CatNameWithCount
        {
            get
            {
                return CategoryName + " (" + GameCount.ToString() + ")";
            }
        }
    }
}