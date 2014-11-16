define("RecentPostsVM", ["DataService"], function (dataService) {

    var RecentPostsVM = function RecentPostsVM() {

        var self = this;
        self.Posts = ko.observableArray();
        self.IsLoading = ko.observable(false);

        self.Load = function () {
            this.IsLoading(true);
            
            dataService.getRecentPosts().done(function (data) {
                self.Posts(data);
            });
            self.IsLoading(false);
        };

        // initialize
        this.Load();
    };

    return RecentPostsVM;
});

require(["RecentPostsVM"], function (RecentPostsVM) {
    var viewModel = new RecentPostsVM();
    ko.applyBindings(viewModel, document.getElementById("postList"));
});

