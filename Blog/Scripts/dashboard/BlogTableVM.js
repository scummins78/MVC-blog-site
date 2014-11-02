var BlogTableVM = function BlogTableVM() {

    var self = this;
    self.Posts = ko.observableArray();
    self.PostCount = ko.observable(0);
    self.CurrentPage = ko.observable(1);
    self.TotalPages = ko.observable(1);
    self.CurrentStartIndex = ko.observable(1);
    self.ItemsPerPage = ko.observable(10);
    self.IsLoading = ko.observable(false);

    self.CurrentEndIndex = ko.pureComputed({
        read: function () {
            return parseInt(self.CurrentStartIndex()) + parseInt(self.ItemsPerPage()) - 1;
        }
    });

    self.Load = function (page) {
        this.IsLoading(true);

        $.getJSON("/Blog/dashboard/bloglist/" + page + "?itemsPerPage=" + self.ItemsPerPage(), function (returnData) {

            if (returnData.Success) {
                self.Posts(returnData.Data.Posts);
                self.PostCount(returnData.Data.PostCount);
                self.CurrentPage(returnData.Data.CurrentPage);
                self.TotalPages(returnData.Data.TotalPages);
                self.CurrentStartIndex(returnData.Data.CurrentStartIndex + 1);
                self.ItemsPerPage(returnData.Data.ItemsPerPage);
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

    // initialize
    this.Load(1);
};
// TODO:  move this code to a central initialize area.  
$(document).ready(function () {
    var viewModel = new BlogTableVM();
    ko.applyBindings(viewModel, document.getElementById("blogtable"));
});
