var RecentPostsVM = function RecentPostsVM() {
    
    var self = this;
    self.Posts = ko.observableArray();
    self.IsLoading = ko.observable(false);

    self.Load = function () {
        this.IsLoading(true);

        $.getJSON("/Blog/widget/recentlist", function (returnData) {

            if (returnData.Success) {
                self.Posts(returnData.Data);
            }

            self.IsLoading(false);
        });
    };

    // initialize
    this.Load();
};
// TODO:  move this code to a central initialize area.  
$(document).ready(function () {
    var viewModel = new RecentPostsVM();
    ko.applyBindings(viewModel, document.getElementById("postList"));
});
