﻿var BlogTableVM = function BlogTableVM() {

    var self = this;
    self.Posts = ko.observableArray();
    self.PostCount = ko.observable(0);
    self.CurrentPage = ko.observable(1);
    self.TotalPages = ko.observable(1);
    self.CurrentStartIndex = ko.observable(1);
    self.ItemsPerPage = ko.observable(10);
    self.IsLoading = ko.observable(false);

    self.ItemCountChanged = function (event) {
        self.Load(self.CurrentPage());
    };

    self.CurrentEndIndex = ko.pureComputed({
        read: function () {
            return parseInt(self.CurrentStartIndex()) + parseInt(self.ItemsPerPage()) - 1;
        }
    });

    self.Pages = ko.pureComputed({
        read: function () {
            var pages = [];
            for (var i = 0; i < self.TotalPages(); i++) {
                pages.push(i);
            }

            return pages;
        }
    });

    self.Load = function (page, sort) {
        this.IsLoading(true);

        var url = "/Blog/dashboard/bloglist/" + page + "?itemsPerPage=" + self.ItemsPerPage();
        if (sort !== undefined) {
            url = url + "&sort=" + sort;
        }

        $.getJSON(url, function (returnData) {

            if (returnData.Success) {
                //self.ItemsPerPage(returnData.Data.ItemsPerPage);
                self.Posts(returnData.Data.Posts);
                self.PostCount(returnData.Data.PostCount);
                self.CurrentPage(returnData.Data.CurrentPage);
                self.TotalPages(returnData.Data.TotalPages);
                self.CurrentStartIndex(returnData.Data.CurrentStartIndex + 1);
            }

            self.IsLoading(false);
        });
    };

    self.NextPage = function () {
        this.Load(this.CurrentPage() + 1);
    };

    self.PreviousPage = function () {
        this.Load(this.CurrentPage() - 1);
    };

    self.Sort = function (element, sortColumn) {
        this.Load(this.CurrentPage(), sortColumn);
    }

    // initialize
    this.Load(1);
};
// TODO:  move this code to a central initialize area.  
$(document).ready(function () {
    var viewModel = new BlogTableVM();
    ko.applyBindings(viewModel, document.getElementById("blogtable"));
});
