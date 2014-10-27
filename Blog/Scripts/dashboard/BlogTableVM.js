var BlogTableVM = function BlogTableVM() {

    var self = this;
    self.Posts = ko.observableArray();
    self.PostCount = ko.observable(0);
    self.CurrentPage = ko.observable(1);
    self.TotalPages = ko.observable(1);
    self.IsLoading = ko.observable(false);

    self.Load = function (page) {
        this.IsLoading(true);

        $.getJSON("/Blog/dashboard/bloglist/" + page, function (returnData) {

            if (returnData.Success) {
                self.Posts(returnData.Data.Posts);
                self.PostCount(returnData.Data.PostCount);
                self.CurrentPage(returnData.Data.CurrentPage);
                self.TotalPages(returnData.Data.TotalPages);
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
