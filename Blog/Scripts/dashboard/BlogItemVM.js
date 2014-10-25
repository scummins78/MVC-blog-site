var BlogItemVM = function BlogItemVM() {

    var self = this;
    self.Posts = ko.observableArray();
    self.IsLoading = ko.observable(false);

    self.Load = function (page) {
        this.IsLoading(true);

        $.getJSON("/Blog/dashboard/bloglist/" + page, function (returnData) {

            if (returnData.Success) {
                self.Posts(returnData.Data);
            }

            self.IsLoading(false);
        });
    };

    self.LoadPage = function (page) {
        this.Load(page);
    }

    // initialize
    this.Load(1);
};
// TODO:  move this code to a central initialize area.  
$(document).ready(function () {
    var viewModel = new BlogItemVM();
    ko.applyBindings(viewModel, document.getElementById("blogtable"));
});
